using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class TrajectorySystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<InputHeld, WorldPosition>> inputFilter = default;
        private readonly EcsFilterInject<Inc<Bubble, Position, UnityObject<BubbleView>>> _bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;

        private readonly EcsPoolInject<Trajectory> trajectoryPointPool = default;
        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;
        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;
        private readonly EcsPoolInject<Prediction> predicationPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Private
        private Vector2 origin;

        private List<Vector2> trajectory;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            trajectory = new List<Vector2>();

            var rowMax = levelConfig.Value.BoardSize.y - 1;

            Hex.GetColBounds(0, levelConfig.Value.BoardSize.x - 1, out int colMin, out int colMax);

            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));
            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));

            origin = new Vector2((topLeft.x + topRight.x) * .5f, bottomRight.y);
        }

        public void Run(IEcsSystems systems)
        {
            if (inputFilter.Value.IsEmpty() || !movingFilter.Value.IsEmpty())
                return;

            trajectory.Clear();

            var position = origin;
            var direction = Vector3.zero;
            foreach (var entity in inputFilter.Value)
                direction = worldPositionPool.Value.Get(entity).Value - position;

            var hitBubbleView = HitTest(position, direction, trajectory);
            if (!hitBubbleView)
                return;

            Vector2Int hitBubblePosition = Vector2Int.zero;
            foreach (var entity in _bubbleFilter.Value)
            {
                if (bubbleViewPool.Value.Get(entity).Value == hitBubbleView)
                {
                    hitBubblePosition = positionPool.Value.Get(entity).Value;
                    break;
                }
            }

            var newBubblePosition = NewBubblePosition(hitBubblePosition, hitBubbleView.transform.position, trajectory.Last());
            if (!newBubblePosition.HasValue)
                return;

            foreach (var point in trajectory)
            {
                var trajectoryEntity = world.Value.NewEntity();
                trajectoryPointPool.Value.Add(trajectoryEntity);
                worldPositionPool.Value.Add(trajectoryEntity).Value = point;
            }

            var predictionEntity = world.Value.NewEntity();
            predicationPool.Value.Add(predictionEntity);
            positionPool.Value.Add(predictionEntity).Value = newBubblePosition.Value;
        }
        #endregion

        #region Private methods
        private BubbleView HitTest(Vector2 position, Vector2 direction, List<Vector2> trajectory)
        {
            BubbleView hitBubbleView = null;
            var reflectionCount = 0;

            trajectory.Add(position);

            while (reflectionCount <= 1)
            {
                var hit = Physics2D.Raycast(position, direction);
                if (!hit.collider)
                    break;

                trajectory.Add(hit.point);

                if (hit.collider.TryGetComponent(out hitBubbleView))
                    break;

                direction = Vector2.Reflect(direction, hit.normal);
                position = hit.point + hit.normal * 0.01f;

                reflectionCount++;
            }

            return hitBubbleView;
        }

        private Vector2Int? NewBubblePosition(Vector2Int hitBubbleCoord, Vector2 hitViewPosition, Vector2 hitPoint)
        {
            if (hitPoint.x <= hitViewPosition.x && hitPoint.y > hitViewPosition.y)
            {
                var left = Hex.GetNeighbourCoord(hitBubbleCoord, Neighbour.Left);
                if (PositionFree(left))
                    return left;
            }

            if (hitPoint.x > hitViewPosition.x && hitPoint.y > hitViewPosition.y)
            {
                var right = Hex.GetNeighbourCoord(hitBubbleCoord, Neighbour.Right);
                if (PositionFree(right))
                    return right;
            }

            if (hitPoint.x <= hitViewPosition.x && hitPoint.y <= hitViewPosition.y)
            {
                var bottomLeft = Hex.GetNeighbourCoord(hitBubbleCoord, Neighbour.BottomLeft);
                if (PositionFree(bottomLeft))
                    return bottomLeft;
            }

            if (hitPoint.x > hitViewPosition.x && hitPoint.y <= hitViewPosition.y)
            {
                var bottomRight = Hex.GetNeighbourCoord(hitBubbleCoord, Neighbour.BottomRight);
                if (PositionFree(bottomRight))
                    return bottomRight;
            }

            return null;
        }

        private bool PositionFree(Vector2Int position)
        {
            foreach (var entity in _bubbleFilter.Value)
            {
                if (positionPool.Value.Get(entity).Value == position)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
