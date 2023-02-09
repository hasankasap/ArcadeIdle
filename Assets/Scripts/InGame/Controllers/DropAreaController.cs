using DG.Tweening;
using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DropAreaController : MonoBehaviour
    {
        [SerializeField] Storage _productStorage;
        [SerializeField] DropAreaSO _dropAreaSO;
        bool _assetTeking = false;
        float _takeDelay => _dropAreaSO.takeDelay;
        ProductTypes productType => _productStorage.getStorageType();

        IEnumerator _takeAssetCoroutine;
        private List<ICharacter> _queCharacters = new List<ICharacter>();

        #region UNITY_METHODS
        #endregion

        #region METHODS
        public Product getLastProduct()
        {
            _productStorage.decrease();
            return _productStorage.getLastProduct();
        }
        public bool hasProduct()
        {
            return  _productStorage.hasProduct();
        }
        private IEnumerator takeProductWithTimer()
        {
            while (_assetTeking)
            {
                yield return new WaitUntil(() => !_productStorage.isStorageFull() && _queCharacters.Count > 0);
                yield return new WaitForSeconds(_takeDelay);
                takeProduct();
                yield return new WaitForFixedUpdate();
            }
        }
        private void takeProduct()
        {
            if (_queCharacters.Count > 0 && _queCharacters[0].canDropWantedProductTypes(productType))
            {
                Product temp = _queCharacters[0].dropProductsWithType(productType);
                Vector3 localAngle = temp.transform.localEulerAngles;
                temp.transform.parent = _productStorage.storagePoint.parent;
                _productStorage.addProduct(temp);
                temp.transform.DOLocalRotate(localAngle, .5f, RotateMode.FastBeyond360);
                Vector3 pos = _productStorage.getStoragePoint();
                temp.transform.DOLocalJump(pos, 2, 1, .5f);
                _productStorage.increase();
            }
            else if (_queCharacters.Count > 0 && !_queCharacters[0].canDropWantedProductTypes(productType))
            {
                _queCharacters.RemoveAt(0);
                if (_queCharacters.Count == 0)
                {
                    startTakeCoroutine(false);
                }
            }
        }
        private void startTakeCoroutine(bool startStatus)
        {
            _assetTeking = startStatus;
            if (startStatus)
            {
                if (_takeAssetCoroutine != null)
                    StopCoroutine(_takeAssetCoroutine);
                _takeAssetCoroutine = takeProductWithTimer();
                StartCoroutine(_takeAssetCoroutine);
            }
            else
            {
                if (_takeAssetCoroutine != null)
                    StopCoroutine(_takeAssetCoroutine);
            }
        }
        #endregion

        #region ACTIONS
        public void onCharacterEnter(Collider characterCollider)
        {
            ICharacter character = characterCollider.GetComponentInChildren<ICharacter>();
            if (character != null)
            {
                if (_queCharacters.Count == 0)
                {
                    startTakeCoroutine(true);
                }
                if (!_queCharacters.Contains(character))
                    _queCharacters.Add(character);
            }
        }
        public void onCharacterExit(Collider characterCollider)
        {
            ICharacter character = characterCollider.GetComponentInChildren<ICharacter>();
            if (_queCharacters.Contains(character))
            {
                _queCharacters.Remove(character);
                if (_queCharacters.Count == 0)
                {
                    startTakeCoroutine(false);
                }
            }
        }
        #endregion
    }
}