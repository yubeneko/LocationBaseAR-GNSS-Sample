using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MapHightCoordinator : MonoBehaviour
{
    [SerializeField] ARPlaneManager _arPlaneManager;
    private float _mapHeight;

    void Start()
    {
        _arPlaneManager.planesChanged += (ARPlanesChangedEventArgs e) =>
        {
            if (e.added.Count > 0)
            {
                _mapHeight = e.added[e.added.Count - 1].center.y;
            }
        };
    }

    void Update()
    {
        var targetPosition = new Vector3(transform.position.x, _mapHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
    }
}
