using Unity.Entities;

namespace TMG.ECS_Entities
{
    public class CreateEntitiesSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            var emptyEntity1 = EntityManager.CreateEntity(typeof(IntData));
            var emptyEntity2 = EntityManager.CreateEntity(typeof(IntData), typeof(IntData2));
            var emptyEntity3 = EntityManager.CreateEntity(typeof(IntData3));
        }

        protected override void OnUpdate()
        {
            
        }
    }
}