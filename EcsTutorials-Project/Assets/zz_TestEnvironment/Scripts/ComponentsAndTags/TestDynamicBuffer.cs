using System;
using Unity.Entities;

namespace TMG.zz_Test_Env
{
    [GenerateAuthoringComponent]
    public struct TestDynamicBuffer : IBufferElementData
    {
        public Workaround valueSet;
    }

    [Serializable]
    public struct Workaround
    {
        public int valueInt;
        public bool valueBool;
    }
}