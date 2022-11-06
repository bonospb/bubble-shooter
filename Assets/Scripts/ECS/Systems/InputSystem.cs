using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class InputSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsPoolInject<InputHeld> inputHeldPool = default;
        private readonly EcsPoolInject<InputReleased> inputReleasedPool = default;
        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;

        private readonly EcsCustomInject<ISceneContext> sceneContext = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            int entity = -1;

            if (Input.GetMouseButton(0))
            {
                entity = world.Value.NewEntity();
                inputHeldPool.Value.Add(entity);
            }

            if (Input.GetMouseButtonUp(0))
            {
                entity = world.Value.NewEntity();
                inputReleasedPool.Value.Add(entity);
                inputHeldPool.Value.Add(entity);
            }

            if (entity >= 0)
                worldPositionPool.Value.Add(entity).Value = sceneContext.Value.Camera.ScreenToWorldPoint(Input.mousePosition);
        }
        #endregion
    }
}
