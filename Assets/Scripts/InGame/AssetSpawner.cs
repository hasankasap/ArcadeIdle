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
        [SerializeField, MyBox.Foldout("Spawned Stack")] Transform _spawnPoint;
        [SerializeField, MyBox.Foldout("Spawned Stack")] int _stackLineCapacity;
        [SerializeField, MyBox.Foldout("Spawned Stack")] float _spawnFrontOffset, _spawnUpOffset;
        int _lineCount = 0, _columnCount = 0;

        private List<GameObject> _spawnedObjects = new List<GameObject>();

        private List<ICharacter> _queCharacters = new List<ICharacter>();

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
        void Start()
        {
            
        }
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
            GameObject temp = Instantiate(prefab, getSpawnPoint(), prefab.transform.rotation, transform);
            _columnCount++;
            if (_columnCount >= _stackLineCapacity)
            {
                _columnCount = 0;
                _lineCount++;
            }
            _spawnedObjects.Add(temp);
        }
        private Vector3 getSpawnPoint()
        {
            Vector3 point = _spawnPoint.position;
            Vector3 forwardOffset = _spawnPoint.forward * _spawnFrontOffset * _columnCount;
            Vector3 upwardOffset = _spawnPoint.up * _spawnUpOffset * _lineCount; 
            point += (forwardOffset + upwardOffset);
            return point;
        }
        private IEnumerator spawnWithTimer()
        {
            _columnCount = 0;
            _lineCount = 0;
            while (true) 
            {
                yield return new WaitUntil(() => _spawnedObjects.Count < capacity);
                spawnAsset();
                yield return new WaitForSeconds(spwantTimer);
                yield return new WaitForFixedUpdate();
            }
        }
        private void sendAsset()
        {
            GameObject asset = _spawnedObjects[_spawnedObjects.Count - 1];
            _spawnedObjects.Remove(asset);
            if (_queCharacters.Count > 0 && _queCharacters[0].canTakeAsset())
                _queCharacters[0].takeAsset(asset);
        }
        private IEnumerator sendWithDelay()
        {
            while (true)
            {
                yield return new WaitUntil(() => _spawnedObjects.Count > 0 && _queCharacters.Count > 0);
                yield return new WaitForSeconds(sendDelay);
                sendAsset();
                yield return new WaitForFixedUpdate();
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
            if (!_queCharacters.Contains(character))
                _queCharacters.Add(character);
            if (_sendAssetCoroutine != null && _queCharacters.Count == 0)
            {
                StopCoroutine(_sendAssetCoroutine);
                _sendAssetCoroutine = sendWithDelay();
                StartCoroutine(_sendAssetCoroutine);
            }
                
            
        }
        public void onCharacterExit(Collider characterCollider)
        {
            ICharacter character = characterCollider.GetComponentInChildren<ICharacter>();
            if (_queCharacters.Contains(character))
                _queCharacters.Remove(character);
        }
        #endregion
    }
}