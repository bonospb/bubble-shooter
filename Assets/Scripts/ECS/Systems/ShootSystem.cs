using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class ShootSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<InputReleased>> inputFilter = default;
        private readonly EcsFilterInject<Inc<Trajectory>> trajectoryFilter = default;
        private readonly EcsFilterInject<Inc<Prediction, Position>> predictionFilter = default;
        private readonly EcsFilterInject<Inc<Bubble, Next>> nextBubbleFilter = default;
        private readonly EcsFilterInject<Inc<Moving>> movingFilter = default;

        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<Created> createdPool = default;
        private readonly EcsPoolInject<Collect> collectPool = default;
        private readonly EcsPoolInject<Next> nextPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (!movingFilter.Value.IsEmpty())
                return;

            if (inputFilter.Value.IsEmpty() || trajectoryFilter.Value.IsEmpty() || predictionFilter.Value.IsEmpty())
                return;

            var pos = Vector2Int.zero;
            foreach (var entity in predictionFilter.Value)
                pos = positionPool.Value.Get(entity).Value;

            foreach (var bubble in nextBubbleFilter.Value)
            {
                positionPool.Value.Add(bubble).Value = pos;
                createdPool.Value.Add(bubble);
                collectPool.Value.Add(bubble);
                nextPool.Value.Del(bubble);
            }
        }
        #endregion
    }
}
