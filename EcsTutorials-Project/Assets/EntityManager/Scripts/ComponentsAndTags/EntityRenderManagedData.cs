using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_EntityManager
{
    [GenerateAuthoringComponent]
    public class EntityRenderManagedData : IComponentData
    {
        public Material Material;
        public Mesh Mesh;
    }
}