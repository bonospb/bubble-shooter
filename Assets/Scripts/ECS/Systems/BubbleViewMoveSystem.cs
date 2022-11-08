using DG.Tweening;
using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleViewMoveSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble, UnityObject<BubbleView>>, Exc<Moving, Created>> filter = default;

        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;
        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;
        private readonly EcsPoolInject<Position> positionPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var bubbleView = bubbleViewPool.Value.Get(entity).Value;

                Vector2 position;

                if (worldPositionPool.Value.Has(entity))
                    position = worldPositionPool.Value.Get(entity).Value;
                else if (positionPool.Value.Has(entity))
                    position = Hex.ToWorldPosition(positionPool.Value.Get(entity).Value);
                else
                    continue;

                if ((Vector2)bubbleView.transform.position == position)
                    continue;

                bubbleView.transform.DOComplete();
                bubbleView.transform.DOLocalMove(position, levelConfig.Value.BubbleMoveSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased(true)
                    .OnComplete(() =>
                    {
                        if (positionPool.Value.Has(entity))
                        {
                            var pos = positionPool.Value.Get(entity).Value;
                            bubbleView.SetText($"{pos.x}/{pos.y}");
                        }
                    });

            }
        }
        #endregion
    }
}
