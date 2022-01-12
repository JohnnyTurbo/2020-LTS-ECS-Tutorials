using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.SyncPoints
{
    public class SpawnGridSystem : SystemBase
    {
        private CubeSpawnData _cubeSpawnData;
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;
        
        protected override void OnStartRunning()
        {
            _cubeSpawnData = GetSingleton<CubeSpawnData>();
            
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            var ecb = _endSimulationECBSystem.CreateCommandBuffer();
            
            for (var x = 0; x < _cubeSpawnData.SpawnGridSize.x; x++)
            {
                for (var y = 0; y < _cubeSpawnData.SpawnGridSize.y; y++)
                {
                    var newCube = ecb.Instantiate(_cubeSpawnData.CubePrefab);
                    var newTranslation = new Translation {Value = new float3(x, 0f, y)};
                    ecb.SetComponent(newCube, newTranslation);
                    if (x == 0 && y == 0)
                    {
                        ecb.AddComponent<FirstCubeTag>(newCube);
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
            
        }
    }
    
    public class MoveCubeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var curTime = Time.ElapsedTime;
            Entities.ForEach((ref Translation translation, in CubeMoveData cubeMoveData) =>
            {
                var frequency = cubeMoveData.MoveSpeed;
                var thisTime = curTime + translation.Value.x * 0.1f;
                translation.Value.y = (float) math.sin(thisTime * frequency) + 2f;
            }).Schedule();
        }
    }
    
    [UpdateAfter(typeof(MoveCubeSystem))]
    public class MakeSyncPointSystem : SystemBase
    {
        private Entity _firstCubeEntity;
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;
        
        protected override void OnStartRunning()
        {
            _firstCubeEntity = GetSingletonEntity<FirstCubeTag>();
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationECBSystem.CreateCommandBuffer();
            if (EntityManager.HasComponent<SyncPointTag>(_firstCubeEntity))
            {
                ecb.RemoveComponent<SyncPointTag>(_firstCubeEntity);
            }
            else
            {
                ecb.AddComponent<SyncPointTag>(_firstCubeEntity);
            }
        }
    }
    
    [UpdateAfter(typeof(MakeSyncPointSystem))]
    [UpdateAfter(typeof(MoveCubeSystem))]
    public class RotateCubeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((ref Rotation rotation, in CubeMoveData cubeMoveData) =>
            {
                var rotationAngle = cubeMoveData.RotationSpeed * deltaTime;
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(rotationAngle));
            }).Schedule();
        }
    }
}