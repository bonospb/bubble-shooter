using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleFlowSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble, Position>, Exc<Created>> bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;
        private readonly EcsFilterInject<Inc<Merge>> mergeFilter = default;

        private readonly EcsPoolInject<Position> positionPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (!movingFilter.Value.IsEmpty() || !mergeFilter.Value.IsEmpty())
                return;

            var rowCount = GetRowCount();
            if (rowCount > levelConfig.Value.RowsMax)
            {
                foreach (var entity in bubbleFilter.Value)
                    positionPool.Value.Get(entity).Value.y--;
            }
            else if (rowCount < levelConfig.Value.RowsMin)
            {
                foreach (var entity in bubbleFilter.Value)
                    positionPool.Value.Get(entity).Value.y++;
            }
        }
        #endregion

        #region Private methods
        private int GetRowCount()
        {
            if (bubbleFilter.Value.IsEmpty())
                return 0;

            var result = 0;
            foreach (var entity in bubbleFilter.Value)
            {
                var bubbleRow = positionPool.Value.Get(entity).Value.y;
                if (bubbleRow > result)
                    result = bubbleRow;
            }

            return result + 1;
        }
        #endregion
    }
}
