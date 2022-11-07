using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BubbleConnectionSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<Bubble, Position>, Exc<Next>> bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;

        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<Connected> connectedPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Private
        private Dictionary<Vector2Int, EcsPackedEntity> map = default;
        private (List<EcsPackedEntity> a, List<EcsPackedEntity> b) cache = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            var size = levelConfig.Value.BoardSize.x * levelConfig.Value.BoardSize.y;

            map = new Dictionary<Vector2Int, EcsPackedEntity>(size);

            cache.a = new List<EcsPackedEntity>(size);
            cache.b = new List<EcsPackedEntity>(size);
        }

        public void Run(IEcsSystems systems)
        {
            if (!movingFilter.Value.IsEmpty())
                return;

            UpdateBubbleMap();

            cache.a.Clear();
            cache.b.Clear();

            Hex.GetColBounds(0, levelConfig.Value.BoardSize.x, out var start, out var end);

            foreach (var entity in bubbleFilter.Value)
            {
                if (positionPool.Value.Get(entity).Value.y != 0)
                    continue;

                connectedPool.Value.Add(entity);

                cache.a.Add(world.Value.PackEntity(entity));
            }

            while (cache.a.Count > 0)
            {
                cache.b.Clear();

                foreach (var bubble in cache.a)
                {
                    if (bubble.Unpack(world.Value, out var entity))
                    {
                        var position = positionPool.Value.Get(entity).Value;
                        foreach (var offset in Hex.NeighboursOffsets)
                        {
                            var neighbourEntity = GetBubbleFromMap(position + offset);
                            if ((neighbourEntity < 0) || connectedPool.Value.Has(neighbourEntity))
                                continue;

                            connectedPool.Value.Add(neighbourEntity);
                            cache.b.Add(world.Value.PackEntity(neighbourEntity));
                        }
                    }
                }

                (cache.a, cache.b) = (cache.b, cache.a);
            }
        }
        #endregion

        #region Private methods
        private void UpdateBubbleMap()
        {
            map.Clear();

            foreach (var entity in bubbleFilter.Value)
            {
                var position = positionPool.Value.Get(entity).Value;
                map[position] = world.Value.PackEntity(entity);
            }
        }

        private int GetBubbleFromMap(Vector2Int coord)
        {
            if (map.TryGetValue(coord, out var bubble) && bubble.Unpack(world.Value, out var entity))
                return entity;

            return -1;
        }
        #endregion
    }
}
