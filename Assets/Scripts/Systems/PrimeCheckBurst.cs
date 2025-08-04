using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using System.Diagnostics;

// Disabling auto-creation so we can control it manually for the test.
[UpdateInGroup(typeof(SimulationSystemGroup))]
[DisableAutoCreation]
public partial class PrimeCheckBurst : SystemBase
{
    private Stopwatch stopwatch = new Stopwatch();

    
    protected override void OnStartRunning()
    {
       
        stopwatch.Restart();
    }

    protected override void OnUpdate()
    {
        
        this.Dependency = new PrimeCheckJob().ScheduleParallel(this.Dependency);
    }

    protected override void OnStopRunning()
    {
        // This is called when the query is empty (all jobs are done).
        stopwatch.Stop();
        UnityEngine.Debug.Log($"[PARALLEL] Total job execution time: {stopwatch.Elapsed.TotalMilliseconds:F4} ms");
    }
}


[BurstCompile]
public partial struct PrimeCheckJob : IJobEntity
{
    
    public void Execute(in PrimeCheckRequest request, ref PrimeCheckResult result)
    {
        ulong n = request.NumberToCheck;

        if (n <= 1) { result.IsPrime = 0; return; }
        if (n <= 3) { result.IsPrime = 1; return; }
        if (n % 2 == 0 || n % 3 == 0) { result.IsPrime = 0; return; }

        ulong limit = (ulong)System.Math.Sqrt(n);
        for (ulong i = 5; i <= limit; i = i + 6)
        {
            if (n % i == 0 || n % (i + 2) == 0)
            {
                result.IsPrime = 0; // Not prime
                return;
            }
        }
        result.IsPrime = 1; // Prime
    }
}