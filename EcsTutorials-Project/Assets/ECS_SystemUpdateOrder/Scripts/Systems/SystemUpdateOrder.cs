using Unity.Entities;
using UnityEngine;

namespace TMG.SystemUpdateOrder
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class TestGroupA : ComponentSystemGroup
    {
    }
    
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(TestGroupA), OrderLast = true)]
    public class TestSystemC : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }
    
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(TestGroupA), OrderFirst = true, OrderLast = false)]
    [UpdateBefore(typeof(TestSystemA))]
    public class TestSystemB : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }

    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(TestGroupA), OrderFirst = true)]
    public class TestSystemA : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }
}
