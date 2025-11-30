using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace ARDrums.Effects
{
    public class FragmentMotion : MonoBehaviour
    {
        private GameObject[] _fragments;
        [SerializeField] private bool _isDebug = false;
        // スライダー
        [FormerlySerializedAs("distance")]
        [Range(0.0f, 100.0f)]
        [SerializeField] float _distance = 0.0f;
        private float _prevDistance = 0.0f;
        private Tweener _tweener;
        
        public Action<float> OnChangeDistance;
        
        void Start()
        {
            _fragments = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject fragment = transform.GetChild(i).gameObject;
                _fragments[i] = fragment;
                Fragment fragmentComponent = fragment.AddComponent<Fragment>();
                fragmentComponent.SetParent(this);
            }

            OnChangeDistance?.Invoke(_distance);
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
            _distance = 3.0f;
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
            _distance = 3.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_prevDistance != _distance)
            {
                OnChangeDistance?.Invoke(_distance);
                _prevDistance = _distance;
            }
#if UNITY_EDITOR
            if (!_isDebug) return;
            if (Input.GetKeyDown(KeyCode.G))
            {
                Gather();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Spread();
            }
#endif
        }
        
        public void SetDistance(float distance)
        {
            _distance = distance;
        }
        
        // 集まる
        public void Gather()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }
            // DOTweenでdistanceを3から0に
            _distance = 3.0f;
            _tweener = DOTween.To(() => _distance, x => _distance = x, 0.0f, 1.0f).SetEase(Ease.OutCubic);
        }
        
        public void GatherTo(float distance)
        {
            float targetDistance = _distance - distance;
            if (targetDistance < 0.0f)
            {
                targetDistance = 0.0f;
            }
            if (_tweener != null)
            {
                _tweener.Kill();
            }
            _tweener = DOTween.To(() => _distance, x => _distance = x, targetDistance, 0.15f).SetEase(Ease.OutCubic);
        }

        public void Spread()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }
            
            _distance = 0.0f;
            _tweener = DOTween.To(() => _distance, x => _distance = x, 3.0f, 1.0f).SetEase(Ease.InCubic);
        }
        
        public void SpreadTo(float distance)
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }

            _tweener = DOTween.To(() => _distance, x => _distance = x, distance, 0.3f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _tweener = DOTween.To(() => _distance, x => _distance = x, 0.0f, 1f)
                        .SetEase(Ease.OutCubic);
                });
        }

        public void SpreadPulse()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }
            _tweener = DOTween.To(() => _distance, x => _distance = x, 1.0f, 0.2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _tweener = DOTween.To(() => _distance, x => _distance = x, 0.0f, 0.1f)
                        .SetEase(Ease.OutCubic);
                });
        }
    }
}