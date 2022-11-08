using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class BubbleFallSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble, Position>, Exc<Connected>> bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;

        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<Collect> mergePool = default;
        private readonly EcsPoolInject<Falling> fallingPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (!movingFilter.Value.IsEmpty())
                return;

            foreach (var entity in bubbleFilter.Value)
            {
                positionPool.Value.Del(entity);
                mergePool.Value.Del(entity);
                fallingPool.Value.Add(entity);
            }
        }
        #endregion
    }
}
