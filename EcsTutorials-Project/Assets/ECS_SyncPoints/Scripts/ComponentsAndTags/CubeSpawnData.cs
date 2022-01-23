using Unity.Entities;
using Unity.Mathematics;

namespace TMG.SyncPoints
{
    [GenerateAuthoringComponent]
    public struct CubeSpawnData : IComponentData
    {
        public Entity CubePrefab;
        public int2 SpawnGridSize;
    }
}