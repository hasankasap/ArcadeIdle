using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
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
        FloatingJoystick _joystick;
        Animator _animator;

        private float _speedAddValue = 0;
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
        private void initialize()
        {
            _stackController = GetComponentInChildren<StackController>();
            if (_stackController != null)
                _stackController.initialize(_characterSO.stackCapacity);

            _navMeshAgent= GetComponentInChildren<NavMeshAgent>();
            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = acceleration;
            _navMeshAgent.angularSpeed = rotationSpeed;

            _joystick = FindObjectOfType<FloatingJoystick>();
            _animator = GetComponentInChildren<Animator>();
        }
        private void movement()
        {
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

        #region STACK_METHODS
        public void takeProducts(Product asset)
        {
            if (_stackController != null)
                _stackController.addStack(asset);
        }

        public bool canTakeProducts()
        {
            if (_stackController != null)
                return !_stackController.isStackFull();
            return false;
        }

        public Product dropProductsWithType(ProductTypes type)
        {
            if (_stackController == null)
                return null;
            return _stackController.getLastProductWithType(type);
        }

        public bool canDropWantedProductTypes(ProductTypes type)
        {
            if (_stackController != null)
                return _stackController.isStackHasWantedProducts(type);
            return false;
        }
        public Product dropToTrash()
        {
            return _stackController.getLastProduct();
        }

        public bool canDropProductToTrash()
        {
            if (_stackController != null)
                return _stackController.isStackHasAnyProduct();
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