using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BaseEnemy<Boss>,IRangedEnemy
{
    [SerializeField] private GameObject _cloneBossPrefab;
    [Header("≈ıªÁ√º Prefabs")]
    [SerializeField] private GameObject _forwardProjectilePrefab;
    [SerializeField] private GameObject _radialProjectilePrefab;
    public GameObject GetClonePrefab()
    {
        return _cloneBossPrefab;
    }
    public GameObject GetProjectilePrefab(string type)
    {
        switch (type)
        {
            case "Forward":
                return _forwardProjectilePrefab;
            case "Radial":
                return _radialProjectilePrefab;
            default:
                return null;
        }
    }
}
