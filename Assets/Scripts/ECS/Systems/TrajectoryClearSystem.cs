using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class TrajectoryClearSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<Trajectory>> trajectoryFilter = default;
        private readonly EcsFilterInject<Inc<Prediction>> predicationFilter = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in trajectoryFilter.Value)
                world.Value.DelEntity(entity);

            foreach (var entity in predicationFilter.Value)
                world.Value.DelEntity(entity);
        }
        #endregion
    }
}
