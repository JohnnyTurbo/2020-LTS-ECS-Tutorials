using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_Worlds
{
    //[DisableAutoCreation]
    public class TestSystem2 : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<BravoTag>().ForEach((Entity e) =>
            {
                Debug.Log("Bravo");
            }).Run();
        }
    }
}