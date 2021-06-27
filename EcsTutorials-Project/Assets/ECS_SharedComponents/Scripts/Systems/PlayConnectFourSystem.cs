using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ConnectFour
{
    public enum ConnectFourColor {None = 0, Red = 1, Blue = 2}
    
    public class PlayConnectFourSystem : SystemBase
    {
        private PieceSpawnData _spawnData;
        private PieceMaterialData _materialData;
        private Entity _gameControllerEntity;

        protected override void OnStartRunning()
        {
            //RequireSingletonForUpdate<PieceSpawnData>();
            _gameControllerEntity = GetSingletonEntity<PieceSpawnData>();
            _spawnData = EntityManager.GetComponentData<PieceSpawnData>(_gameControllerEntity);
            _materialData = EntityManager.GetComponentData<PieceMaterialData>(_gameControllerEntity);
            
            _spawnData.IsRedTurn = true;
        }

        protected override void OnUpdate()
        {
            var spawnColumn = -1;

            #region Region_GetPlayerInput

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                spawnColumn = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                spawnColumn = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                spawnColumn = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                spawnColumn = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                spawnColumn = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                spawnColumn = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                spawnColumn = 6;
            }

            #endregion

            if (spawnColumn == -1) return;

            var newHorizontalPosition = new HorizontalPosition {Value = spawnColumn};

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
            
            if (highestPosition >= 5) return;

            var newPieceEntity = EntityManager.Instantiate(_spawnData.Prefab);

            EntityManager.AddSharedComponentData(newPieceEntity, newHorizontalPosition);

            var newVerticalPosition = new VerticalPosition {Value = highestPosition + 1};

            EntityManager.AddComponentData(newPieceEntity, newVerticalPosition);

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

            var gameOver = false;

            if (_spawnData.IsRedTurn)
            {
                SetPieceColor(newPieceEntity, _materialData.RedMaterial);
                gameOver = CheckForWin(newHorizontalPosition, ConnectFourColor.Red);
            }
            else
            {
                SetPieceColor(newPieceEntity, _materialData.BlueMaterial);
                gameOver = CheckForWin(newHorizontalPosition, ConnectFourColor.Blue);
            }

            if (gameOver)
            {
                var winningColor = _spawnData.IsRedTurn ? "Red" : "Blue";
                Debug.Log($"{winningColor} won!");
                //EntityManager.RemoveComponent<PieceSpawnData>(_gameControllerEntity);
            }
            else
            {
                _spawnData.IsRedTurn = !_spawnData.IsRedTurn;
            }
        }

        private void SetPieceColor(Entity newPieceEntity, Material pieceMaterial)
        {
            var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(newPieceEntity);
            renderMesh.material = pieceMaterial;
            EntityManager.SetSharedComponentData(newPieceEntity, renderMesh);
        }

        private bool CheckForWin(HorizontalPosition columnFilter, ConnectFourColor curTurn)
        {
            var pieceActive = new NativeArray<ConnectFourColor>(6, Allocator.Temp);
            Entities
                .WithSharedComponentFilter(columnFilter)
                .ForEach((Entity newPieceEntity, in VerticalPosition y, in RenderMesh renderMesh) =>
                {
                    if(renderMesh.material == _materialData.RedMaterial)
                    {
                        pieceActive[y.Value] = ConnectFourColor.Red;
                    }
                    else if(renderMesh.material == _materialData.BlueMaterial)
                    {
                        pieceActive[y.Value] = ConnectFourColor.Blue;
                    }
                    else
                    {
                        pieceActive[y.Value] = ConnectFourColor.None;
                    }
                }).WithoutBurst().Run();
            
            var lowestWinningPiece = -1;
            for (var i = 0; i < 3; i++)
            {
                if (pieceActive[i] == curTurn && 
                    pieceActive[i + 1] == curTurn && 
                    pieceActive[i + 2] == curTurn &&
                    pieceActive[i + 3] == curTurn)
                {
                    lowestWinningPiece = i;
                    break;
                }
            }
            if(lowestWinningPiece == -1 ) return false;
            
            Entities
                .WithSharedComponentFilter(columnFilter)
                .WithStructuralChanges()
                .ForEach((Entity winningPieceEntity, RenderMesh renderMesh, in VerticalPosition y) =>
                {
                    if (y.Value >= lowestWinningPiece)
                    {
                        renderMesh.material = _materialData.YellowMaterial;
                        EntityManager.SetSharedComponentData(winningPieceEntity, renderMesh);
                    }
                }).WithoutBurst().Run();
            return true;
        }
    }
}