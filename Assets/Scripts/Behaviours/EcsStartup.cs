using Assets.Scripts.ECS.Systems;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.ECS.Systems;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS
{
    [RequireComponent(typeof(SceneContext))]
    internal class EcsStartup : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private int levelIdx = 0;
        [SerializeField] private GameConfig gameConfig = default;
        #endregion

        #region Private
        private EcsWorld world = null;
        private EcsSystems systems = null;

        private ILevelConfig levelConfig = null;
        #endregion

        #region Unity methods
        private void Start()
        {
            Application.targetFrameRate = 60;

            levelConfig = gameConfig.LevelConfigs[levelIdx];

            world = new EcsWorld();
            systems = new EcsSystems(world);
            systems
                .Add(new CameraInitSystem())
                .Add(new BackgroundInitSystem())
                .Add(new BoardPhysicsBoundsInitSystem())

                .Add(new InputSystem())

                .Add(new TrajectorySystem())

                .Add(new TrajectoryViewUpdateSystem())

                .Add(new InputClearSystem())
                .Add(new TrajectoryClearSystem())

#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif

                .DelHere<WorldPosition>()

                .Inject(GetComponent<ISceneContext>())
                .Inject(gameConfig)
                .Inject(levelConfig)

                .Init();
        }

        private void Update() =>
            systems?.Run();

        private void OnDestroy()
        {
            systems?.Destroy();
            systems = null;

            world?.Destroy();
            world = null;
        }
        #endregion
    }
}
