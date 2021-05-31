using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ECS_EntityManager
{
    public class SpawnEntitiesSystem : SystemBase
    {
        private int2 _entitySpacing;
        
        protected override void OnStartRunning()
        {
            var entitySpawnData = GetSingleton<EntitySpawnData>();
            
            var gridSize = entitySpawnData.SpawnGrid;
            _entitySpacing = entitySpawnData.EntitySpacing;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    var newEntity = EntityManager.Instantiate(entitySpawnData.EntityPrefab);
                    var newPosition = new LocalToWorld {Value = CalculateTransform(x, y)};
                    EntityManager.SetComponentData(newEntity, newPosition);

                    if ((x + y) % 2 == 0)
                    {
                        EntityManager.AddComponent<OscillatingTag>(newEntity);
                    }
                }
            }
        }

        private float4x4 CalculateTransform(int x, int y)
        {
            return float4x4.Translate(new float3
            {
                x = x * _entitySpacing.x,
                y = 1f,
                z = y * _entitySpacing.y
            });
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                var oscillators = EntityManager.CreateEntityQuery(typeof(OscillatingTag));
                EntityManager.DestroyEntity(oscillators);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                EntityManager.DestroyAndResetAllEntities();
            }
        }
    }
}