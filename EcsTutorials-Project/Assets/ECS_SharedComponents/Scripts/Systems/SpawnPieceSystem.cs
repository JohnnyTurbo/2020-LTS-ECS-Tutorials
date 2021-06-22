using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ConnectFour
{
    public class SpawnPieceSystem : SystemBase
    {
        private PieceSpawnData _spawnData;
        private PieceMaterialData _materialData;
        
        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<PieceSpawnData>();
            _spawnData = GetSingleton<PieceSpawnData>();
            var spawnEntity = GetSingletonEntity<PieceSpawnData>();
            _materialData = EntityManager.GetComponentData<PieceMaterialData>(spawnEntity);
            _spawnData.IsRedTurn = true;
        }

        protected override void OnUpdate()
        {
            var spawnCol = -1;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                spawnCol = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                spawnCol = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                spawnCol = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                spawnCol = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                spawnCol = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                spawnCol = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                spawnCol = 6;
            }

            if (spawnCol == -1) return;

            var newHorizontalPosition = new HorizontalPosition {Value = spawnCol};
            
            var highestPosition = -1;

            Entities
                .WithSharedComponentFilter(newHorizontalPosition)
                .ForEach((in VerticalPosition verticalPosition) =>
                {
                    if (verticalPosition.Value > highestPosition)
                    {
                        highestPosition = verticalPosition.Value;
                    }
                }).Run();

            Debug.Log($"High: {highestPosition}");

            if(highestPosition >= 5) return;
            
            var newPieceEntity = EntityManager.Instantiate(_spawnData.Prefab);

            EntityManager.AddSharedComponentData(newPieceEntity, newHorizontalPosition);

            var newVerticalPosition = new VerticalPosition {Value = highestPosition + 1};
            
            EntityManager.SetComponentData(newPieceEntity, newVerticalPosition);

            var newPosition = new Translation
            {
                Value = new float3
                {
                    x = newHorizontalPosition.Value,
                    y = newVerticalPosition.Value,
                    z = 0f
                }
            };
            
            EntityManager.SetComponentData(newPieceEntity, newPosition);

            var newMaterial = _spawnData.IsRedTurn ? _materialData.RedMaterial : _materialData.BlueMaterial;

            var curMaterial = EntityManager.GetSharedComponentData<RenderMesh>(newPieceEntity);
            curMaterial.material = newMaterial;
            EntityManager.SetSharedComponentData(newPieceEntity, curMaterial);
            _spawnData.IsRedTurn = !_spawnData.IsRedTurn;
        }
    }
}