using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DungeonCommandController : MonoBehaviour
{
    [SerializeField] private Plane _plane;
    [SerializeField] private GameObject _player = null;

    public GameObject PlayerObj => _player;
    public List<iDungeonActor> Actors;

    private int _lock = 0;

    void Awake()
    {
        Actors = new List<iDungeonActor>();
        Actors.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<iDungeonActor>());
    }

    public void RequestSeekerMove()
    {
        if (IsLock())
            return;
        
        BroadcastToActors();
    }

    public void DecreaseLockCount()
    {
        --_lock;
        if (_lock < 0)
            _lock = 0;
    }

    private void BroadcastToActors()
    {
        if (IsLock())
        {
            Debug.Log("Command Locked!");
            return;
        }
        
        foreach (var actor in Actors)
        {
            ++_lock;
            StartCoroutine(actor.DoAct(DecreaseLockCount));
        }
    }

    private bool IsLock()
    {
        return _lock > 0;
    }
}
