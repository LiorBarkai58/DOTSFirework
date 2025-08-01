using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct FireworkSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FireworkSpawner>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach (var (spawner, entity) in SystemAPI.Query<RefRO<FireworkSpawner>>().WithEntityAccess())
            {

                float3 spawnPosition = float3.zero;

                for (int i = 0; i < spawner.ValueRO.Count; i++)
                {
                    Entity instance = ecb.Instantiate(spawner.ValueRO.Prefab);
                    float3 velocity = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(spawner.ValueRO.MinSpeed, spawner.ValueRO.MaxSpeed);
                    velocity.y = 5;
                    ecb.SetComponent(instance, new LocalTransform
                    {
                        Position = spawnPosition,
                        Rotation = quaternion.identity,
                        Scale = 1f
                    });

                    ecb.AddComponent(instance, new VelocityComponent { Value = velocity });
                }

                ecb.DestroyEntity(entity); // remove spawner after firing
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}