using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100))
            {
                Debug.Log(hit.collider.gameObject.layer);
                var railPointPrefab = Resources.Load<GameObject>("Rail Point");
                var sphere = GameObject.Instantiate(railPointPrefab);
                sphere.transform.position = hit.point;
            }
        }
    }
}
