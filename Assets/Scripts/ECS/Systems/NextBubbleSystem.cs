using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class NextBubbleSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<Bubble, Next>> filter = default;

        private readonly EcsPoolInject<Bubble> booblePool = default;
        private readonly EcsPoolInject<Next> nextPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        private readonly EcsCustomInject<IRandomService> randomService = default;
        #endregion

        #region Private
        private int idx = 0;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (!filter.Value.IsEmpty())
                return;

            var entity = world.Value.NewEntity();
            int id;
            if (levelConfig.Value.BubbleQueue.Length > 0)
            {
                id = levelConfig.Value.BubbleQueue[idx];

                idx++;
                idx %= levelConfig.Value.BubbleQueue.Length;
            }
            else
            {
                id = randomService.Value.Range(0, levelConfig.Value.BubbleData.Count);
            }

            booblePool.Value.Add(entity).Value = levelConfig.Value.BubbleData[id].Number;
            nextPool.Value.Add(entity).Index = 0;
        }
        #endregion
    }
}
