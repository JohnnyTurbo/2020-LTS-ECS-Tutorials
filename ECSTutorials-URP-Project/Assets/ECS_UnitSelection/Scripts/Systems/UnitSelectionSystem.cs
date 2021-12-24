using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace TMG.UnitSelection
{
    [AlwaysUpdateSystem]
    public class UnitSelectionSystem : SystemBase
    {
        private Camera _mainCamera;
        private BuildPhysicsWorld _buildPhysicsWorld;
        private CollisionWorld _collisionWorld;

        private EntityArchetype _selectionArchetype;
        private float3 _mouseStartPos;
        private bool _isDragging;

        public float3 MouseStartPos => _mouseStartPos;
        public bool IsDragging => _isDragging;

        protected override void OnCreate()
        {
            _mainCamera = Camera.main;
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();

            _selectionArchetype = EntityManager.CreateArchetype(typeof(PhysicsCollider), typeof(LocalToWorld),
                typeof(SelectionColliderTag));
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseStartPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !_isDragging)
            {
                if (math.distance(_mouseStartPos, Input.mousePosition) > 25)
                {
                    _isDragging = true;
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectUnits();
                }

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
            _isDragging = false;

            var topLeft = math.min(_mouseStartPos, Input.mousePosition);
            var botRight = math.max(_mouseStartPos, Input.mousePosition);

            var rect = Rect.MinMaxRect(topLeft.x, topLeft.y, botRight.x, botRight.y);

            var cornerRays = new[]
            {
                _mainCamera.ScreenPointToRay(rect.min),
                _mainCamera.ScreenPointToRay(rect.max),
                _mainCamera.ScreenPointToRay(new Vector2(rect.xMin, rect.yMax)),
                _mainCamera.ScreenPointToRay(new Vector2(rect.xMax, rect.yMin))
            };

            var vertices = new NativeArray<float3>(5, Allocator.Temp);

            for (var i = 0; i < cornerRays.Length; i++)
            {
                vertices[i] = cornerRays[i].GetPoint(50f);
            }

            vertices[4] = _mainCamera.transform.position;
            
            var collisionFilter = new CollisionFilter
            {
                BelongsTo = (uint) CollisionLayers.Selection,
                CollidesWith = (uint) CollisionLayers.Units
            };
            
            var physicsMaterial = Unity.Physics.Material.Default;
            physicsMaterial.CollisionResponse = CollisionResponsePolicy.RaiseTriggerEvents;

            var selectionCollider = ConvexCollider.Create(vertices, ConvexHullGenerationParameters.Default,
                collisionFilter, physicsMaterial);

            var newSelectionEntity = EntityManager.CreateEntity(_selectionArchetype);
            EntityManager.SetComponentData(newSelectionEntity, new PhysicsCollider{Value = selectionCollider});
        }

        private void SelectSingleUnit()
        {
            _collisionWorld = _buildPhysicsWorld.PhysicsWorld.CollisionWorld;

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(100f);

            if (Raycast(rayStart, rayEnd, out var raycastHit))
            {
                var hitEntity = _buildPhysicsWorld.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
                if (EntityManager.HasComponent<SelectableUnitTag>(hitEntity))
                {
                    EntityManager.AddComponent<SelectedEntityTag>(hitEntity);
                }
            }
        }

        private bool Raycast(float3 rayStart, float3 rayEnd, out RaycastHit raycastHit)
        {
            var raycastInput = new RaycastInput
            {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter
                {
                    BelongsTo = (uint) CollisionLayers.Selection,
                    CollidesWith = (uint) (CollisionLayers.Ground | CollisionLayers.Units)
                }
            };
            return _collisionWorld.CastRay(raycastInput, out raycastHit);
        }

        private void DeselectUnits()
        {
            EntityManager.RemoveComponent<SelectedEntityTag>(GetEntityQuery(typeof(SelectedEntityTag)));
        }
    }
}





















