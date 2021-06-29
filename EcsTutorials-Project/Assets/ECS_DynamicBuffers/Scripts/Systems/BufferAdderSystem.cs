using Unity.Entities;
using UnityEngine;

namespace TMG.DynamicBuffers
{
    public class BufferAdderSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            
        }

        protected override void OnUpdate()
        {
            var entity1 = EntityManager.CreateEntity();

            var buffer1 = EntityManager.AddBuffer<IntBufferElement>(entity1);
            var buffer2 = EntityManager.AddBuffer<IntBufferElement>(entity1);
            buffer1.Add(1);
            Debug.Log(buffer1[0].Value);
            
            var sum = 0;
            Entities.ForEach((DynamicBuffer<IntBufferElement> buffer) =>
            {
                var midSom = 0;
                buffer.Add(10);
                for (var i = 0; i < buffer.Length; i++)
                {
                    sum += buffer[i];
                    midSom += buffer[i];
                }
                Debug.Log($"Mid Som: {midSom}");
            }).Run();
            Debug.Log($"Sum: {sum}");
        }
    }
}