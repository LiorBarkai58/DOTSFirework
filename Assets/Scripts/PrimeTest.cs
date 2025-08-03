using UnityEngine;
using Unity.Entities;

public class PrimeTest : MonoBehaviour
{
    // A large prime number to provide a good workload.
    public ulong numberToTest = 1_000_000_007;

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;

        // --- Run Sequential Test ---
        var sequentialSystem = world.GetOrCreateSystemManaged<PrimeCheckSystem_Sequential>();
        var entitySeq = entityManager.CreateEntity();
        entityManager.AddComponentData(entitySeq, new PrimeCheckRequest { NumberToCheck = numberToTest });
        sequentialSystem.Enabled = true; // Enable the system
        sequentialSystem.Update();       // Force an update to run the test
        sequentialSystem.Enabled = false; // Disable it after running
  

        Debug.Log("------------------------------------");

        // --- Run Parallel Test ---
        var parallelSystem = world.GetOrCreateSystemManaged<PrimeCheckBurst>();
        var entityPar = entityManager.CreateEntity();
        // Add the request and the result component, which the job will fill.
        entityManager.AddComponentData(entityPar, new PrimeCheckRequest { NumberToCheck = numberToTest });
        entityManager.AddComponent<PrimeCheckResult>(entityPar); // Add an uninitialized result
        parallelSystem.Enabled = true; // Enable the system to let it run its job
    }
}