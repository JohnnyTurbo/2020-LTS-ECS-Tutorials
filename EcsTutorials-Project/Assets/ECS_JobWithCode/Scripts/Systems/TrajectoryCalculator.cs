using ECS_JobWithCode.Scripts.ComponentsAndTags;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS_JobWithCode.Scripts.Systems
{
    public class TrajectoryCalculator : SystemBase
    {
        private NativeArray<Entity> _spheres;
        private int _numberSpheres = 250;

        private float _initalVelocity = 10f;
        private float _gravityFactor = 9.8f;
        
        /*protected override void OnStartRunning()
        {
            var sphereSpawnData = GetSingleton<SpherePrefabData>();
            _numberSpheres = sphereSpawnData.SpawnCount;

            _spheres = new NativeArray<Entity>(_numberSpheres, Allocator.Persistent);
            
            for (var i = 0; i < _numberSpheres; i++)
            {
                _spheres[i] = EntityManager.Instantiate(sphereSpawnData.Prefab);
            }
        }*/

        protected override void OnUpdate()
        {
            var curTime = (float)Time.ElapsedTime;
            var initialVelocity = _initalVelocity;
            var gravityFactor = _gravityFactor;
            var numberSpheres = _numberSpheres;
            
            Job.WithCode(() =>
            {
                var angle = math.radians(30f * math.sin((curTime - 2.5f * math.PI) / 5f) + 45f);
                //Debug.LogWarning($"Kurt {angle}");
                
                var previousPoint = float3.zero;
                
                for (var i = 1; i < numberSpheres; i++)
                {
                    var time = i * 0.01f;
                    var curPoint = new float3
                    {
                        x = initialVelocity * time * math.cos(angle),
                        y = initialVelocity * time * math.sin(angle) - 0.5f * gravityFactor * math.pow(time, 2),
                        z = 0f
                    }; 
                    //Debug.Log($"y{i}: {curPoint.y}");
                    Debug.DrawLine(previousPoint, curPoint);
                    if(curPoint.y <= 0f){break;}
                    previousPoint = curPoint;
                }
                
            }).Schedule();
        }
    }
}