using FreeTeam.BubbleShooter.Data;
using FreeTeam.BubbleShooter.Views;
using FreeTeam.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Configuration
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "BubbleShooterGame/LevelConfig")]
    public sealed class LevelConfig : ScriptableObject, ILevelConfig
    {
        #region SerializeFields
        [Foldout("Board settings", true)]
        [SerializeField] private Vector2Int _boardSize = default;
        [SerializeField] private int _rowsMax = default;
        [SerializeField] private int _rowsMin = default;

        [Foldout("Start settings", true)]
        [SerializeField] private bool _useSeed = default;
        [SerializeField] private int _randomSeed = default;

        [Foldout("Bubble settings", true)]
        [SerializeField] private float _bubbleMoveSpeed = default;
        [SerializeField] private float _bubbleFlySpeed = default;
        [Space(10)]
        [SerializeField] private BubbleView _bubbleView = default;
        [Space(10)]
        [SerializeField] private BubbleData[] _bubbleData = default;
        [Space(10)]
        [SerializeField] private int[] _bubbleQueue = default;
        #endregion

        #region Public
        public Vector2Int BoardSize => _boardSize;

        public BubbleView BubbleView => _bubbleView;
        public IReadOnlyList<BubbleData> BubbleData => _bubbleData;
        public int[] BubbleQueue => _bubbleQueue;

        public int RowsMax => _rowsMax;
        public int RowsMin => _rowsMin;

        public float BubbleMoveSpeed => _bubbleMoveSpeed;
        public float BubbleFlySpeed => _bubbleFlySpeed;

        public bool UseSeed => _useSeed;
        public int RandomSeed => _randomSeed;
        #endregion
    }

}
