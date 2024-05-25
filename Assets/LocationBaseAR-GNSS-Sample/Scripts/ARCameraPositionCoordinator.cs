using UnityEngine;

/// <summary>
/// ARカメラオブジェクトの親オブジェクトにアタッチして、ARカメラの位置を調整する
/// </summary>
public class ARCameraPositionCoordinator : MonoBehaviour
{
    [SerializeField] private Transform _gnssPositionTransform;
    [SerializeField] private Transform _arCameraTransform;

    void LateUpdate()
    {
        var targetPosition = transform.position + (_gnssPositionTransform.position - _arCameraTransform.position);
        targetPosition.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
    }
}
