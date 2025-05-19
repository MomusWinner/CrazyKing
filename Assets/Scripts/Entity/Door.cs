using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Entity
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform _leftSide;
        [SerializeField] private Transform _rightSide;
        [SerializeField] private Key _key;
        private bool _isOpening;

        private void Update() {
            if (_isOpening) return;
            
            if ((_key.transform.position - transform.position).sqrMagnitude < 15f)
            {
                _isOpening = true;
                StartCoroutine(WaitKey());
            }
        }

        public void Open()
        {
            _leftSide.DORotate(new Vector3(0, 0, 90), 1f).SetEase(Ease.OutBounce);
            _rightSide.DORotate(new Vector3(0, 0, -90), 1f).SetEase(Ease.OutBounce);
            
        }

        private IEnumerator WaitKey()
        {
            yield return _key.MoveToDoor(transform.position);
            Open();
            Destroy(_key.gameObject);
        }
    }
}