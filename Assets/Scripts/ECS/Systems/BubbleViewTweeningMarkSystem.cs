using DG.Tweening;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class BubbleViewTweeningMarkSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<UnityObject<BubbleView>>> viewFilter = default;

        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;
        private readonly EcsPoolInject<Moving> movingPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in viewFilter.Value)
            {
                var view = bubbleViewPool.Value.Get(entity).Value;
                if (DOTween.IsTweening(view.transform))
                {
                    if (!movingPool.Value.Has(entity))
                        movingPool.Value.Add(entity);
                }
                else
                {
                    movingPool.Value.Del(entity);
                }
            }
        }
        #endregion
    }
}
