using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField] private float speed = 90f; // degrees per second
    [SerializeField] private Space rotationSpace = Space.Self;

    // Update is called once per frame
    void Update()
    {
        if (axis.sqrMagnitude <= 0f) return;
        transform.Rotate(axis.normalized, speed * Time.deltaTime, rotationSpace);
    }
}
