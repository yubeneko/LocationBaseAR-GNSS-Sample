using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;

public class MapRotationCoordinator : MonoBehaviour
{
    private AbstractMap _map;
    private List<Location> _locationBuffer = new List<Location>(20);
    private bool _isComputing = false;

    [SerializeField] private Transform _arCameraTransform;
    [SerializeField] private Transform _mapRotationPivot;

    void Start()
    {
        _map = GetComponent<AbstractMap>();
        _map.OnInitialized += OnMapInitialized;
    }

    private void OnMapInitialized()
    {
        _map.OnUpdated -= OnMapInitialized;

        LocationProviderFactory.Instance.DefaultLocationProvider.OnLocationUpdated += OnLocationUpdated;
    }

    private void OnLocationUpdated(Location location)
    {
        LogWindow.Instance.Log($"Accuracy: {location.Accuracy}m");

        if (!_isComputing) { return; }

        // 精度が悪い場合はバッファに追加しない
        if (location.Accuracy > 20.0f)
        {
            return;
        }

        _locationBuffer.Add(location);
    }

    private void RotateMap(float rotationGap)
    {
        // 地図を回転させるときはARカメラの位置を中心として回転させる
        _map.transform.SetParent(_mapRotationPivot);
        _map.transform.localRotation = Quaternion.Euler(0, rotationGap, 0);
        _map.transform.SetParent(null);
    }

    public void AlignMapRotationMethod1()
    {
        if (_isComputing) { return; }

        StartCoroutine(AlignMapRotationMethodCoroutine());
    }

    IEnumerator AlignMapRotationMethodCoroutine()
    {
        IEnumerator computeRotationGapCoroutine;

        LogWindow.Instance.Log("Start Map Rotation Align with Device Orientation");
        computeRotationGapCoroutine = ComputeRotationGapFromDeviceOrientation();

        yield return StartCoroutine(computeRotationGapCoroutine);

        LogWindow.Instance.Log("Finish Map Rotation Align with Device Orientation");
        if (computeRotationGapCoroutine.Current is float)
        {
            RotateMap(-1.0f * (float)computeRotationGapCoroutine.Current);
        }

        yield return null;
    }

    /// <summary>
    /// デバイスの向きとARカメラの向きから地図の回転量の差を計算する
    /// </summary>
    /// <returns></returns>
    IEnumerator ComputeRotationGapFromDeviceOrientation()
    {
        _isComputing = true;
        _locationBuffer.Clear();

        var waitCount = 0;

        // 比較的精度が高いデータが5つ以上取得できるまで待つ
        while (_locationBuffer.Count < 5)
        {
            yield return new WaitForSeconds(1.0f);
            waitCount++;

            // 15秒待ってもデータが取得できない場合は処理を終了
            if (waitCount > 15)
            {
                _isComputing = false;
                yield break;
            }
        }

        // 平均を取ってデータを多少平滑化する
        var deviceOrientation = _locationBuffer.Select(x => x.DeviceOrientation).Average();
        _isComputing = false;
        // 初期状態で、mapオブジェクトはワールド座標系の+z軸方向を北として生成されることを前提とする
        yield return deviceOrientation - _arCameraTransform.rotation.eulerAngles.y;
    }
}
