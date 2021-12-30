using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.SystemStateComponents
{
    public class MoveCapsuleSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity e, ref Translation translation, ref CapsuleMoveData moveData) =>
            {
                moveData.TimeAlive += deltaTime;
                if (moveData.TimeAlive > moveData.TimeToLive)
                {
                    ecb.DestroyEntity(e);
                }
                else
                {
                    translation.Value.x += moveData.Speed * deltaTime;
                    translation.Value.y = math.cos(moveData.TimeAlive * 4f + math.PI) + 2f;
                }
            }).Run();
        }
    }
}