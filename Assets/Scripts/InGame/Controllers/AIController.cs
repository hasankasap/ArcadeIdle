using Game.Utils;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class AIController : Character
    {
        #region PRIVATE_VARIABLES
        List<AssetSpawner> _spawners = new List<AssetSpawner>();
        List<AssetTransformer> _transformers= new List<AssetTransformer>();
        List<TrashCan> _trashCans = new List<TrashCan>();
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
        #endregion

        #region METHODS
        protected override void initialize()
        {
            base.initialize();
            _spawners = FindObjectsOfType<AssetSpawner>().ToList();
            _transformers= FindObjectsOfType<AssetTransformer>().ToList();
            _trashCans = FindObjectsOfType<TrashCan>().ToList();
        }
        protected override void movement()
        {
            base.movement();
        }

        private IEnumerator startMovement()
        {
            while (true) 
            {
                goClosestSpawner();
                yield return new WaitForFixedUpdate();
                yield return new WaitUntil(() => _navMeshAgent.hasPath && _navMeshAgent.remainingDistance < .5f);
                stop();
                yield return new WaitUntil(() => _stackController.isStackFull());
                goDropOnClosestTransformerArea();
                yield return new WaitUntil(() => _navMeshAgent.hasPath && _navMeshAgent.remainingDistance < .5f);
                stop();
                yield return new WaitUntil(() => !_stackController.isStackHasWantedProducts(ProductTypes.Product));
                goPickUpClosestTransformerArea();
                yield return new WaitUntil(() => _navMeshAgent.hasPath && _navMeshAgent.remainingDistance < .5f);
                stop();
                yield return new WaitUntil(() => _stackController.isStackFull());
                goTrashCan();
                yield return new WaitUntil(() => _navMeshAgent.hasPath && _navMeshAgent.remainingDistance < .5f);
                stop();
                yield return new WaitUntil(() => !_stackController.isStackHasWantedProducts(ProductTypes.TransformedProduct));
                yield return new WaitForFixedUpdate();
            }
        }
        private void goClosestSpawner()
        {
            AssetSpawner closest = findClosestSpawner();
            runToTarget(closest.getSpawnAreaCenter().position);
        }
        private void goDropOnClosestTransformerArea()
        {
            AssetTransformer closest = findClosestTransformer();
            runToTarget(closest.getDropAreaCenter().position);
        }
        private void goPickUpClosestTransformerArea()
        {
            AssetTransformer closest = findClosestTransformer();
            runToTarget(closest.getPicUpAreaCenter().position);
        }
        private void goTrashCan()
        {
            TrashCan closest = findClosestTrashCan();
            runToTarget(closest.getTranshCanDepositPoint().position);
        }
        private void runToTarget(Vector3 target)
        {
            target.y = transform.position.y;
            _navMeshAgent.SetDestination(target);
            if (_animator != null)
                _animator.SetBool("Run", true);
        }
        private void stop()
        {
            if (_animator != null)
            {
                _animator.SetBool("Run", false);
            }
        }
        private AssetSpawner findClosestSpawner()
        {
            if (_spawners.Count > 1)
            {
                AssetSpawner closest = _spawners[0];
                float closestDistance = Vector3.Distance(closest.transform.position, transform.position);
                float distance = 0;
                foreach (AssetSpawner spawner in _spawners) 
                {
                    distance = Vector3.Distance(spawner.transform.position, transform.position);
                    if (distance <= closestDistance)
                    {
                        closest = spawner;
                        closestDistance = distance;
                    }
                }
                return closest;
            }
            return _spawners[0];
        }
        private AssetTransformer findClosestTransformer()
        {
            if (_spawners.Count > 1)
            {
                AssetTransformer closest = _transformers[0];
                float closestDistance = Vector3.Distance(closest.transform.position, transform.position);
                float distance = 0;
                foreach (AssetTransformer transformer in _transformers)
                {
                    distance = Vector3.Distance(transformer.transform.position, transform.position);
                    if (distance <= closestDistance)
                    {
                        closest = transformer;
                        closestDistance = distance;
                    }
                }
                return closest;
            }
            return _transformers[0];
        }
        private TrashCan findClosestTrashCan()
        {
            if (_spawners.Count > 1)
            {
                TrashCan closest = _trashCans[0];
                float closestDistance = Vector3.Distance(closest.transform.position, transform.position);
                float distance = 0;
                foreach (TrashCan trashCan in _trashCans)
                {
                    distance = Vector3.Distance(trashCan.transform.position, transform.position);
                    if (distance <= closestDistance)
                    {
                        closest = trashCan;
                        closestDistance = distance;
                    }
                }
                return closest;
            }
            return _trashCans[0];
        }

        #endregion

        #region ACTIONS
        protected override void onGameStarted(object[] obj)
        {
            base.onGameStarted(obj);
            StartCoroutine(startMovement());
        }
        #endregion
    }
}