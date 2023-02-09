using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PickUpAreaController : MonoBehaviour
    {
        private List<ICharacter> _queCharacters = new List<ICharacter>();

        [SerializeField] Storage _storageProperties;
        [SerializeField] PickUpAreaSO _pickUpAreaSO;
        bool _assetsSending = false;
        float _delay => _pickUpAreaSO.sendDelay;
        private int _currentCount = 0;

        IEnumerator _sendAssetCoroutine;

        #region UNITY_METHODS

        #endregion

        #region METHODS
        public Vector3 getStoragePoint()
        {
            Vector3 pos = _storageProperties.getStoragePoint();
            pos = _storageProperties.storagePoint.parent.TransformPoint(pos);
            _storageProperties.increase();
            _currentCount++;
            return pos;
        }
        public void addProduct(Product product)
        {
            _storageProperties.addProduct(product);
        }
        public bool checkCanAddInstantly()
        {
            return  !(_currentCount >= _storageProperties.getCapacity());
        }
        public bool canAdd()
        {
            return !_storageProperties.isStorageFull();
        }
        private void sendProduct()
        {
            if (_queCharacters.Count > 0 && _queCharacters[0].canTakeProducts())
            {
                Product asset = _storageProperties.getLastProduct();
                _storageProperties.decrease();
                _queCharacters[0].takeProducts(asset);
                _currentCount--;
            }
            else if (_queCharacters.Count > 0 && !_queCharacters[0].canTakeProducts())
            {
                _queCharacters.RemoveAt(0);
                if (_queCharacters.Count == 0)
                {
                    startSendingCoroutine(false);
                }
            }
        }
        private IEnumerator sendWithDelay()
        {
            while (_assetsSending)
            {
                yield return new WaitUntil(() => _storageProperties.hasProduct() && _queCharacters.Count > 0);
                yield return new WaitForSeconds(_delay);
                sendProduct();
                yield return new WaitForFixedUpdate();
            }
        }
        private void startSendingCoroutine(bool startStatus)
        {
            _assetsSending = startStatus;
            if (startStatus)
            {
                if (_sendAssetCoroutine != null)
                    StopCoroutine(_sendAssetCoroutine);
                _sendAssetCoroutine = sendWithDelay();
                StartCoroutine(_sendAssetCoroutine);
            }
            else
            {
                if (_sendAssetCoroutine != null)
                    StopCoroutine(_sendAssetCoroutine);
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
                    startSendingCoroutine(true);
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
                    startSendingCoroutine(false);
                }
            }
        }
        #endregion
    }
}