using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Material = UnityEngine.Material;
using RaycastHit = Unity.Physics.RaycastHit;

namespace TMG.UnitSelection
{
    public class MultiUnitSelectionSystem : SystemBase
    {
        private Camera _mainCamera;
        private BuildPhysicsWorld _physicsWorldSystem;
        private CollisionWorld _collisionWorld;
        private SelectionUIData _selectionUIData;

        private float3 _mouseStartPos;
        private float3 _mouseEndPos;
        private bool _isDragging;

        public bool IsDragging => _isDragging;

        public float3 MouseStartPos => _mouseStartPos;

        protected override void OnStartRunning()
        {
            _mainCamera = Camera.main;
            _selectionUIData = GetSingleton<SelectionUIData>();
        }
        
        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseStartPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !_isDragging)
            {
                if (math.distance(_mouseStartPos, Input.mousePosition) > 50)
                {
                    _isDragging = true;
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                DeselectUnits();
                _mouseEndPos = Input.mousePosition;
                
                if (_isDragging)
                {
                    SelectMultipleUnits();
                }

                else
                {
                    SelectSingleUnit();
                }
            }
        }

        private void SelectMultipleUnits()
        {
            var castDistance = 100f;

            var topLeft = math.min(_mouseStartPos, _mouseEndPos);
            var botRight = math.max(_mouseStartPos, _mouseEndPos);

            var rect = Rect.MinMaxRect(topLeft.x, topLeft.y, botRight.x, botRight.y);

            var cornerRays = new UnityEngine.Ray[]
            {
                _mainCamera.ScreenPointToRay(rect.min),
                _mainCamera.ScreenPointToRay(rect.max),
                _mainCamera.ScreenPointToRay(new Vector2(rect.xMin, rect.yMax)),
                _mainCamera.ScreenPointToRay(new Vector2(rect.xMax, rect.yMin))
            };

            var verticies = new NativeArray<float3>(8, Allocator.Temp);

            for (int i = 0, j = 0; i < 8; i+=2, j++)
            {
                verticies[i] = cornerRays[j].origin;
                verticies[i + 1] = cornerRays[j].GetPoint(castDistance);
            }

            var physicsMaterial = Unity.Physics.Material.Default;
            physicsMaterial.CollisionResponse = CollisionResponsePolicy.RaiseTriggerEvents;
            var collisionFilter = new CollisionFilter
            {
                BelongsTo = (uint) CollisionLayers.Selection,
                CollidesWith = (uint) CollisionLayers.Units
            };
            var selectionCollider = ConvexCollider.Create(verticies, ConvexHullGenerationParameters.Default,
                collisionFilter, physicsMaterial);
            var newSelectionEntity =
                EntityManager.CreateEntity(typeof(PhysicsCollider), typeof(Translation), typeof(LocalToWorld));
            EntityManager.SetComponentData(newSelectionEntity, new PhysicsCollider{Value = selectionCollider});
        }

        private void SelectSingleUnit()
        {
            _physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
            _collisionWorld = _physicsWorldSystem.PhysicsWorld.CollisionWorld;

            var ray = _mainCamera.ScreenPointToRay(_mouseEndPos);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(_mainCamera.farClipPlane);

            if (Raycast(rayStart, rayEnd, out var hit))
            {
                var selectedEntity = _physicsWorldSystem.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                if (EntityManager.HasComponent<SelectableUnitTag>(selectedEntity))
                {
                    SelectUnit(selectedEntity);
                }
            }
        }

        private bool Raycast(float3 rayFrom, float3 rayTo, out RaycastHit raycastHit)
        {
            var input = new RaycastInput()
            {
                Start = rayFrom,
                End = rayTo,
                Filter = new CollisionFilter
                {
                    BelongsTo = (uint) CollisionLayers.Selection,
                    CollidesWith = (uint) (CollisionLayers.Ground | CollisionLayers.Units)
                }
            };
            return _collisionWorld.CastRay(input, out raycastHit);
        }

        private void SelectUnit(Entity selectedEntity)
        {
            EntityManager.AddComponent<SelectedEntityTag>(selectedEntity);
            var selectionUI = EntityManager.Instantiate(_selectionUIData.SelectionUIPrefab);
            EntityManager.AddComponentData(selectionUI, new Parent {Value = selectedEntity});
            EntityManager.AddComponentData(selectionUI, new LocalToParent {Value = float4x4.zero});
        }

        private void DeselectUnits()
        {
            EntityManager.RemoveComponent<SelectedEntityTag>(GetEntityQuery(typeof(SelectedEntityTag)));
            EntityManager.DestroyEntity(GetEntityQuery(typeof(SelectionUITag)));
        }
    }
}