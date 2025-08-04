using Unity.Entities;

// Component to hold the number we want to check.
// This will be added to an entity to trigger the calculation.
public struct PrimeCheckRequest : IComponentData
{
    public ulong NumberToCheck;
}

// Component to store the result of the calculation.
public struct PrimeCheckResult : IComponentData
{
    // 1 for Prime, 0 for Not Prime. Bools are not blittable for jobs.
    public byte IsPrime;
}