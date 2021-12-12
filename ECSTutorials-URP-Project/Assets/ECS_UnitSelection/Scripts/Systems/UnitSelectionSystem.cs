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

        protected override void OnCreate()
        {
            _mainCamera = Camera.main;
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectUnits();
                }
                
                SelectSingleUnit();
            }
        }

        private void DeselectUnits()
        {
            EntityManager.RemoveComponent<SelectedEntityTag>(GetEntityQuery(typeof(SelectedEntityTag)));
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
    }
}





















