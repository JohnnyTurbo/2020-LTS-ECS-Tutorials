using Unity.Entities;
using Unity.Transforms;

namespace TMG.ECS_CommandBuffer
{
    public class SpawnEntitiesSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            var deltaTime = Time.DeltaTime;
            Entities.WithAll<ShouldSpawnTag>().ForEach((Entity e, int entityInQueryIndex, ref EntitySpawnData spawnData, 
                in Translation translation) =>
            {
                spawnData.Timer -= deltaTime;
                if (spawnData.Timer <= 0)
                {
                    spawnData.Timer = spawnData.SpawnDelay;
                    var newEntity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
                    ecb.AddComponent<CapsuleTag>(entityInQueryIndex, newEntity);
                    ecb.SetComponent(entityInQueryIndex, newEntity, translation);
                }
            }).ScheduleParallel();
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}