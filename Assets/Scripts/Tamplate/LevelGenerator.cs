using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelGenerator : MonoBehaviour
    {
        
        #region UNITY_METHODS
        #endregion

        #region METHODS
        public Level generateLevel(Level prefab)
        {
            return Instantiate(prefab);
        }
        #endregion
        #region ACTIONS
        #endregion
    }
}