using FreeTeam.BubbleShooter.Data;
using FreeTeam.BubbleShooter.Views;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Configuration
{
    public interface ILevelConfig
    {
        Vector2Int BoardSize { get; }

        BubbleView BubbleView { get; }
        IReadOnlyList<BubbleData> BubbleData { get; }
        int[] BubbleQueue { get; }


        int RowsMax { get; }
        int RowsMin { get; }

        float BubbleMoveSpeed { get; }
        float BubbleFlySpeed { get; }

        bool UseSeed { get; }
        int RandomSeed { get; }
    }
}
