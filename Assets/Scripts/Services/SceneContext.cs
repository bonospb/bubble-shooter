using FreeTeam.BubbleShooter.Views;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Services
{
    internal class SceneContext : ISceneContext
    {
        #region SerializeFields
        [SerializeField] private Camera _camera = default;
        [SerializeField] private SpriteRenderer _background = default;
        [SerializeField] private Transform _bubbleViewRoot = default;
        [SerializeField] private LineRenderer _trajectoryRenderer = default;
        [SerializeField] private PredicationView _predicationView = default;
        [SerializeField] private ParticleSystem _destroyParticles = default;
        #endregion

        #region Public
        public Camera Camera => _camera;
        public SpriteRenderer Background => _background;
        public Transform BubbleViewRoot => _bubbleViewRoot;
        public LineRenderer TrajectoryRenderer => _trajectoryRenderer;
        public PredicationView PredicationView => _predicationView;
        public ParticleSystem DestroyParticles => _destroyParticles;
        #endregion
    }
}
