using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class TrajectorySystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<InputHeld, WorldPosition>> inputFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;

        private readonly EcsPoolInject<Trajectory> trajectoryPointPool = default;
        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;

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

            HitTest(position, direction, trajectory);

            foreach (var point in trajectory)
            {
                var trajectoryEntity = world.Value.NewEntity();
                trajectoryPointPool.Value.Add(trajectoryEntity);
                worldPositionPool.Value.Add(trajectoryEntity).Value = point;
            }
        }
        #endregion

        #region Private methods
        private void HitTest(Vector2 position, Vector2 direction, List<Vector2> trajectory)
        {
            var reflectionCount = 0;

            trajectory.Add(position);

            while (reflectionCount <= 1)
            {
                var hit = Physics2D.Raycast(position, direction);
                if (!hit.collider)
                    break;

                trajectory.Add(hit.point);

                if (hit.collider.TryGetComponent<BubbleView>(out _))
                    break;

                direction = Vector2.Reflect(direction, hit.normal);
                position = hit.point + hit.normal * 0.01f;

                reflectionCount++;
            }
        }
        #endregion
    }
}
