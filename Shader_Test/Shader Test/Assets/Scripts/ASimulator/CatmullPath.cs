using System;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

[Serializable]
public class CatmullPath : MonoBehaviour
{
    [SerializeField] private int _resolution;
    [SerializeField] private GameObject _controlPointPrefab;
    
    public CatmullRom Path => _catmullRom;
    public VertexPath VertexPath => _vertexPath;
    
    [SerializeField] private CatmullRom _catmullRom;
    [SerializeField] private VertexPath _vertexPath;
    private List<Transform> _controlPoints;

    private void Awake()
    {
        _controlPoints = new List<Transform>();
    }

    public void CreatePath(Vector3 point)
    {
        for (var i = 0; i < 4; ++i)
        {
            var go = Instantiate(_controlPointPrefab, point, Quaternion.identity);
            go.transform.SetParent(transform);

            _controlPoints.Add(go.transform);
        }
        
        _catmullRom = new CatmullRom(_controlPoints.ToArray(), _resolution, false);
    }

    public void UpdateLastPoint(Vector3 point)
    {
        if (_controlPoints == null)
            return;

        var p0 = _controlPoints[0].position;
        var dx = (point.x - p0.x);
        var dz = (point.z - p0.z);
        
        // _controlPoints[1].SetPositionAndRotation(p0 + new Vector3(dx / 8.0f, point.y, dz * 0.5f), Quaternion.identity);
        // _controlPoints[2].SetPositionAndRotation(p0 + new Vector3(dx * 0.5f, point.y, dz / 8.0f * 7.0f), Quaternion.identity);
        // _controlPoints[3].SetPositionAndRotation(point, Quaternion.identity);
        _controlPoints[1].SetPositionAndRotation(p0 + new Vector3(dx / 3.0f, point.y, dz / 3.0f), Quaternion.identity);
        _controlPoints[2].SetPositionAndRotation(p0 + new Vector3(dx / 3.0f * 2.0f, point.y, dz / 3.0f * 2.0f), Quaternion.identity);
        _controlPoints[3].SetPositionAndRotation(point, Quaternion.identity);
        
        _catmullRom.Update(_controlPoints.ToArray());
        
        GetComponent<RoadMeshRenderer>().TriggerUpdate();
    }
}
