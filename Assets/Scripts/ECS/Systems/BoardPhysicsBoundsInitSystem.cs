using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class BoardPhysicsBoundsInitSystem : IEcsInitSystem
    {
        #region Inject
        private readonly EcsCustomInject<ISceneContext> sceneContext = default;
        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            var rowMax = levelConfig.Value.BoardSize.y - 1;

            Hex.GetColBounds(0, levelConfig.Value.BoardSize.x - 1, out int colMin, out int colMax);

            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));
            var bottomLeft = Hex.ToWorldPosition(new Vector2Int(colMin, rowMax));
            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));

            var edgeCollider = sceneContext.Value.EdgeCollider;
            edgeCollider.points = new[] { bottomLeft, topLeft, topRight, bottomRight };
        }
        #endregion
    }
}
