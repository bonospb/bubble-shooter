using FreeTeam.BubbleShooter.Views;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Services
{
    internal class SceneContext : MonoBehaviour, ISceneContext
    {
        #region SerializeFields
        [SerializeField] private Camera _camera = default;
        [SerializeField] private SpriteRenderer _background = default;
        [SerializeField] private Transform _bubbleViewContainer = default;
        [SerializeField] private LineRenderer _trajectoryRenderer = default;
        [SerializeField] private PredicationView _predicationView = default;
        [SerializeField] private ParticleSystem _destroyParticles = default;
        [SerializeField] private EdgeCollider2D _edgeCollider = default;
        #endregion

        #region Public
        public Camera Camera => _camera;

        public SpriteRenderer Background => _background;
        public LineRenderer TrajectoryRenderer => _trajectoryRenderer;

        public Transform BubbleViewContainer => _bubbleViewContainer;

        public PredicationView PredicationView => _predicationView;
        public ParticleSystem DestroyParticles => _destroyParticles;

        public EdgeCollider2D EdgeCollider => _edgeCollider;
        #endregion
    }
}
