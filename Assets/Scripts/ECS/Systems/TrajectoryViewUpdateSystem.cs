using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class TrajectoryViewUpdateSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Trajectory, WorldPosition>> trajectoryFilter = default;

        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;

        private readonly EcsCustomInject<ISceneContext> sceneContext = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            var trajectoryCount = trajectoryFilter.Value.GetEntitiesCount();

            sceneContext.Value.TrajectoryRenderer.enabled = !trajectoryFilter.Value.IsEmpty();
            sceneContext.Value.TrajectoryRenderer.positionCount = trajectoryCount;

            var i = 0;
            foreach (var entity in trajectoryFilter.Value)
            {
                sceneContext.Value.TrajectoryRenderer.SetPosition(i, worldPositionPool.Value.Get(entity).Value);
                i++;
            }
        }
        #endregion
    }
}
