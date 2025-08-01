using Unity.Entities;
using UnityEngine;
using System.Diagnostics;

// Disabling auto-creation so we can control it manually for the test.
[DisableAutoCreation]
public partial class PrimeCheckSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // This runs on the main thread for any entity that has a request
        // but not yet a result.
        foreach (var (request, entity) in SystemAPI.Query<RefRO<PrimeCheckRequest>>()
                                                    .WithNone<PrimeCheckResult>().WithEntityAccess())
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ulong n = request.ValueRO.NumberToCheck;
            byte result = IsPrime(n);

            stopwatch.Stop();

            // Add the result component to the entity so we don't check it again.
            EntityManager.AddComponentData(entity, new PrimeCheckResult { IsPrime = result });

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
                return 0; // Not prime
            }
        }
        return 1; // Prime
    }
}