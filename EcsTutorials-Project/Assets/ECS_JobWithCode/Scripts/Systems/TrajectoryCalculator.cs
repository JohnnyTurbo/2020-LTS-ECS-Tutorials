using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.JobWithCode
{
    public class TrajectoryCalculator : SystemBase
    {
        private int _numberPoints = 250;
        private float _initialVelocity = 10f;
        private float _gravityFactor = 9.8f;
        
        protected override void OnUpdate()
        {
            var curTime = (float)Time.ElapsedTime;
            var initialVelocity = _initialVelocity;
            var gravityFactor = _gravityFactor;
            var numberPoints = _numberPoints;
            
            var points = new NativeArray<float3>(_numberPoints, Allocator.TempJob);
            
            Job.WithCode(() =>
            {
                var angle = math.radians(30f * math.sin((curTime - 2.5f * math.PI) / 5f) + 45f);
                
                for (var i = 0; i < numberPoints; i++)
                {
                    var time = i * 0.01f;
                    points[i] = new float3
                    {
                        x = initialVelocity * time * math.cos(angle),
                        y = initialVelocity * time * math.sin(angle) - 0.5f * gravityFactor * math.pow(time, 2),
                        z = 0f
                    };
                }
            }).Schedule();

            Dependency.Complete();
            
            for (var i = 0; i < numberPoints - 1; i++)
            {
                if (points[i].y < 0f)
                {
                    break;
                }
                Debug.DrawLine(points[i], points[i+1]);
            }
        }
    }
}