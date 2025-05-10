using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMosnter : MonoBehaviour
{
    // projectile 시험해보려고 만든 스크립트입니다.
    // 나중에 지울 스크립트
    private Vector3 _dir;
    private int _damage;

    public void Initialize(Vector3 direction, int damage)
    {
        _dir = direction.normalized;
        _damage = damage;
    }

    private void Update()
    {
        transform.position += _dir * 6f * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어 체력 떨어지게 하는 부분을 여기다가
            Debug.Log("Health 떨어짐");
        }
    }
}
