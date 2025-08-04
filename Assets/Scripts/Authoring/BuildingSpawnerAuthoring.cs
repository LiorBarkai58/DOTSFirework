using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class BuildingSpawnerAuthoring : MonoBehaviour
    {
        public GameObject EasyPrefab;
        public GameObject MediumPrefab;
        public GameObject HardPrefab;
        public float SpawnInterval = 3f;

        class Baker : Baker<BuildingSpawnerAuthoring>
        {
            public override void Bake(BuildingSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BuildingSpawningComponent
                {
                    SpawnInterval = authoring.SpawnInterval,
                    TimeSinceLastSpawn = 0f,
                    EasyPrefab = GetEntity(authoring.EasyPrefab, TransformUsageFlags.Dynamic),
                    MediumPrefab = GetEntity(authoring.MediumPrefab, TransformUsageFlags.Dynamic),
                    HardPrefab = GetEntity(authoring.HardPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}