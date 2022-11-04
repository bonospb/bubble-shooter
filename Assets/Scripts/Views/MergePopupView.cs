using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Views
{
    public class MergePopupView : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private float _animDuration = default;
        [SerializeField] private float _animValue = default;
        [SerializeField] private float _fadeDuration = default;

        [SerializeField] private TextMeshPro _label = default;
        #endregion

        #region Public
        public TextMeshPro Label => _label;
        #endregion

        #region Unity methods
        private void Start()
        {
            DOTween.Sequence()
                .Join(transform.DOMoveY(_animValue, _animDuration)
                    .SetEase(Ease.OutQuart)
                    .SetRelative(true))
                .Append(DOTween.ToAlpha(() => _label.color, x => _label.color = x, 0, _fadeDuration))
                .OnComplete(() => Destroy(gameObject));
        }
        #endregion
    }
}
