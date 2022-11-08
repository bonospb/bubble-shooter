using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public class BubbleFallDeathSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble, Falling, UnityObject<BubbleView>>> filter = default;

        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;
        private readonly EcsPoolInject<Destroyed> destroyedPool = default;
        private readonly EcsPoolInject<Bubble> bubblePool = default;
        private readonly EcsPoolInject<Falling> fallingPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Private
        private float _borderY;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            _borderY = Hex.ToWorldPosition(new Vector2Int(0, levelConfig.Value.BoardSize.y)).y;
            _borderY += levelConfig.Value.BubbleView.Renderer.sprite.bounds.size.y;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var view = bubbleViewPool.Value.Get(entity).Value;
                if (view.transform.position.y < _borderY)
                {
                    destroyedPool.Value.Add(entity);
                    fallingPool.Value.Del(entity);
                    bubblePool.Value.Del(entity);
                }
            }
        }
        #endregion
    }
}
