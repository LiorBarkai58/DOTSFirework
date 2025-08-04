using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct BuildingSpawnerSystem : ISystem
    {
        private Unity.Mathematics.Random _random;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _random = Unity.Mathematics.Random.CreateFromIndex(12345);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (spawner, entity) in SystemAPI.Query<RefRW<BuildingSpawningComponent>>().WithEntityAccess())
            {
                
                spawner.ValueRW.TimeSinceLastSpawn += deltaTime;
                var currentTransform = SystemAPI.GetComponentRO<LocalTransform>(entity).ValueRO;
                float3 spawnerPosition = currentTransform.Position;

                if (spawner.ValueRW.TimeSinceLastSpawn >= spawner.ValueRW.SpawnInterval)
                {
                    spawner.ValueRW.TimeSinceLastSpawn = 0f;

                    int choice = _random.NextInt(0, 5);
                    Entity prefab = choice switch
                    {
                        0 => spawner.ValueRO.EasyPrefab,
                        1 => spawner.ValueRO.EasyPrefab,
                        2 => spawner.ValueRO.MediumPrefab,
                        3 => spawner.ValueRO.MediumPrefab,
                        4 => spawner.ValueRO.HardPrefab,
                        _ => Entity.Null
                    };
                    if (prefab != Entity.Null)
                    {
                        Entity spawned = state.EntityManager.Instantiate(prefab);
                        state.EntityManager.SetComponentData(spawned, new LocalTransform
                        {
                            Position = spawnerPosition,
                            Rotation = quaternion.Euler(0, math.radians(90), 0),
                            Scale = 1f
                        });
                    }
                }
            }
        }
    }
}
