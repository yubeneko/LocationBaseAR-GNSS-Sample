using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class ARObjectsManager : MonoBehaviour
{
    [SerializeField] private AbstractMap _map;
    [SerializeField] private GameObject _arObject;
    // 東京駅
    private Vector2d _latLon = new Vector2d(35.681497975322095, 139.76539705139803);

    void Start()
    {
        _map.OnInitialized += OnMapInitialized;
    }

    private void OnMapInitialized()
    {
        _map.OnInitialized -= OnMapInitialized;

        var go = Instantiate(
            _arObject,
            _map.GeoToWorldPosition(_latLon) + new Vector3(0, 1.0f, 0),
            Quaternion.identity
        );

        go.transform.SetParent(_map.gameObject.transform);
        go.layer = 6;
    }
}
