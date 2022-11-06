using FreeTeam.BubbleShooter.Views;
using FreeTeam.Editor;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Configuration
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "BubbleShooterGame/GameConfig")]
    public sealed class GameConfig : ScriptableObject, IGameConfig
    {
        #region SerializeFields
        [Foldout("Game settings", true)]
        [SerializeField] private byte _minMergeCount = 3;

        [Foldout("Views settings")]
        [SerializeField] private MergePopupView _mergePopupText = default;

        [Space(10)]
        [SerializeField] private LevelConfig[] levelConfigs = default;
        #endregion

        #region Public
        public ILevelConfig[] LevelConfigs => levelConfigs;

        public MergePopupView MergePopupText => _mergePopupText;

        public byte MinMergeCount => _minMergeCount;
        #endregion
    }
}
