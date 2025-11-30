using UnityEngine;
using Random = UnityEngine.Random;

namespace ARDrums.Effects
{
    public class Fragment : MonoBehaviour
    {
        private FragmentMotion _parent;
    
        private Vector3 _direction;
        private float _baseDistance;
        private Vector3 _rotationAxis;
        private Quaternion _baseRotation;
        private float _distance;

        private void Update()
        {
            UpdatePosition();
        }

        public void SetParent(FragmentMotion parent)
        {
            _parent = parent;
            _parent.OnChangeDistance += OnChangeDistance;
        
            Vector3 direction = transform.localPosition;
            _direction = direction.normalized;
            _baseDistance = direction.magnitude;
        
            _baseRotation = transform.localRotation;
            _rotationAxis = Random.onUnitSphere;
        }
    
        private void OnChangeDistance(float distance)
        {   
            _distance = distance;
        }
    
        private void UpdatePosition()
        {
            Vector3 localPosition = _direction * (_baseDistance * (1.0f + _distance));
            Vector3 position = _parent.transform.position + localPosition;
            // perlin noise
            Vector3 noise = new Vector3(
                (Mathf.PerlinNoise(position.y, Time.time*0.1f) - 0.5f ) * _distance,
                (Mathf.PerlinNoise(Time.time*0.1f, position.z) - 0.5f ) * _distance,
                (Mathf.PerlinNoise(Time.time*0.1f, position.x) - 0.5f ) * _distance
            ) * 3.0f;
            transform.localPosition = localPosition + noise;
        
            transform.localRotation = _baseRotation * Quaternion.AngleAxis(120.0f * _distance * _distance, _rotationAxis);
        }
    }
}