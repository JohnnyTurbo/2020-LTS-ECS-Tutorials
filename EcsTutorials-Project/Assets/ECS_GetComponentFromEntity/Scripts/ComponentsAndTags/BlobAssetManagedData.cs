using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_GetComponentFromEntity
{
    [GenerateAuthoringComponent]
    public class BlobAssetManagedData : IComponentData
    {
        public Entity[] Leaders;
    }
}