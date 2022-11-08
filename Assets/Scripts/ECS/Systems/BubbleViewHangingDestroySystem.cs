using DG.Tweening;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleViewHangingDestroySystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<UnityObject<BubbleView>>, Exc<Bubble, Moving>> filter = default;

        private readonly EcsPoolInject<Destroyed> destroyedPool = default;
        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;

        private readonly EcsCustomInject<ISceneContext> sceneContext = default;
        #endregion

        #region Implements
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var view = bubbleViewPool.Value.Get(entity).Value;

                DOTween.Complete(view.transform);

                if (destroyedPool.Value.Has(entity))
                {
                    var particleParams = new ParticleSystem.EmitParams
                    {
                        position = view.transform.localPosition,
                        startColor = view.Renderer.color,
                        applyShapeToPosition = true
                    };

                    sceneContext.Value.DestroyParticles.Emit(particleParams, 10);
                }

                bubbleViewPool.Value.Del(entity);

                Object.Destroy(view.gameObject);
            }
        }
        #endregion
    }
}
