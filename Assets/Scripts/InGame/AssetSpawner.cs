using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Game
{
    public class AssetSpawner : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] SpawnerSO _spawnerSO;
        PickUpAreaController _pickupArea;

        IEnumerator _spawnCoroutine;
        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region PROPERTIES
        private float spwantTimer => _spawnerSO.spawnTimer;
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
        private void initialize()
        {
            _pickupArea = GetComponentInChildren<PickUpAreaController>();
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = spawnWithTimer();
            StartCoroutine(_spawnCoroutine);
        }
        private void spawnAsset()
        {
            GameObject temp = Instantiate(prefab, _pickupArea.getStoragePoint(), prefab.transform.rotation, _pickupArea.transform);
            Product tempProduct = temp.GetComponent<Product>();
            _pickupArea.addProduct(tempProduct);
        }
        private IEnumerator spawnWithTimer()
        {
            while (true) 
            {
                yield return new WaitUntil(() => _pickupArea.canAdd());
                spawnAsset();
                yield return new WaitForSeconds(spwantTimer);
                yield return new WaitForFixedUpdate();
            }
        }
        #endregion
        #region ACTIONS
        private void onGameStarted(object[] obj)
        {
            initialize();
        }
        #endregion
    }
}