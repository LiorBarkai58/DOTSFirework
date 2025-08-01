using Unity.Entities;

namespace Components
{
    public struct FireworkSpawner : IComponentData
    {
        public Entity Prefab;
        public int Count;
        public float MinSpeed;
        public float MaxSpeed;
    }
}