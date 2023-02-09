using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;
using DG.Tweening;
using UnityEditor.Rendering;

namespace Game
{
    public class AssetTransformer : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] TransformerSO _transformerSO;
        [SerializeField] Transform _transformerInputPoint, _transformerOutputPoint;
        DropAreaController _dropAreaController;
        PickUpAreaController _pickUpAreaController;

        IEnumerator _transformCoroutine;
        [SerializeField] Transform _pickUpAreaCenter, _dropAreaCenter;

        #endregion

        #region PROPERTIES
        private GameObject prefab => _transformerSO.transformedPrefab;
        private float transformDelay => _transformerSO.transformDelay;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            initialize();
        }
        #endregion

        #region METHODS
        private void initialize()
        {
            _dropAreaController = GetComponentInChildren<DropAreaController>();
            _pickUpAreaController = GetComponentInChildren<PickUpAreaController>();
            if (_transformCoroutine != null)
                StopCoroutine(_transformCoroutine);
            _transformCoroutine = changeWithTimer();
            StartCoroutine(_transformCoroutine);
        }
        private IEnumerator changeWithTimer()
        {
            while (true) 
            {
                yield return new WaitUntil(() => _dropAreaController.hasProduct() && _pickUpAreaController.checkCanAddInstantly());
                yield return new WaitForSeconds(transformDelay);
                changeProduct();
                yield return new WaitForFixedUpdate();
            }
        }
        private void changeProduct()
        {
            Vector3 spawnPos = _pickUpAreaController.getStoragePoint();
            Product tempProduct = _dropAreaController.getLastProduct();
            tempProduct.transform.DOScale(Vector3.zero, .6f).SetLink(tempProduct.gameObject).SetDelay(.1f);
            tempProduct.transform.DOJump(_transformerInputPoint.position, 2, 1, .5f).OnComplete(()=> 
            {
                spawnTransformedProduct(spawnPos);
                Destroy(tempProduct);
            });
        }
        private void spawnTransformedProduct(Vector3 spawnPos)
        {
            GameObject temp = Instantiate(prefab, _transformerOutputPoint.position, prefab.transform.rotation, _pickUpAreaController.transform);
            Product tempProduct = temp.GetComponent<Product>();
            temp.transform.DOMove(spawnPos, .5f).OnComplete(()=> _pickUpAreaController.addProduct(tempProduct));
        }
        public Transform getDropAreaCenter()
        {
            return _dropAreaCenter;
        }
        public Transform getPicUpAreaCenter()
        {
            return _pickUpAreaCenter;
        }
        #endregion

        #region ACTIONS
        #endregion
    }
}