using DG.Tweening;
using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleViewFlySystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Created, UnityObject<BubbleView>>> bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Trajectory, WorldPosition>> trajectoryFilter = default;
        private readonly EcsFilterInject<Inc<Prediction, Position>> predictionFilter = default;

        private readonly EcsPoolInject<New> newPool = default;
        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;
        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Private
        private readonly List<Vector3> _path = new List<Vector3>();
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (bubbleFilter.Value.IsEmpty() || trajectoryFilter.Value.IsEmpty() || predictionFilter.Value.IsEmpty())
                return;

            _path.Clear();

            var pos = Vector2Int.zero;
            foreach (var entity in predictionFilter.Value)
                pos = positionPool.Value.Get(entity).Value;

            GetFlyPath(_path, pos);

            foreach (var bubbleEntity in bubbleFilter.Value)
            {
                var bubbleView = bubbleViewPool.Value.Get(bubbleEntity).Value;
                bubbleView.Trail.enabled = true;

                bubbleView.transform.position = _path[0];

                bubbleView.DOComplete();
                bubbleView.transform.DOPath(_path.ToArray(), levelConfig.Value.BubbleFlySpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased(true)
                    .OnComplete(() =>
                    {
                        newPool.Value.Add(bubbleEntity);

                        bubbleView.Trail.enabled = false;
                        bubbleView.SetText($"{pos.x}/{pos.y}");
                    });
            }
        }
        #endregion

        #region Private methods
        private void GetFlyPath(List<Vector3> path, Vector2Int pos)
        {
            foreach (var i in trajectoryFilter.Value)
                path.Add(worldPositionPool.Value.Get(i).Value);

            path.RemoveAt(_path.Count - 1);
            path.Add(Hex.ToWorldPosition(pos));
        }
        #endregion
    }
}
