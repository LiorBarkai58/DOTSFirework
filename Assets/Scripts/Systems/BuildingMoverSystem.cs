using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct BuildingMoverSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

            foreach (var (transform, velocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BuildingMovementComponent>>())
            {
                float deltaTime = SystemAPI.Time.DeltaTime;
                transform.ValueRW.Position += velocity.ValueRW.Value * deltaTime;
                
            }
        }
    }
}