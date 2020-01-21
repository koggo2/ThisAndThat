using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public CatmullPath pathPrefab;

    private Dictionary<int, CatmullPath> _pathMap;

    private void Awake()
    {
        _pathMap = new Dictionary<int, CatmullPath>();
    }

    private void Update()
    {
        if(_pathMap != null && _pathMap.Count > 0)
        {
            foreach (var path in _pathMap.Values)
            {
                path.Path.DrawSpline(Color.yellow);
            }
        }
    }

    public static int PathCreate(Vector3 point)
    {
        var pathObject = Instantiate (Instance.pathPrefab, point, Quaternion.identity);
        pathObject.transform.parent = Instance.transform;

        var pathInstance = pathObject.GetComponent<CatmullPath>();
        pathInstance.CreatePath(point);
        
        Instance._pathMap.Add(pathInstance.GetInstanceID(), pathInstance);
        
        return pathInstance.GetInstanceID();
    }

    public static void MovingPath(int instanceId, Vector3 point)
    {
        if (!Instance._pathMap.ContainsKey(instanceId))
        {
            return;
        }

        var path = Instance._pathMap[instanceId];
        if (path == null)
            return;

        path.UpdateLastPoint(point);
    }
}
