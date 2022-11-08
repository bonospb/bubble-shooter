using DG.Tweening;
using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleViewShakeSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<UnityObject<BubbleView>, Position>> _bubbleViewFilter = default;
        private readonly EcsFilterInject<Inc<Bubble, New, Position>> _newBubbleFilter = default;

        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (_newBubbleFilter.Value.IsEmpty())
                return;

            var newBubbleCoord = Vector2Int.zero;
            foreach (var entity in _newBubbleFilter.Value)
                newBubbleCoord = positionPool.Value.Get(entity).Value;

            Vector3 newBubbleViewPosition = Hex.ToWorldPosition(newBubbleCoord);

            foreach (var offset in Hex.NeighboursOffsets)
            {
                var neighbourBubbleView = GetBubbleViewAt(newBubbleCoord + offset);
                if (neighbourBubbleView == null)
                    continue;

                var punchDir = (neighbourBubbleView.transform.position - newBubbleViewPosition).normalized * .1f;

                DOTween.Complete(neighbourBubbleView.Renderer.transform);
                neighbourBubbleView.Renderer.transform.DOPunchPosition(punchDir, 0.15f, 0);
            }
        }
        #endregion

        #region Private
        private BubbleView GetBubbleViewAt(Vector2Int coord)
        {
            foreach (var entity in _bubbleViewFilter.Value)
            {
                if (positionPool.Value.Get(entity).Value == coord)
                    return bubbleViewPool.Value.Get(entity).Value;
            }

            return null;
        }
        #endregion
    }
}
