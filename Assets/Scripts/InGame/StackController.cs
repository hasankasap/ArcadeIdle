using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StackController : MonoBehaviour
    {
        private List<GameObject> _stackedAssets = new List<GameObject>();
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
        public void addStack(GameObject asset)
        {
            if (_stackPoint == null)
                _stackPoint = transform;
            Vector3 localAngles = asset.transform.localEulerAngles;
            asset.transform.parent = _stackPoint;
            Vector3 stackPos = Vector3.zero;
            stackPos.y += _stackedAssets.Count * _stackUpOffset;
            asset.transform.DOLocalRotate(localAngles, .5f, RotateMode.FastBeyond360);
            asset.transform.DOLocalJump(stackPos, 2f, 1, .5f);
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