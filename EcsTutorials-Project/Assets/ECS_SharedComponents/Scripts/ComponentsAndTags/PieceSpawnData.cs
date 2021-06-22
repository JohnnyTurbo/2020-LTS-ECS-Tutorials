using Unity.Entities;

namespace TMG.ConnectFour
{
    [GenerateAuthoringComponent]
    public struct PieceSpawnData : IComponentData
    {
        public Entity Prefab;
        public bool IsRedTurn;
    }
}