using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool _isMouseDown = false;

    private int _currentPathInstanceId = 0;
    private RaycastHit _hit;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isMouseDown = true;
            _currentPathInstanceId = 0;
            
            if (UIEventHandler.IsOnBuildTrack)
            {
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, float.MaxValue))
                    // if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100))
                {
                    // var railPointPrefab = Resources.Load<GameObject>("Rail Point");
                    // var sphere = GameObject.Instantiate(railPointPrefab);
                    // sphere.transform.position = hit.point;

                    _currentPathInstanceId = PathManager.PathCreate(_hit.point);
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0) && _isMouseDown)
        {
            _isMouseDown = false;
            _currentPathInstanceId = 0;
        }

        if (Input.GetMouseButton(0) && _isMouseDown)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, float.MaxValue))
            {
                if (_hit.collider.gameObject.layer != LayerMask.NameToLayer("Plane"))
                {
                    return;
                }
                
                PathManager.MovingPath(_currentPathInstanceId, _hit.point);                
            }
        }
    }
}
