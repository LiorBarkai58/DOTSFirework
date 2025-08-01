using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class VelocityAuthoring : MonoBehaviour
    {

        class Baker : Baker<VelocityAuthoring>
        {
            public override void Bake(VelocityAuthoring authoring)
            {
                AddComponent(new VelocityComponent
                {
                });
            }
        }
    }
}