using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class BoardInitSystem : IEcsInitSystem
    {
        #region Private
        private readonly EcsWorldInject world = default;

        private readonly EcsPoolInject<Bubble> bubblePool = default;
        private readonly EcsPoolInject<Position> positionPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        private readonly EcsCustomInject<IRandomService> randomService = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            Random.InitState(2);

            for (int row = -5; row < levelConfig.Value.RowsMin; row++)
            {
                var col = levelConfig.Value.BoardSize.x - (Mathf.Abs(row) % 2);
                Hex.GetColBounds(row, col, out var start, out var end);

                for (var q = start; q < end; q += 2)
                {
                    var random = randomService.Value.Range(0, levelConfig.Value.BubbleData.Count);
                    if (random < 0)
                        continue;

                    var entity = world.Value.NewEntity();

                    bubblePool.Value.Add(entity).Value = levelConfig.Value.BubbleData[random].Number;
                    positionPool.Value.Add(entity).Value = new Vector2Int(q, row);
                }
            }
        }
        #endregion
    }
}
