using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    void Update()
    {
        var targetPosition = _target.position + _offset;
        transform.position = targetPosition;
    }
}
