using UnityEngine;

public class ViewChanger : MonoBehaviour
{
    [SerializeField] private Camera _arCamera;
    [SerializeField] private Camera _overheadCamera;

    public void ToggleView()
    {
        var isArCameraActivated = _arCamera.depth > _overheadCamera.depth;

        if (isArCameraActivated)
        {
            _arCamera.depth = 0;
            _overheadCamera.depth = 1;
        }
        else
        {
            _arCamera.depth = 1;
            _overheadCamera.depth = 0;
        }
    }
}
