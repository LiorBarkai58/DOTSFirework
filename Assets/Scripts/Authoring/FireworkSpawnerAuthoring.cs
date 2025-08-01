using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class FireworkSpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int count = 20;
        public float MinSpeed;
        public float MaxSpeed;

        class Baker : Baker<FireworkSpawnerAuthoring>
        {
            public override void Bake(FireworkSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new FireworkSpawner
                {
                    Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Renderable),
                    Count = authoring.count,
                    MinSpeed = authoring.MinSpeed,
                    MaxSpeed = authoring.MaxSpeed,
                });
            }
        }
    }

    
}