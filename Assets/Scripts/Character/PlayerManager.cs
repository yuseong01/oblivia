using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject _player;

    public Transform PlayerTransform()
    {
        if(_player == null )
            _player = GameObject.FindWithTag("Player");
        return _player.transform;
    }
    public GameObject Player => _player;
    protected override void Awake()
    {
        base.Awake();
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");

            if (_player == null)
            {
                Debug.LogWarning("[PlayerManager] Player with tag 'Player' not found.");
            }
            else
            {
                Debug.Log("[PlayerManager] Player found successfully.");
            }
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return _player != null ? _player.transform.position : Vector3.zero;
    }

    public bool HasPlayer => _player != null;
}
