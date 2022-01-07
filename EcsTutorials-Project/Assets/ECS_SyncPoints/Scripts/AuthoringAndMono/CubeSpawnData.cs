using Unity.Entities;
using Unity.Mathematics;

namespace ECS_SyncPoints.Scripts.AuthoringAndMono
{
    [GenerateAuthoringComponent]
    public struct CubeSpawnData : IComponentData
    {
        public Entity CubePrefab;
        public int2 SpawnGridSize;
    }
}