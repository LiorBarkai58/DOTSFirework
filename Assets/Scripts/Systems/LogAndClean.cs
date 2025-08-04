using Unity.Entities;
using UnityEngine;

// Tell this system to run after the job has been scheduled.
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PrimeCheckBurst))]
public partial class LogAndCleanupSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // THIS IS THE FIX:
        // This line forces the system to pause and wait until the
        // PrimeCheckJob is 100% complete before continuing.
        this.Dependency.Complete();

        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                           .CreateCommandBuffer(World.Unmanaged);

        // Now that we've waited, we can safely read the result.
        foreach (var (result, request, entity) in SystemAPI.Query<RefRO<PrimeCheckResult>, RefRO<PrimeCheckRequest>>()
                                                          .WithEntityAccess())
        {
            // This will now log the correct value written by the job.
            Debug.Log($"[PARALLEL] Result: {request.ValueRO.NumberToCheck} is prime? {result.ValueRO.IsPrime == 1}");

            // Clean up the entity by removing the request component.
            ecb.RemoveComponent<PrimeCheckRequest>(entity);
        }
    }
}