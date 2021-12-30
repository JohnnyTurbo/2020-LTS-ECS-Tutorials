using Unity.Entities;

namespace TMG.SystemStateComponents
{
    [GenerateAuthoringComponent]
    public struct CapsuleMoveData : IComponentData
    {
        public float Speed;
        public float TimeAlive;
        public float TimeToLive;
    }
}