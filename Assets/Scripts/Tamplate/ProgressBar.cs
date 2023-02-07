
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ProgressBar : MonoBehaviour 
    {
        [SerializeField] Image _progressBarImage, _bg;
        [SerializeField] Text _progressText;

        #region METHOD
        private void Awake()
        {
            _progressBarImage.fillAmount = 0;
        }
        public void updateBar(float current, float total)
        {
            if (_progressText != null)
                _progressText.text = current.ToString("0") + "/" + total.ToString("0");
            float percentage = ((float)current / (float)total);
            _progressBarImage.fillAmount = percentage;
        }
        public void updateBarWithEase(float current, float total)
        {
            float percentage = ((float)current / (float)total);
            if (_progressText != null)
                _progressText.text = current.ToString("0") + "/" + total.ToString("0");
            float from = 0;
            float to = percentage;
            int count = 0;
            DOTween.To(() => from, x => from = x, to, .5f).OnUpdate(()=> 
            {
                _progressBarImage.fillAmount = from;
            });
        }
        #endregion

        #region ACTION
        public void disableBar()
        {
            _progressBarImage.gameObject.SetActive(false);
            _bg.gameObject.SetActive(false);
        }
        public void activateBar()
        {
            _bg.gameObject.SetActive(true);
            _progressBarImage.gameObject.SetActive(true);
        }
        #endregion
    }
}