using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviour , ICharacter
    {

        #region PRIVATE_VARIABLES
        [SerializeField] protected CharacterSO _characterSO;
        protected StackController _stackController;
        protected NavMeshAgent _navMeshAgent;
        protected Animator _animator;

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
        protected virtual void Start()
        {
            initialize();
        }
        protected virtual void OnEnable()
        {
            EventManager.StartListening(Events.GAME_STARTED, onGameStarted);
        }
        protected virtual void OnDisable()
        {
            EventManager.StopListening(Events.GAME_STARTED, onGameStarted);
        }
        #endregion

        #region METHODS
        protected virtual void initialize()
        {
            _stackController = GetComponentInChildren<StackController>();
            if (_stackController != null)
                _stackController.initialize(_characterSO.stackCapacity);

            _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            _navMeshAgent.speed = speed;
            _navMeshAgent.acceleration = acceleration;
            _navMeshAgent.angularSpeed = rotationSpeed;

            _animator = GetComponentInChildren<Animator>();
        }
        protected virtual void movement()
        {
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
        protected virtual void onGameStarted(object[] obj)
        {

        }
        #endregion
    }
}