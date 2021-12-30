using Unity.Entities;

namespace TMG.SyncPoints
{
    [GenerateAuthoringComponent]
    public struct CubeSpawnData : IComponentData
    {
        public Entity CubePrefab;
        public int NumberToSpawn;
    }
}