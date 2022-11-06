using FreeTeam.BubbleShooter.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class InputClearSystem : IEcsRunSystem
    {
        #region Private
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<InputHeld>> heldInputFilter = default;
        private readonly EcsFilterInject<Inc<InputReleased>> releasedInputFilter = default;
        #endregion

        #region Implements
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in heldInputFilter.Value)
                world.Value.DelEntity(entity);

            foreach (var entity in releasedInputFilter.Value)
                world.Value.DelEntity(entity);
        }
        #endregion
    }
}
