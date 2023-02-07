using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelManager : BaseSingleton<LevelGenerator>
    {
        [SerializeField] private LevelSO[] _levelPrefabs;
        private Level _currentLevelPrefab;
        private int _currentLevel = 0;
        [SerializeField] LevelGenerator _levelGenerator;
        [SerializeField] int _startIndex = 0;

        #region UNITY_METHODS
        #endregion

        #region METHODS
        public void initialize()
        {
            loadLevel(_currentLevel);
        }
        
        public void setCurrentLevel(int currentIndex)
        {
            _currentLevel = currentIndex;
        }
      
        private void loadLevel(int index)
        {
            if (_levelPrefabs.Length == 0)
            {
                Debug.Log("there is no attached level");
                return;
            }
            if (index < 0)
            {
                Debug.LogError("Invalid level index: " + index);
                return;
            }
            if (index >= _levelPrefabs.Length)
            {
                index %= _levelPrefabs.Length;
                if (index < _startIndex)
                    index = _startIndex;
            }
                // Destroy any existing level
            if (_currentLevelPrefab != null)
            {
                Destroy(_currentLevelPrefab.gameObject);
            }

            // Instantiate the new level
            _currentLevelPrefab = _levelGenerator.generateLevel(_levelPrefabs[index].getLevelPrefab());
            EventManager.TriggerEvent(Events.LEVEL_LOADED, new object[] { });
        }
        #endregion

        #region ACTIONS
        public void loadSameLevel()
        {
            loadLevel(_currentLevel);
        }
        public void loadNextLevel()
        {
            _currentLevel++;
            loadLevel(_currentLevel);
        }
        #endregion
    }
}