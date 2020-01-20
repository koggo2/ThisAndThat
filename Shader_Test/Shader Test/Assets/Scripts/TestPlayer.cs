using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TestPlayer : MonoBehaviour, iDungeonActor
{
    private DungeonCommandController dungeonCommandController = null;
    private NavMeshAgent _navAgent;
    private Vector3 _tempDestination;
    
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        dungeonCommandController = FindObjectOfType<DungeonCommandController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButton(0)) {
                RaycastHit hit;
                
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    _tempDestination = hit.point;
                    dungeonCommandController.RequestSeekerMove();
                }
            }
        }
        
        Camera.main.transform.position = new Vector3(transform.position.x, 8f, transform.position.z - 3f);
    }

    public IEnumerator DoAct(Action endCallback)
    {
        _navAgent.Move((_tempDestination - transform.position).normalized * 0.1f);

        yield return null;

        endCallback.Invoke();
    }
}
