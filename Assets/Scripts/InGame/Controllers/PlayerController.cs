using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class PlayerController : Character
    {
        #region PRIVATE_VARIABLES
        FloatingJoystick _joystick;
        private bool _canPlay = false;
        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY_METHODS
        protected override void Start()
        {
            base.Start();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        private void Update()
        {
            if (_canPlay)
            {
                movement();
                if (Input.GetMouseButtonUp(0))
                {
                    if (!_navMeshAgent.isStopped)
                        _navMeshAgent.isStopped = true;
                    if (_animator != null)
                    {
                        _animator.SetBool("Run", false);
                    }
                }
            }
        }
        #endregion

        #region METHODS
        protected override void initialize()
        {
            base.initialize();
            _joystick = FindObjectOfType<FloatingJoystick>();
        }
        protected override void movement()
        {
            base.movement();
            if (_joystick == null)
                return;
            Vector3 movementDir = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical) * sensitivity;
            if (movementDir.magnitude > .05f)
            {
                if (_navMeshAgent.isStopped)
                    _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(transform.position + movementDir);
                if (_animator != null)
                {
                    _animator.SetBool("Run", true);
                }
            }
                
        }
        #endregion

        #region ACTIONS
        protected override void onGameStarted(object[] obj)
        {
            base.onGameStarted(obj);
            _canPlay = true;
        }
        #endregion
    }
}