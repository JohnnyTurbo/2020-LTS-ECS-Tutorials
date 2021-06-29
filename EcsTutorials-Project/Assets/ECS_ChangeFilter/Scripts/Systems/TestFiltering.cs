using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ChangeFilter
{
    public class TestFiltering : SystemBase
    {
        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<TestTag1>();
        }

        protected override void OnUpdate()
        {
            Entities
                .WithChangeFilter<Translation>()
                .ForEach((Entity e) =>
            {
                Debug.Log($"Entity {e.Index} moved!");
            }).Run();
            
            Entities
                .WithChangeFilter<Rotation>()
                .ForEach((Entity e) =>
                {
                    Debug.Log($"Entity {e.Index} Rotated!");
                }).Run();
            
            Entities
                .WithChangeFilter<Translation, Rotation>()
                .ForEach((Entity e) =>
                {
                    Debug.Log($"Entity {e.Index} moved and rotated!");
                }).Run();
            
            Entities.WithChangeFilter<PhysicsVelocity>().ForEach((Entity echo) =>
            {
                Debug.Log($"Entity echo {echo.Index} has velocity changing");
            }).Run();
        }
    }
}