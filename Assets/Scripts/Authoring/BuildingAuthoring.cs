using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Authoring
{
    public class BuildingAuthoring : MonoBehaviour
    {
        public float3 velocity;
        class Baker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                AddComponent(new BuildingMovementComponent
                {
                    Value = authoring.velocity
                });
            }
        }
    }
}