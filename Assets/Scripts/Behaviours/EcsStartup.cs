using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS
{
    [RequireComponent(typeof(SceneContext))]
    public class EcsStartup : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private int _levelIdx = 0;
        [SerializeField] private GameConfig _gameConfig = default;
        #endregion

        #region private
        private EcsWorld _world = null;
        private EcsSystems _systems = null;
        #endregion

        #region Unity methods
        private void Start()
        {
            Application.targetFrameRate = 60;

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems


#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(GetComponent<ISceneContext>())
                .Inject(_gameConfig)

                .Init();
        }

        private void Update() =>
            _systems?.Run();

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;

            _world?.Destroy();
            _world = null;
        }
        #endregion
    }
}
