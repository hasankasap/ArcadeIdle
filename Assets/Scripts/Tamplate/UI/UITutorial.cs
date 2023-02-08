using DG.Tweening;
using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UITutorial : MonoBehaviour
    {
        [SerializeField, ReadOnly]bool _isPressed;
        [SerializeField] GameObject _tutorial, _animatons;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] Image _joystick;
        Tween _fadeOutTween;
        #region UNITY_METHODS
        void Start()
        {
            resetTutorial();
            _animatons.SetActive(true);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isPressed)
            {
                _isPressed = true;
                if (_fadeOutTween != null)
                    _fadeOutTween.Kill();
                _fadeOutTween = _canvasGroup.DOFade(0, .5f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    onTutorialClosed();
                });
            }
        }
        #endregion

        #region METHODS
        public void resetTutorial()
        {
            if (_fadeOutTween != null)
                _fadeOutTween.Kill();
            _isPressed = false;
            _canvasGroup.alpha = 1;
            _joystick.raycastTarget = false;
            _tutorial.SetActive(true);
        }
        #endregion

        #region ACTIONS
        public void onTutorialClosed()
        {
            _joystick.raycastTarget = true;
            _tutorial.SetActive(false);
            EventManager.TriggerEvent(Events.GAME_STARTED, new object[] { });
        }
        #endregion
    }
}