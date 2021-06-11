using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_CommandBuffer
{
    public class DestroyEntitiesSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
                var capsuleQuery = EntityManager.CreateEntityQuery(typeof(CapsuleTag));
                ecb.DestroyEntity(capsuleQuery);
            }
        }
    }
}