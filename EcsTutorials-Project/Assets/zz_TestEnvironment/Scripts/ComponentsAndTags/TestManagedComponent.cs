using Unity.Entities;

namespace TMG.zz_Test_Env
{
    [GenerateAuthoringComponent]
    public class TestManagedComponent : IComponentData
    {
        public int Value;
        public TestHelper Doubler;
    }
}