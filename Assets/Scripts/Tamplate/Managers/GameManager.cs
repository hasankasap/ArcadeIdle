using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : BaseSingleton<GameManager>
    {
        [SerializeField] LevelManager _levelManager;
        [SerializeField] DataBase _dataBase;
        [SerializeField] InGameUIController _gameUIController;
        [SerializeField] MoneyManager _moneyManager;

        [SerializeField] GameInfoSO _gameInfoSO;

        #region UNITY_METHODS
        void Start()
        {
            initialize();
        }
        #endregion

        #region METHODS
        void initialize()
        {
            _dataBase.loadAllData();
            if (_gameInfoSO != null)
            {
                _levelManager.setCurrentLevel(_gameInfoSO.getLevelData());
                _levelManager.initialize();
                _moneyManager.initialize();
            }
        }
        #endregion

        #region ACTIONS
        public void onNextLevelAction()
        {
            _gameInfoSO.increaseLevelCount();
            _dataBase.saveAllData();
            _levelManager.loadNextLevel();
            if (_gameInfoSO != null)
            {
                _moneyManager.initialize();
            }
        }
        public void onRetryAction()
        {
            _dataBase.saveAllData();
            _levelManager.loadSameLevel();
        }
        #endregion

        [MyBox.ButtonMethod]
        private void resetData()
        {
            _dataBase.resetallData();
            _dataBase.saveAllData();
        }
    }
}