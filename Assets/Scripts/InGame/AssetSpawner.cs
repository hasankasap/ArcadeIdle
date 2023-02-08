using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AssetSpawner : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] SpawnerSO _spawnerSO;
        [Space(3)]
        [SerializeField] Storage _storageProperties;

        private List<GameObject> _spawnedObjects = new List<GameObject>();

        private List<ICharacter> _queCharacters = new List<ICharacter>();
        bool _assetsSending = false;

        IEnumerator _spawnCoroutine, _sendAssetCoroutine;
        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region PROPERTIES
        private float spwantTimer => _spawnerSO.spawnTimer;
        private float sendDelay => _spawnerSO.sendDelay;
        private int capacity => _spawnerSO.spawnerCapacity;
        private GameObject prefab => _spawnerSO.spawnPrefab;
        #endregion

        #region UNITY_METHODS
        void OnEnable()
        {
            EventManager.StartListening(Events.GAME_STARTED, onGameStarted);
        }
        void OnDisable()
        {
            EventManager.StopListening(Events.GAME_STARTED, onGameStarted);
        }
        #endregion

        #region METHODS
        private void spawnAsset()
        {
            GameObject temp = Instantiate(prefab, GameUtils.getStoragePoint(_storageProperties), prefab.transform.rotation, transform);
            _spawnedObjects.Add(temp);
            _storageProperties.columnCount++;
            if (_storageProperties.columnCount >= _storageProperties.storageProp.storageLineCapacity)
            {
                _storageProperties.columnCount = 0;
                _storageProperties.lineCount++;
            }
        }
        private IEnumerator spawnWithTimer()
        {
            _storageProperties.columnCount = 0;
            _storageProperties.lineCount = 0;
            while (true) 
            {
                yield return new WaitUntil(() => _spawnedObjects.Count < capacity);
                spawnAsset();
                yield return new WaitForSeconds(spwantTimer);
                yield return new WaitForFixedUpdate();
            }
        }
        private void sendProduct()
        {
            if (_queCharacters.Count > 0 && _queCharacters[0].canTakeAsset())
            {
                GameObject asset = _spawnedObjects[_spawnedObjects.Count - 1];
                _spawnedObjects.Remove(asset);
                _storageProperties.columnCount--;
                if (_storageProperties.columnCount < 0) 
                {
                    _storageProperties.lineCount--;
                    _storageProperties.columnCount = _storageProperties.storageProp.storageLineCapacity;
                }
                _queCharacters[0].takeAsset(asset);
            }
            else if (_queCharacters.Count > 0 && !_queCharacters[0].canTakeAsset())
            {
                _queCharacters.RemoveAt(0);
                if (_queCharacters.Count == 0)
                {
                    startSendingCoroutine(false);
                    _assetsSending = false;
                }
            }
        }
        private IEnumerator sendWithDelay()
        {
            _assetsSending = true;
            while (_assetsSending)
            {
                yield return new WaitUntil(() => _spawnedObjects.Count > 0 && _queCharacters.Count > 0);
                yield return new WaitForSeconds(sendDelay);
                sendProduct();
                yield return new WaitForFixedUpdate();
            }
        }
        private void startSendingCoroutine(bool startStatus)
        {
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
        private void onGameStarted(object[] obj)
        {
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = spawnWithTimer();
            StartCoroutine(_spawnCoroutine);
        }
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
                    _assetsSending = false;
                }
            }        
        }
        #endregion
    }
}