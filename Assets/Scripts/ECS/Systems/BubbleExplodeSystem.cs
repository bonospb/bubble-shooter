using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public class BubbleExplodeSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble, Position>> _bubbleFilter = default;
        private readonly EcsFilterInject<Inc<Collecting>> collectingFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> _movingFilter = default;

        private readonly EcsPoolInject<Bubble> bubblePool = default;
        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<Collecting> collectingPool = default;
        private readonly EcsPoolInject<Destroyed> destroyedPool = default;
        #endregion

        #region Implementation 
        public void Run(IEcsSystems systems)
        {
            if (collectingFilter.Value.IsEmpty() || !_movingFilter.Value.IsEmpty())
                return;

            foreach (var entity in collectingFilter.Value)
            {
                bubblePool.Value.Del(entity);
                positionPool.Value.Del(entity);
                collectingPool.Value.Del(entity);
                destroyedPool.Value.Add(entity);
            }
        }
        #endregion
    }
}
