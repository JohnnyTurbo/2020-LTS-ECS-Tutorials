using Unity.Entities;
using UnityEngine;

namespace TMG.ConnectFour
{
    [GenerateAuthoringComponent]
    public class PieceMaterialData : IComponentData
    {
        public Material RedMaterial;
        public Material BlueMaterial;
    }
}