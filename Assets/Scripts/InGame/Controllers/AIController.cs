using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class AIController : Character
    {
        #region PRIVATE_VARIABLES

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY_METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        #endregion

        #region METHODS
        protected override void initialize()
        {
            base.initialize();
        }
        protected override void movement()
        {
            base.movement();
        }

        private IEnumerator movemet()
        {
            yield return new WaitForSeconds(1);
        }

        #region STACK_METHODS
        #endregion

        #endregion

        #region ACTIONS
        protected override void onGameStarted(object[] obj)
        {
            base.onGameStarted(obj);
        }
        #endregion
    }
}