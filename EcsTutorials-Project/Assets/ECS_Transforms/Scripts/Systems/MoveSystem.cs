using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ECS_Transforms
{
    public class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((in TestMoveData testMoveData) =>
            {
                /*if (Input.GetKey(testMoveData.MoveKey))
                {
                    var e = testMoveData.EntityToMove;
                    var t = GetComponent<Translation>(e);
                    t.Value.x += testMoveData.MoveSpeed * deltaTime;
                    SetComponent(e, t);
                }

                if (Input.GetKey(testMoveData.RotationKey))
                {
                    var e = testMoveData.EntityToMove;
                    var r = GetComponent<Rotation>(e);
                    r.Value = math.mul(r.Value,
                        quaternion.RotateX(math.radians(testMoveData.RoationSpeed * deltaTime))); 
                    SetComponent(e, r);
                }*/
                
                var e = testMoveData.EntityToMove;
                //var t = GetComponent<Translation>(e);
                //var r = GetComponent<Rotation>(e);
                //var s = GetComponent<Scale>(e);
                var trs = GetComponent<LocalToWorld>(e);
                
                //TRSToLocalToWorldSystem
                //float4 hello;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    trs.Value = new float4x4(quaternion.Euler(45, 0, 0), new float3(3, 4, 3));
                    SetComponent(e, trs);
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    trs.Value = float4x4.Translate(new float3(3, 4, 5));
                    SetComponent(e, trs);
                }

            }).Run();
        }
    }
}