using FreeTeam.BP.Editor;
using FreeTeam.BubbleShooter.Views;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Configuration
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "BubbleShooterGame/GameConfig")]
    public sealed class GameConfig : ScriptableObject
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
        public LevelConfig[] LevelConfigs => levelConfigs;

        public MergePopupView MergePopupText => _mergePopupText;

        public byte MinMergeCount => _minMergeCount;
        #endregion
    }
}
