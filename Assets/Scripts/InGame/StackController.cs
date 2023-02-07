using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StackController : MonoBehaviour
    {
        private List<GameObject> _stackedAssets = new List<GameObject>();
        private int _capacity = 5;

        #region UNITY_METHODS
        void Start()
        {
            
        }
        void OnEnable()
        {
            
        }
        void OnDisable()
        {
            
        }
        #endregion

        #region METHODS
        public void initialize()
        {

        }
        public void addStack(GameObject asset)
        {
            asset.transform.parent = transform;
            asset.transform.localPosition = Vector3.zero;
            _stackedAssets.Add(asset);
        }
        public bool isStackFull()
        {
            return _stackedAssets.Count >= _capacity;
        }
        public bool isStackHasAsset()
        {
            return _stackedAssets.Count > 0;
        }
        public GameObject getLastAsset()
        {
            GameObject asset = _stackedAssets[_stackedAssets.Count - 1];
            _stackedAssets.Remove(asset);
            return asset;
        }
        #endregion
        #region ACTIONS
        #endregion
    }
}