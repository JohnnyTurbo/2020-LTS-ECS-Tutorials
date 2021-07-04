using Unity.Entities;
using UnityEngine;

namespace TMG.DynamicBuffers
{
    public class FishingSimSystem : SystemBase
    {
        private Entity _fisherman;
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
            
            _fisherman = EntityManager.CreateEntity();
            _fishLog = EntityManager.AddBuffer<FishLengthBufferElement>(_fisherman);
        }

        protected override void OnUpdate()
        {
            if (!_fishingSimData.CanFish || !Input.GetKeyDown(KeyCode.Space)) return;

            _fishingSimData.CaughtFish = !_fishingSimData.CaughtFish;
            SetSingleton(_fishingSimData);
            
            _fishingSimUI.Fish.SetActive(_fishingSimData.CaughtFish);
            _fishingSimUI.FishCaughtText.gameObject.SetActive(_fishingSimData.CaughtFish);
            _fishingSimUI.LowerFishingLine.SetActive(!_fishingSimData.CaughtFish);
            
            if (_fishingSimData.CaughtFish)
            {
                var fishLength = _fishingSimData.NextFishLength;
                //_fishLog.Add(new FishLengthBufferElement {Value = 6});
                _fishLog.Add(fishLength);
                _fishingSimUI.FishCaughtText.text = $"You Caught a\n<size=400>{fishLength}\"</size>\nTrout!";
            }
            
            
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

                outputStr = outputStr.Remove(outputStr.Length - 3, 3);
                outputStr += $"\nTotal length of {totalLength}!";
                _fishingSimUI.ScreenDimmer.SetActive(true);
                _fishingSimUI.FinalText.gameObject.SetActive(true);
                _fishingSimUI.FinalText.text = outputStr;
            }
        }
    }
}