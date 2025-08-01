using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct VelocitySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            float duration = 2f;
            float dampingFactor = deltaTime / duration;
            float3 gravity = new float3(0, -4.81f, 0);

            foreach (var (transform, velocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<VelocityComponent>>())
            {
                velocity.ValueRW.Value.y += gravity.y * deltaTime;

                velocity.ValueRW.Value.x = math.lerp(velocity.ValueRO.Value.x, 0f, dampingFactor);
                velocity.ValueRW.Value.z = math.lerp(velocity.ValueRO.Value.z, 0f, dampingFactor);

                transform.ValueRW.Position += velocity.ValueRW.Value * deltaTime;
                
                float newScale = math.lerp(transform.ValueRO.Scale, 0f, dampingFactor);
                transform.ValueRW.Scale = newScale;
            }
        }
    }
}