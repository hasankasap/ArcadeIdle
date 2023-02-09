using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TrashCan : MonoBehaviour
    {
        [SerializeField] Transform _trashInputPoint;
        [SerializeField] TrashCanSO _trashCanSO;
        private List<ICharacter> _queCharacters = new List<ICharacter>();
        IEnumerator _takeAssetCoroutine;
        bool _assetTeking = false;
        private float takeDelay => _trashCanSO.takeDelay;

        #region UNITY_METHODS
        #endregion

        #region METHODS
        private IEnumerator takeProductWithTimer()
        {
            while (_assetTeking)
            {
                yield return new WaitUntil(() => _queCharacters.Count > 0);
                yield return new WaitForSeconds(takeDelay);
                takeProduct();
                yield return new WaitForFixedUpdate();
            }
        }
        private void takeProduct()
        {
            if (_queCharacters.Count > 0 && _queCharacters[0].canDropProductToTrash())
            {
                Product temp = _queCharacters[0].dropToTrash();
                Vector3 localAngle = temp.transform.localEulerAngles;
                temp.transform.parent = transform;
                temp.transform.DOLocalJump(_trashInputPoint.localPosition, 2, 1, .5f).OnComplete(()=> Destroy(temp.gameObject));
            }
            else if (_queCharacters.Count > 0 && !_queCharacters[0].canDropProductToTrash())
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