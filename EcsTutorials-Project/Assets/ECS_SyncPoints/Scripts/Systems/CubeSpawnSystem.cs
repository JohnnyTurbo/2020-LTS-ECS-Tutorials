using Unity.Entities;

namespace TMG.SyncPoints
{
    public class CubeSpawnSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            var cubeSpawnData = GetSingleton<CubeSpawnData>();

            for (var i = 0; i < cubeSpawnData.NumberToSpawn; i++)
            {
                EntityManager.Instantiate(cubeSpawnData.CubePrefab);
            }
        }

        protected override void OnUpdate()
        {
            
        }
    }
}