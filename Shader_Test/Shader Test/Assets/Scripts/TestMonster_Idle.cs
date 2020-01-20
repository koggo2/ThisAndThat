using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TestMonster_Idle : MonoBehaviour, iDungeonActor
{
    private const float MinimumMovingLength = 4.0f;
    
    private NavMeshAgent _navAgent;
    private DungeonCommandController dungeonCommandController = null;
    private float _currentMovingLength = 0f;
    private Vector3 _currentDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        dungeonCommandController = FindObjectOfType<DungeonCommandController>();
    }

    public IEnumerator DoAct(Action endCallback)
    {
        _currentMovingLength += 0.1f;
        
        if (_currentMovingLength < MinimumMovingLength)
        {
            _navAgent.Move(_currentDirection * 0.1f);
        }
        else
        {
            _currentMovingLength = 0f;
            _currentDirection = Random.insideUnitSphere;
            _navAgent.Move(_currentDirection * 0.1f);
        }

        yield return null;

        endCallback?.Invoke();
    }
}
