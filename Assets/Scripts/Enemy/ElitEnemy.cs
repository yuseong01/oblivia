using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElitEnemy : BaseEnemy<ElitEnemy>, IRangedEnemy
{
    [Header("≈ıªÁ√º Prefabs")]
    [SerializeField] private GameObject _forwardProjectilePrefab;
    [SerializeField] private GameObject _radialProjectilePrefab;
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
