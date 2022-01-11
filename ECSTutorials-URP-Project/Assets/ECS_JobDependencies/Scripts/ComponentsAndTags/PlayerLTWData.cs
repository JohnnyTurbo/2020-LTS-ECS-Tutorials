using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.JobDependencies
{
    [GenerateAuthoringComponent]
    public class PlayerLTWData : IComponentData
    {
        public NativeArray<LocalToWorld> Value;
    }
}