using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;
using DG.Tweening;

namespace Game
{
    public class AssetTransformer : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] TransformerSO _transformerSO;
        [SerializeField] StorageProperties _assetStorageProperties, _transformedStorageProperties;

        private List<GameObject> _storedAssets = new List<GameObject>();
        private List<ICharacter> _queCharacters = new List<ICharacter>();

        int _lineCount = 0, _columnCount = 0;
        bool _assetTeking = false;

        IEnumerator _transformCoroutine, _takeAssetCoroutine;

        #endregion

        #region PROPERTIES
        private GameObject prefab => _transformerSO.transformedPrefab;
        private int capacity => _transformerSO.capacity;
        private float transformDelay => _transformerSO.transformDelay;
        private float assetTakeDelay => _transformerSO.assetTakeDelay;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            if (_transformCoroutine != null)
                StopCoroutine(_transformCoroutine);
            _transformCoroutine = transformWithTimer();
            StartCoroutine(_transformCoroutine);
        }
        #endregion

        #region METHODS
        private IEnumerator transformWithTimer()
        {
            while (true) 
            {
                yield return new WaitUntil(() => _storedAssets.Count > 0);
                yield return new WaitForSeconds(transformDelay);
                transformAsset();
                yield return new WaitForFixedUpdate();
            }
        }
        private void transformAsset()
        {
            _assetStorageProperties.columnCount--;
            if (_assetStorageProperties.columnCount < 0)
            {
                _assetStorageProperties.lineCount--;
                _assetStorageProperties.columnCount = _assetStorageProperties.storageLineCapacity;
            }
        }
        private IEnumerator takeAssetWithTimer()
        {
            while (_assetTeking) 
            {
                yield return new WaitUntil(() => _storedAssets.Count < capacity && _queCharacters.Count > 0);
                yield return new WaitForSeconds(assetTakeDelay);
                takeAsset();
                yield return new WaitForFixedUpdate();
            }
        }
        private void takeAsset()
        {
            if (_queCharacters.Count > 0 && _queCharacters[0].canDropAsset())
            {
                GameObject temp = _queCharacters[0].dropAsset();
                Vector3 localAngle = temp.transform.localEulerAngles;
                temp.transform.parent = transform;
                _storedAssets.Add(temp);
                temp.transform.DOLocalRotate(localAngle, .5f, RotateMode.FastBeyond360);
                temp.transform.DOLocalJump(GameUtils.getStoragePoint(_assetStorageProperties), 2, 1, .5f);

                _columnCount++;
                if (_columnCount >= _assetStorageProperties.storageLineCapacity)
                {
                    _columnCount = 0;
                    _lineCount++;
                }
            }
            else if (_queCharacters.Count > 0 && !_queCharacters[0].canDropAsset())
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
                _takeAssetCoroutine = takeAssetWithTimer();
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