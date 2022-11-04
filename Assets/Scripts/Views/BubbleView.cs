using DG.Tweening;
using FreeTeam.BP.Editor;
using TMPro;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Views
{
    public class BubbleView : MonoBehaviour
    {
        #region SerializeFields
        [Foldout("Rendered")]
        [SerializeField] private SpriteRenderer _renderer = default;

        [Foldout("Physics", true)]
        [SerializeField] private Collider2D _collider = default;
        [SerializeField] private Rigidbody2D _rigidbody = default;

        [Foldout("Effects")]
        [SerializeField] private TrailRenderer _trail = default;

        [Foldout("Debug")]
        [SerializeField] private TextMeshPro _debugLabel = default;
        #endregion

        #region Public
        public SpriteRenderer Renderer => _renderer;
        public Collider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public TrailRenderer Trail => _trail;
        #endregion

        #region Unity methods
        private void Start()
        {
#if UNITY_EDITOR
            _debugLabel.gameObject.SetActive(true);
#else
            _debugLabel.gameObject.SetActive(false);
#endif
        }

        private void OnDestroy()
        {
            DOTween.Kill(_renderer.transform);
        }
        #endregion

        #region Public methods
        public void SetText(string value) =>
            _debugLabel.SetText(value);
        #endregion
    }
}
