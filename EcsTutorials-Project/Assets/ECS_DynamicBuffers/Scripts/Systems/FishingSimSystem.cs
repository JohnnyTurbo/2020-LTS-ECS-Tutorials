using Unity.Entities;
using UnityEngine;

namespace TMG.DynamicBuffers
{
    public class FishingSimSystem : SystemBase
    {
        private DynamicBuffer<FishLengthBufferElement> _fishLog;
        
        private FishingSimData _fishingSimData;
        private FishingSimUI _fishingSimUI;
        
        protected override void OnStartRunning()
        {
            #region ControllerSingleton

            var controllerEntity = GetSingletonEntity<FishingSimData>();
            _fishingSimData = EntityManager.GetComponentData<FishingSimData>(controllerEntity);
            _fishingSimUI = EntityManager.GetComponentData<FishingSimUI>(controllerEntity);
            
            #endregion
            
            var fisherman = EntityManager.CreateEntity();
            _fishLog = EntityManager.AddBuffer<FishLengthBufferElement>(fisherman);
        }

        protected override void OnUpdate()
        {
            // If the game is over or the space key is not pressed, return
            if (!_fishingSimData.CanFish || !Input.GetKeyDown(KeyCode.Space)) return;

            // Otherwise, proceed with fishing

            #region CatchFishRegion

            _fishingSimData.CaughtFish = !_fishingSimData.CaughtFish;
            SetSingleton(_fishingSimData);
            
            _fishingSimUI.Fish.SetActive(_fishingSimData.CaughtFish);
            _fishingSimUI.FishCaughtText.gameObject.SetActive(_fishingSimData.CaughtFish);
            _fishingSimUI.LowerFishingLine.SetActive(!_fishingSimData.CaughtFish);

            #endregion
            
            if (_fishingSimData.CaughtFish)
            {
                // Get random fish length
                var fishLength = _fishingSimData.NextFishLength;
                //_fishLog.Add(new FishLengthBufferElement {Value = 6});
                _fishLog.Add(fishLength);
                _fishingSimUI.FishCaughtText.text = $"You Caught a\n<size=400>{fishLength}\"</size>\nTrout!";
            }
            
            // Once you've caught 8 fish
            if (!_fishingSimData.CaughtFish && _fishLog.Length >= 8)
            {
                _fishingSimData.CanFish = false;
                SetSingleton(_fishingSimData);
                
                var outputStr = "You caught fish with lengths of:\n";
                var totalLength = 0;
                for (var i = 0; i < _fishLog.Length; i++)
                {
                    outputStr += $"{_fishLog[i].Value}\" - ";
                    totalLength += _fishLog[i];
                }

                #region DisplayEndUIRegion

                outputStr = outputStr.Remove(outputStr.Length - 3, 3);
                outputStr += $"\nTotal length of {totalLength}!";
                _fishingSimUI.ScreenDimmer.SetActive(true);
                _fishingSimUI.FinalText.gameObject.SetActive(true);
                _fishingSimUI.FinalText.text = outputStr;

                #endregion
            }
        }
    }
}