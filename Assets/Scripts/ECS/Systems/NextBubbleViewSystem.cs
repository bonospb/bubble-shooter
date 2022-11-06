using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class NextBubbleViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble, Next>, Exc<Moving>> _bubbleFilter = default;

        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;

        private readonly EcsCustomInject<ILevelConfig> _levelConfig = default;
        #endregion

        #region Private
        private Vector2 origin = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            Hex.GetColBounds(0, _levelConfig.Value.BoardSize.x - 1, out int colMin, out int colMax);

            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));

            var rowMax = _levelConfig.Value.BoardSize.y - 1;

            Hex.GetColBounds(rowMax, _levelConfig.Value.BoardSize.x - 1, out colMin, out colMax);

            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));

            origin = new Vector2((topLeft.x + topRight.x) * 0.5f, bottomRight.y);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bubbleFilter.Value)
            {
                if (!worldPositionPool.Value.Has(entity))
                    worldPositionPool.Value.Add(entity).Value = origin;
                else
                    worldPositionPool.Value.Get(entity).Value = origin;
            }
        }
        #endregion
    }
}
