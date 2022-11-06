using FreeTeam.BubbleShooter.Views;

namespace FreeTeam.BubbleShooter.Configuration
{
    public interface IGameConfig
    {
        ILevelConfig[] LevelConfigs { get; }

        byte MinMergeCount { get; }

        MergePopupView MergePopupText { get; }
    }
}
