using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleViewFallSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Falling, UnityObject<BubbleView>>> bubbleFilter = default;

        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var i in bubbleFilter.Value)
            {
                var bubbleView = bubbleViewPool.Value.Get(i).Value;
                bubbleView.Collider.enabled = false;
                if (bubbleView.Rigidbody.bodyType == RigidbodyType2D.Static)
                {
                    bubbleView.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    bubbleView.Rigidbody.AddForce(Vector2.right * Random.Range(-2f, 2f), ForceMode2D.Impulse);
                }
            }
        }
        #endregion
    }
}
