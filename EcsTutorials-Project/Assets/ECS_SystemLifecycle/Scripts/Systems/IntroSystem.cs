using Unity.Entities;
using UnityEngine;

namespace TMG.SystemLifecycle
{
    [DisableAutoCreation]
    public class IntroSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log("OnCreate()");
        }

        protected override void OnStartRunning()
        {
            Debug.Log("OnStartRunning()");
        }

        protected override void OnUpdate()
        {
            Debug.Log("OnUpdate()");
            Entities.WithAll<CapsuleTag>().ForEach((Entity e) =>
            {
                
            }).Run();
        }

        protected override void OnStopRunning()
        {
            Debug.Log("OnStopRunning()");
        }

        protected override void OnDestroy()
        {
            Debug.Log("OnDestroy()");
        }
    }
}










