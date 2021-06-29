using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ChangeFilter
{
    public class TestMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Entities.WithAll<TestTag1>().ForEach((Entity e, ref Translation translation) =>
                {
                    Debug.Log("Moving Translation");
                }).Run();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Entities.WithAll<TestTag1>().ForEach((Entity e, ref Rotation rotation) =>
                {
                    Debug.Log("Moving Rotation");
                }).Run();
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Entities.WithAll<TestTag1>().ForEach((Entity e, ref Translation translation, ref Rotation rotation) =>
                {
                    Debug.Log("Moving Translation and rotation");
                }).Run();
            }
        }
    }
}