using DG.Tweening;
using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InGameUIController : MonoBehaviour
    {
        [SerializeField] UITutorial _uiTutorial;

        #region MONEY
        [SerializeField, MyBox.Foldout("Money Things")] Text _totalMoneyText;
        [SerializeField, MyBox.Foldout("Money Things")] Transform _moneyAnimImageSpawnPos, _moneyAnimTarget;
        [SerializeField, MyBox.Foldout("Money Things")] Image _moneyAnimTargetImage;
        [SerializeField, MyBox.Foldout("Money Things")] int _maxImageCount;
        [SerializeField, MyBox.Foldout("Money Things")] float _spawnRange, _spawnDelayBtwImages = .1f;
        float _totalMoney;
        #endregion


        #region UNITY_METHODS
        void OnEnable()
        {
            EventManager.StartListening(Events.LEVEL_LOADED, onLevelLoadded);
            EventManager.StartListening(Events.UPDATE_TOTAL_MONEY_UI, onTotalMoneyUpdate);
        }
        void OnDisable()
        {
            EventManager.StopListening(Events.LEVEL_LOADED, onLevelLoadded);
            EventManager.StopListening(Events.UPDATE_TOTAL_MONEY_UI, onTotalMoneyUpdate);
        }
        #endregion

        #region METHODS
        public void initialize()
        {
            refreshUI();
        }
        private void refreshUI()
        {
            _uiTutorial.resetTutorial();
        }
        public void moneyUIAnim(float value, float minDelay)
        {
            int count = Mathf.FloorToInt(value);
            if (count > _maxImageCount)
                count = _maxImageCount;
            float randomX, randomY;
            Vector3 spawnPos;
            if (count < 3)
                count = 3;
            for (int i = 0; i < count; i++)
            {
                randomX = Random.Range(-_spawnRange / 2, _spawnRange / 2);
                randomY = Random.Range(-_spawnRange / 2, _spawnRange / 2);
                spawnPos = _moneyAnimImageSpawnPos.position + new Vector3(randomX, randomY, 0);
                GameObject temp = Instantiate(_moneyAnimTargetImage.gameObject, _moneyAnimImageSpawnPos.position, _moneyAnimTargetImage.transform.rotation, transform);
                Transform image = temp.transform;
                float delay = minDelay + (i * .01f);
                image.DOMove(spawnPos, .2f).OnComplete(() => image.DOMove(_moneyAnimTarget.position, .5f).SetDelay(delay).OnComplete(() => Destroy(temp)));
            }
        }
        #endregion

        #region ACTIONS
        private void onTotalMoneyUpdate(object[] obj)
        {
            float value = (float)obj[0];
            _totalMoney = value;
            _totalMoneyText.text = MoneyTextUtility.FloatToStringConverter(_totalMoney);
        }


        private void onLevelLoadded(object[] obj)
        {
            refreshUI();
        }      
        #endregion
    }
}