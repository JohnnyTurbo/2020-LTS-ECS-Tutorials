using TMG.zz_Test_Env;
using Unity.Entities;
using UnityEngine;

namespace zz_TestEnvironment.Scripts.Systems
{
    public class TestDoublingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Entities.ForEach((TestManagedComponent data) =>
                {
                    Debug.Log($"value before: {data.Value}");
                    data.Value = data.Doubler.Doubler(data.Value);
                    Debug.Log($"Data After: {data.Value}");
                }).WithoutBurst().Run();
            }
        }
    }
}