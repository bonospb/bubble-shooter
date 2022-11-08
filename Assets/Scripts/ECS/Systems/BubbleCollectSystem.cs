using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleCollectSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<Bubble, Collect, Position>> newBubbleFilter = default;
        private readonly EcsFilterInject<Inc<Bubble, Position>> bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;

        private readonly EcsPoolInject<Collect> collectPool = default;
        private readonly EcsPoolInject<Collecting> collectingPool = default;
        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<Bubble> bubblePool = default;

        private readonly EcsCustomInject<IGameConfig> gameConfig = default;
        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Private
        private Dictionary<Vector2Int, EcsPackedEntity> map = null;
        private HashSet<EcsPackedEntity> toCollect = null;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            var boardSize = levelConfig.Value.BoardSize.x * levelConfig.Value.BoardSize.y;
            map = new Dictionary<Vector2Int, EcsPackedEntity>(boardSize);
            toCollect = new HashSet<EcsPackedEntity>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!movingFilter.Value.IsEmpty() || newBubbleFilter.Value.IsEmpty())
                return;

            UpdateMap();

            var newBubbleValue = -1;
            var newBubblePosition = Vector2Int.zero;
            foreach (var entity in newBubbleFilter.Value)
            {
                collectPool.Value.Del(entity);

                newBubbleValue = bubblePool.Value.Get(entity).Value;
                newBubblePosition = positionPool.Value.Get(entity).Value;
            }

            toCollect.Clear();

            GetBubblesToMerge(newBubblePosition, newBubbleValue, toCollect);

            if (toCollect.Count < gameConfig.Value.MinMergeCount)
                return;

            foreach (var packedEntity in toCollect)
            {
                if (!packedEntity.Unpack(world.Value, out var entity))
                    continue;

                collectingPool.Value.Add(entity);
                positionPool.Value.Del(entity);
            }
        }
        #endregion

        #region Private methods
        private void UpdateMap()
        {
            map.Clear();

            foreach (var entity in bubbleFilter.Value)
                map[positionPool.Value.Get(entity).Value] = world.Value.PackEntity(entity);
        }

        private void GetBubblesToMerge(Vector2Int position, int value, HashSet<EcsPackedEntity> result)
        {
            foreach (var offset in Hex.NeighboursOffsets)
            {
                var neighbourPosition = position + offset;
                if (!map.TryGetValue(neighbourPosition, out var neighbour))
                    continue;

                if (!neighbour.Unpack(world.Value, out var neighbourEntity))
                    continue;

                var neighbourValue = bubblePool.Value.Get(neighbourEntity).Value;
                if (neighbourValue == value && result.Add(neighbour))
                    GetBubblesToMerge(neighbourPosition, neighbourValue, result);
            }
        }
        #endregion
    }
}
