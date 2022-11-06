using DG.Tweening;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Views
{
    public class PredicationView : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private SpriteRenderer _renderer = default;
        #endregion

        #region Public
        public SpriteRenderer Renderer => _renderer;
        #endregion

        #region Unity methods
        private void OnDestroy() =>
            DOTween.Kill(_renderer.transform);
        #endregion
    }
}
