using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevel;

namespace TMG.ECS_Worlds
{
    public class NewWorldSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                var newWorld = new World("TurboWorld");
                var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
                ScriptBehaviourUpdateOrder.AddWorldToPlayerLoop(newWorld, ref playerLoop);
                
                var turboManager = newWorld.EntityManager;

                var newEnt = turboManager.CreateEntity();
                turboManager.AddComponent<BravoTag>(newEnt);
                
                //var newSys = newWorld.GetOrCreateSystem<TestSystem2>();
                newWorld.AddSystem(new TestSystem2());
                //newWorld.AddSystem(newSys);
            }
        }
    }
}