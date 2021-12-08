using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace TMG.UnitSelection
{
    [DisableAutoCreation]
    public class SingleUnitSelectionSystem : SystemBase
    {
        private Camera _mainCamera;
        private BuildPhysicsWorld _physicsWorldSystem;
        private CollisionWorld _collisionWorld;
        private SelectionUIData _selectionUIData;

        protected override void OnStartRunning()
        {
            _mainCamera = Camera.main;
            _selectionUIData = GetSingleton<SelectionUIData>();
        }
        
        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                DeselectUnits();

                _physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
                _collisionWorld = _physicsWorldSystem.PhysicsWorld.CollisionWorld;

                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
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