using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : MonoBehaviour, iDungeonActor
{
    private GameObject _triggeredPlayer = null;
    private bool _activated = false;

    void Start()
    {
        _activated = false;
    }
    
    public IEnumerator DoAct(Action endCallback)
    {
        if (_triggeredPlayer != null && !_activated)
        {
            _activated = true;
            yield return TempActionToPlayer();
        }

        endCallback?.Invoke();
    }

    private IEnumerator TempActionToPlayer()
    {
        var dTime = 0f;

        while (dTime < 2f)
        {
            _triggeredPlayer.gameObject.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(2f, 2f, 2f), dTime / 2.0f);
            dTime += Time.deltaTime;
            yield return null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        var player = other.gameObject.GetComponent<TestPlayer>();
        if (player != null)
        {
            _triggeredPlayer = player.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<TestPlayer>();
        if (player != null)
        {
            _triggeredPlayer = null;
        }
    }
}
