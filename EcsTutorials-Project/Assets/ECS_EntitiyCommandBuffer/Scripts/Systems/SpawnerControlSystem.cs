using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_CommandBuffer
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SpawnerControlSystem : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem _endInitializationEntityCommandBufferSystem;
        protected override void OnCreate()
        {
            _endInitializationEntityCommandBufferSystem =
                World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endInitializationEntityCommandBufferSystem.CreateCommandBuffer();
            var spawnerQuery = EntityManager.CreateEntityQuery(typeof(EntitySpawnData));

            if (Input.GetKeyDown(KeyCode.Y))
            {
                ecb.AddComponent<ShouldSpawnTag>(spawnerQuery);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                ecb.RemoveComponent<ShouldSpawnTag>(spawnerQuery);
            }
        }
    }
}