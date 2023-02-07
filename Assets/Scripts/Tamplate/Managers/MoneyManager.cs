using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MoneyManager : BaseSingleton<MoneyManager>
    {
        [SerializeField] GameInfoSO _gameInfoSO;
        #region UNITY_METHODS
        void OnEnable()
        {
            EventManager.StartListening(Events.ADD_MONEY, onMoneyAddEvent);
        }
        void OnDisable()
        {
            EventManager.StopListening(Events.ADD_MONEY, onMoneyAddEvent);
        }
        #endregion

        #region METHODS
        #endregion
        #region ACTIONS
        public void initialize()
        {
            float value = _gameInfoSO.getPlayerMoney();
            EventManager.TriggerEvent(Events.UPDATE_TOTAL_MONEY_UI, new object[] { value });
        }
        private void onMoneyAddEvent(object[] obj)
        {
            int value = (int)obj[0];
            _gameInfoSO.sumPlayerMoney(value);
            string moneyText = MoneyTextUtility.FloatToStringConverter(value);
            EventManager.TriggerEvent(Events.UPDATE_TOTAL_MONEY_UI, new object[] { moneyText, value});
            DataBase.instance.saveAllData();
        }
        #endregion
    }
}