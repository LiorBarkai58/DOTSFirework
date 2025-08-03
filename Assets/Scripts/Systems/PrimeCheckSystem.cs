// PrimeCheckSystem_Sequential.cs - CORRECT VERSION
using Unity.Entities;
using UnityEngine;
using System.Diagnostics;
[UpdateInGroup(typeof(SimulationSystemGroup))]
[DisableAutoCreation]
public partial class PrimeCheckSystem_Sequential : SystemBase
{
    // 1. Get a reference to the system that manages command buffers
    private EndSimulationEntityCommandBufferSystem m_EndSimECBSystem;

    protected override void OnCreate()
    {
        // Cache the system on creation
        m_EndSimECBSystem = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        // 2. Create a new command buffer to record our commands
        var ecb = m_EndSimECBSystem.CreateCommandBuffer();

        foreach (var (request, entity) in SystemAPI.Query<RefRO<PrimeCheckRequest>>()
                                                    .WithNone<PrimeCheckResult>().WithEntityAccess())
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ulong n = request.ValueRO.NumberToCheck;
            byte result = IsPrime(n);

            stopwatch.Stop();

            // 3. Record the command on the buffer instead of executing it immediately
            ecb.AddComponent(entity, new PrimeCheckResult { IsPrime = result });

            UnityEngine.Debug.Log($"[SEQUENTIAL] {n} is prime? {result == 1}. Time: {stopwatch.Elapsed.TotalMilliseconds:F4} ms");
        }
    }

    private byte IsPrime(ulong n)
    {
        if (n <= 1) return 0;
        if (n <= 3) return 1;
        if (n % 2 == 0 || n % 3 == 0) return 0;

        ulong limit = (ulong)System.Math.Sqrt(n);
        for (ulong i = 5; i <= limit; i = i + 6)
        {
            if (n % i == 0 || n % (i + 2) == 0)
            {
                return 0;
            }
        }
        return 1;
    }
}