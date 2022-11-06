namespace FreeTeam.BubbleShooter.ECS.Components
{
    public struct UnityObject<T> where T : UnityEngine.Component
    {
        public T Value;
    }
}
