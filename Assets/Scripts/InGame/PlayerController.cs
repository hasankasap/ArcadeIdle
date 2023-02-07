using DG.Tweening.Plugins.Options;
using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour, ICharacter
    {
        #region PRIVATE_VARIABLES
        [SerializeField] CharacterSO _characterSO;
        StackController _stackController;
        NavMeshAgent _navMeshAgent;

        [SerializeField, ReadOnly] bool _canPlay = false;

        private float _speedAddValue = 0;

            #region INPUT
            private Vector3 _mouseFirstPos, _mousePos, _mouseDir;
            #endregion

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region PROPERTIES
        public float speed { get { return _characterSO.speed + _speedAddValue; } set { speed = _characterSO.speed; } }
        
        public float sensitivity { get { return _characterSO.sensitivity; } set { sensitivity = _characterSO.sensitivity; } }
        public float rotationSpeed { get { return _characterSO.rotationSpeed; } set { rotationSpeed = _characterSO.rotationSpeed; } }
        public float acceleration { get { return _characterSO.acceleration; } set { acceleration = _characterSO.acceleration; } }
        #endregion

        #region UNITY_METHODS
        void Start()
        {
            initialize();
        }
        void OnEnable()
        {
            EventManager.StartListening(Events.GAME_STARTED, onGameStarted);
        }
        void OnDisable()
        {
            EventManager.StopListening(Events.GAME_STARTED, onGameStarted);
        }
        private void Update()
        {
            if (_canPlay)
            {
                movement();
            }
        }
        #endregion

        #region METHODS
        private void initialize()
        {
            _stackController = GetComponentInChildren<StackController>();
            if (_stackController != null)
                _stackController.initialize();
            _navMeshAgent= GetComponentInChildren<NavMeshAgent>();
            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = acceleration;
            _navMeshAgent.angularSpeed = rotationSpeed;
        }
        private Vector3 getMouseInputDir()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseFirstPos = Input.mousePosition;
                _mouseFirstPos.x /= Screen.width;
                _mouseFirstPos.y /= Screen.height;
                _mouseFirstPos.z = _mouseFirstPos.y;
                _mouseFirstPos.y = 0;
            } 
            else if (Input.GetMouseButton(0))
            {
                _mousePos = Input.mousePosition;
                _mousePos.x /= Screen.width;
                _mousePos.y /= Screen.height;
                _mousePos.z = _mousePos.y;
                _mousePos.y = 0;
                _mouseDir = _mousePos - _mouseFirstPos;
                _mouseFirstPos = _mousePos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _mouseDir = Vector3.zero;
            }
            return _mouseDir;
        }
        private void movement()
        {
            Vector3 tempDir = getMouseInputDir();
            Vector3 movementDir = tempDir * sensitivity;
            if (movementDir.magnitude > .05f)
                _navMeshAgent.SetDestination(transform.position + movementDir);

        }

        #region STACK_METHODS
        public void takeAsset(GameObject asset)
        {
            if (_stackController != null)
                _stackController.addStack(asset);
        }

        public bool canTakeAsset()
        {
            if (_stackController != null)
                return !_stackController.isStackFull();
            return false;
        }

        public GameObject dropAsset()
        {
            if (_stackController == null)
                return null;
            return _stackController.getLastAsset();
        }

        public bool canDropAsset()
        {
            if (_stackController != null)
                return _stackController.isStackHasAsset();
            return false;
        }
        #endregion

        #endregion

        #region ACTIONS
        private void onGameStarted(object[] obj)
        {
            _canPlay = true;
        }
        #endregion
    }
}