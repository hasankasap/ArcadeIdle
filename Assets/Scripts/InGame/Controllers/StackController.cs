using DG.Tweening;
using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class StackController : MonoBehaviour
    {
        private List<Product> _stackedProduct = new List<Product>();
        private int _capacity = 5;
        [SerializeField] Transform _stackPoint;
        [SerializeField] float _stackUpOffset;

        #region UNITY_METHODS
        #endregion

        #region METHODS
        public void initialize(int capacity)
        {
            _capacity= capacity;
        }
        public void addStack(Product product)
        {
            if (_stackPoint == null)
                _stackPoint = transform;
            Vector3 localAngles = product.transform.localEulerAngles;
            product.transform.parent = _stackPoint;
            Vector3 stackPos = Vector3.zero;
            stackPos.y += _stackedProduct.Count * _stackUpOffset;
            product.transform.DOLocalRotate(localAngles, .5f, RotateMode.FastBeyond360);
            product.transform.DOLocalJump(stackPos, 2f, 1, .5f);
            _stackedProduct.Add(product);
        }
        public bool isStackFull()
        {
            return _stackedProduct.Count >= _capacity;
        }
        public bool isStackHasWantedProducts(ProductTypes type)
        {
            return _stackedProduct.Exists(x => x.type == type);
        }
        public Product getLastProductWithType(ProductTypes type)
        {
            Product asset = _stackedProduct.FindLast(x => x.type == type);//_stackedAssets[_stackedAssets.Count - 1];
            _stackedProduct.Remove(asset);
            rePositionStackedProducts();
            return asset;
        }
        private void rePositionStackedProducts()
        {
            for (int i = 0; i < _stackedProduct.Count; i++)
            {
                Vector3 stackPos = Vector3.zero;
                stackPos.y += i * _stackUpOffset;
                _stackedProduct[i].transform.localPosition = stackPos;
            }
        }
        public Product getLastProduct()
        {
            Product product = _stackedProduct[_stackedProduct.Count - 1];
            _stackedProduct.Remove(product);
            return product;
        }
        public bool isStackHasAnyProduct()
        {
            return _stackedProduct.Count > 0;
        }
        #endregion
        #region ACTIONS
        #endregion
    }
}