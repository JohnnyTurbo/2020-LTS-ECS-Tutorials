using Unity.Entities;
using UnityEngine;

namespace TMG.ConnectFour
{
    public class ReloadSceneSystem : SystemBase
    {
        
        
        private Entity _gameControllerEntity;
        private ReloadSceneKey _reloadSceneKey;
        
        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<ReloadSceneKey>();
            _gameControllerEntity = GetSingletonEntity<ReloadSceneKey>();
            _reloadSceneKey = EntityManager.GetComponentData<ReloadSceneKey>(_gameControllerEntity);
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(_reloadSceneKey.Value))
            {
                var pieceQuery = EntityManager.CreateEntityQuery(typeof(HorizontalPosition));
                EntityManager.DestroyEntity(pieceQuery);
            }
        }
    }
}