using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct VelocityComponent : IComponentData
    {
        public float3 Value;
    }
}