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
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            if (Input.GetKeyDown(KeyCode.D))
            {
                var capsules = EntityManager.CreateEntityQuery(typeof(CapsuleTag));
                ecb.DestroyEntitiesForEntityQuery(capsules);
            }
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}