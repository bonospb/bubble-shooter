using Leopotam.EcsLite;

public static class EcsFilterExtensions
{
    public static bool IsEmpty(this EcsFilter ecsFilter) =>
        ecsFilter.GetEntitiesCount() <= 0;
}
