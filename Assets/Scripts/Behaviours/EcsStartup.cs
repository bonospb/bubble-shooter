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

        private ISceneContext sceneContext = null;
        private ILevelConfig levelConfig = null;
        private IRandomService randomService = null;
        #endregion

        #region Unity methods
        private void Start()
        {
            Application.targetFrameRate = 60;

            sceneContext = GetComponent<ISceneContext>();
            levelConfig = gameConfig.LevelConfigs[levelIdx];
            randomService = new RandomService(levelConfig.UseSeed ? levelConfig.RandomSeed : null);

            world = new EcsWorld();
            systems = new EcsSystems(world);
            systems
                .Add(new CameraInitSystem())
                .Add(new BackgroundInitSystem())
                .Add(new BoardPhysicsBoundsInitSystem())
                .Add(new BoardInitSystem())

                .Add(new InputSystem())

                .Add(new NextBubbleSystem())
                .Add(new TrajectorySystem())

                .Add(new BubbleConnectionSystem())
                .Add(new BubbleFallSystem())

                .Add(new BubbleCollectSystem())
                .Add(new BubbleExplodeSystem())

                .Add(new ShootSystem())

                .Add(new BubbleFlowSystem())

                .Add(new NextBubbleViewSystem())
                .Add(new CreateBubbleViewSystem())
                .Add(new PredictionViewUpdateSystem())
                .Add(new TrajectoryViewUpdateSystem())
                .Add(new BubbleFallDeathSystem())

                .Add(new BubbleViewMoveSystem())
                .Add(new BubbleViewFlySystem())
                .Add(new BubbleViewFallSystem())
                .Add(new BubbleViewShakeSystem())

                //.Add(new MergeTextSpawnSystem())
                //.Add(new PerfectNotificationSystem())
                //.Add(new ComboMergeNotificationSystem())

                .Add(new BubbleViewTweeningMarkSystem())
                //  -   .Add(new BubbleCompleteMergeSystem())

                .Add(new BubbleViewHangingDestroySystem())

                .Add(new InputClearSystem())
                .Add(new TrajectoryClearSystem())

#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif

                .DelHere<Prediction>()
                .DelHere<Connected>()
                .DelHere<Created>()
                .DelHere<WorldPosition>()
                .DelHere<Collecting>()
                .DelHere<Destroyed>()
                .DelHere<New>()

                .Inject(sceneContext)
                .Inject(randomService)
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
