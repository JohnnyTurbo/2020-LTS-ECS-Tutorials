using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_Singletons
{
    public class SpawnCapsuleAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        public float SpawnInterval;
        public GameObject CapsulePrefab;
        public Vector3 MinSpawnPosition;
        public Vector3 MaxSpawnPosition;
        public KeyCode FunKey;
        
        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(CapsulePrefab);
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var spawnCapsuleData = new SpawnCapsuleData
            {
                SpawnInterval = SpawnInterval,
                SpawnTimer = SpawnInterval,
                EntityPrefab = conversionSystem.GetPrimaryEntity(CapsulePrefab),
                FunKey = FunKey,
                Random = Random.CreateFromIndex(0),
                MinSpawnPosition = MinSpawnPosition,
                MaxSpawnPosition = MaxSpawnPosition
            };
            dstManager.AddComponentData(entity, spawnCapsuleData);
        }
    }
}