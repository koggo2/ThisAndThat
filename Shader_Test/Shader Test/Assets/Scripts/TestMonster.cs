using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TestMonster : MonoBehaviour, iDungeonActor
{
    private NavMeshAgent _navAgent;
    private DungeonCommandController dungeonCommandController = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        dungeonCommandController = FindObjectOfType<DungeonCommandController>();
    }

    public IEnumerator DoAct(Action endCallback)
    {
        var direction = (dungeonCommandController.PlayerObj.transform.position - transform.position).normalized;
        _navAgent.Move(direction * 0.1f);

        yield return null;

        endCallback.Invoke();
    }
}
