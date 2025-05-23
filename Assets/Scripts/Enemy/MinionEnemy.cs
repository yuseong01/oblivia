using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IEnemy;

public class MinionEnemy : BaseEnemy<MinionEnemy>
{
    [SerializeField] private float orbitRadius = 1f;
    [SerializeField] private float orbitSpeed = 180f; // degrees per second
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private Transform player;
    private Vector2 centerFollow;
    private float angle;
    public void SetType(EnemyType type)
    {
        _type = type;
        Debug.Log($"[SetType] {_type}");
    }
    protected override void Update()
    {
        base.Update();
        MoveMinion();

    }
    public void SetInitialAngle(float initialAngle)
    {
        angle = initialAngle;
    }
    public void SetSpeedMinon(float speed)
    {
        followSpeed = speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerStatHandler playerStatHandler = other.GetComponent<PlayerStatHandler>();
            if (playerStatHandler != null)
            {
                playerStatHandler.Health = -GetAttackPower();
            }
        }
    }

    public void MoveMinion()
    {
        if (player == null)
        {
            player = PlayerManager.Instance.PlayerTransform();
            if (player == null) return;
            centerFollow = player.position;
        }
        centerFollow = Vector2.Lerp(centerFollow, player.position, followSpeed * Time.deltaTime);

        angle += Random.Range(0.1f,3f) * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        Vector2 orbitPosition = centerFollow + offset;

        transform.position = orbitPosition;
    }
}
