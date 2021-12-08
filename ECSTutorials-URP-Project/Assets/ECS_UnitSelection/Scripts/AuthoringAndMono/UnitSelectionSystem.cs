/*using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using Material = Unity.Physics.Material;
using RaycastHit = UnityEngine.RaycastHit;

namespace TMG.HideMe
{
    public class UnitSelectionSystem : MonoBehaviour
    {
        private float3 mouseStartPos;
        private float3 mouseEndPos;
        private Camera _mainCamera;
        private EntityManager _entityManager;
        private bool isDragging;
        private bool justStopped;
        //private TestRaycastSelectionSystem _selectionSystem;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //_selectionSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<TestRaycastSelectionSystem>();
            //_selectionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<TestRaycastSelectionSystem>();
        }

        private void OnGUI()
        {
            if (isDragging /*&& _selectionSystem.EntitySelected#1#)
            {
                var rect = GetScreenRect(mouseStartPos, Input.mousePosition);
                DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
                DrawScreenRectBorder(rect,1, Color.blue);

                
            }

            if (justStopped)
            {
                justStopped = false;
                var rect = GetScreenRect(mouseStartPos, Input.mousePosition);
                DrawScreenRays(Color.red);
            }
        }

        private void DrawScreenRays(Color color)
        {
            var topLeft = Vector3.Min(mouseStartPos, mouseEndPos);
            var bottomRight = Vector3.Max(mouseStartPos, mouseEndPos);
            // Create Rect
            var rect = Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
            
            var ray1 = _mainCamera.ScreenPointToRay(rect.min);
            var ray2 = _mainCamera.ScreenPointToRay(rect.max);
            var ray3 = _mainCamera.ScreenPointToRay(new Vector2(rect.xMin, rect.yMax));
            var ray4 = _mainCamera.ScreenPointToRay(new Vector2(rect.xMax, rect.yMin));

            var ray1d = ray1.direction * 1000f;
            var ray2d = ray2.direction * 1000f;
            var ray3d = ray3.direction * 1000f;
            var ray4d = ray4.direction * 1000f;
            
            /*Debug.DrawRay(ray1.origin, ray1d, color, 25.1f);
            Debug.DrawRay(ray2.origin, ray2d, color, 25.1f);
            Debug.DrawRay(ray3.origin, ray3d, color, 25.1f);
            Debug.DrawRay(ray4.origin, ray4d, color, 25.1f);#1#

            RaycastHit hit1;
            RaycastHit hit2;
            RaycastHit hit3;
            RaycastHit hit4;
            
            Physics.Raycast(ray1, out hit1);
            Physics.Raycast(ray2, out hit2);
            Physics.Raycast(ray3, out hit3);
            Physics.Raycast(ray4, out hit4);
            
            /*Debug.DrawLine(hit1.point, hit3.point, Color.green, 5.1f);
            Debug.DrawLine(hit3.point, hit2.point, Color.green, 5.1f);
            Debug.DrawLine(hit2.point, hit4.point, Color.green, 5.1f);
            Debug.DrawLine(hit4.point, hit1.point, Color.green, 5.1f);#1#

            var nearpoint1 = ray1.GetPoint(_mainCamera.nearClipPlane);
            var nearpoint2 = ray2.GetPoint(_mainCamera.nearClipPlane);
            var nearpoint3 = ray3.GetPoint(_mainCamera.nearClipPlane);
            var nearpoint4 = ray4.GetPoint(_mainCamera.nearClipPlane);

            /*var farDistance = hit1.distance;
            farDistance = math.max(farDistance, hit2.distance);
            farDistance = math.max(farDistance, hit3.distance);
            farDistance = math.max(farDistance, hit4.distance);#1#

            //farDistance += 5f;
            
            var farpoint1 = ray1.GetPoint(hit1.distance + 5f);
            var farpoint2 = ray2.GetPoint(hit2.distance + 5f);
            var farpoint3 = ray3.GetPoint(hit3.distance + 5f);
            var farpoint4 = ray4.GetPoint(hit4.distance + 5f);
            
            Debug.DrawLine(farpoint1, farpoint3, Color.green, 25.1f);
            Debug.DrawLine(farpoint3, farpoint2, Color.green, 25.1f);
            Debug.DrawLine(farpoint2, farpoint4, Color.green, 25.1f);
            Debug.DrawLine(farpoint4, farpoint1, Color.green, 25.1f);
            
            var selectionBounds =
                new NativeArray<float3>(
                    new float3[]
                    {
                        farpoint1, farpoint2, farpoint3, farpoint4, nearpoint1, nearpoint2, nearpoint3, nearpoint4
                    }, Allocator.Temp);
            
            var pMat = Material.Default;
            pMat.CollisionResponse = CollisionResponsePolicy.RaiseTriggerEvents;
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = 1,
                CollidesWith = 2
            };
            var selectionCollider = ConvexCollider.Create(selectionBounds, ConvexHullGenerationParameters.Default, collisionFilter, pMat);
            selectionBounds.Dispose();
            var newSelectionEntity = _entityManager.CreateEntity(typeof(PhysicsCollider), typeof(SelectionTag), typeof(Translation), typeof(LocalToWorld));
            _entityManager.SetComponentData(newSelectionEntity, new PhysicsCollider{Value = selectionCollider});
        }
    }
}*/