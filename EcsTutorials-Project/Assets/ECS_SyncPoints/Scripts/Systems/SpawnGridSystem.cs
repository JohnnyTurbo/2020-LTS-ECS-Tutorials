﻿using ECS_SyncPoints.Scripts.AuthoringAndMono;
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
                var magnitude = 1f;
                var frequency = 1f;
                var thisTime = curTime + translation.Value.x * translation.Value.y;
                translation.Value.y = magnitude * (float) math.sin(thisTime * frequency) + magnitude + 1f;
            }).ScheduleParallel();
        }
    }

    //[DisableAutoCreation]
    /*[UpdateAfter(typeof(MoveCubeSystem))]
    public class SpawnNewCubeSystem : SystemBase
    {
        private CubeSpawnData _cubeSpawnData;

        protected override void OnStartRunning()
        {
            _cubeSpawnData = GetSingleton<CubeSpawnData>();
        }

        protected override void OnUpdate()
        {
            EntityManager.Instantiate(_cubeSpawnData.CubePrefab);
        }
    }*/
    
    //[UpdateAfter(typeof(SpawnNewCubeSystem))]
    [UpdateAfter(typeof(MoveCubeSystem))]
    public class RotateCubeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((ref Rotation rotation, in CubeMoveData cubeMoveData) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(cubeMoveData.RotationSpeed * deltaTime));
            }).ScheduleParallel();
        }
    }
}