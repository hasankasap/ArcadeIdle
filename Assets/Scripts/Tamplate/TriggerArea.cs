using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class TriggerArea : MonoBehaviour
    {
        [SerializeField] TriggerAreaEvent _enterEvent = new TriggerAreaEvent();
        [SerializeField] TriggerAreaEvent _exitEvent = new TriggerAreaEvent();
        #region UNITY_METHODS
        private void OnTriggerEnter(Collider other)
        {
            _enterEvent.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _exitEvent.Invoke(other);
        }
        #endregion

        #region METHODS
        #endregion
        #region ACTIONS
        #endregion
    }
    [System.Serializable]
    public class TriggerAreaEvent : UnityEvent<Collider>
    {
    }
}