using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserAttack : IAttackStrategy
{
    private PatternUtil.LaserPatternType _pattern;
    private float _warnDuration = 0.5f;
    private float _damage = 30f;
    private float _width = 0.5f;
    private Vector2 _minBounds = new Vector2(-8, -4);
    private Vector2 _maxBounds = new Vector2(8, 4);
    public LaserAttack(PatternUtil.LaserPatternType pattern)
    {
        _pattern = pattern;
    }

    public void ExecuteAttack(IEnemy enemy)
    {
        if (enemy is MonoBehaviour mb)
        {
            BoundsUtil.UpdateBoundsFromRoom(enemy, ref _minBounds, ref _maxBounds);
            Vector3 origin = enemy.GetEnemyPosition().position;

            var dirs = PatternUtil.GetLaserDirections(_pattern, enemy.GetEnemyPosition(), enemy.GetPlayerPosition());

            foreach (var dir in dirs)
            {
                mb.StartCoroutine(FireLaser(origin, dir.normalized));
            }
        }
    }

    private IEnumerator FireLaser(Vector3 origin, Vector3 dir)
    {
        IEnemy enemy = FindEnemyByOrigin(origin);
        enemy?.SetStop(true);

        Vector3 start = BoundsUtil.GetRayToRoomEdge(origin, -dir, _minBounds, _maxBounds);
        Vector3 end = BoundsUtil.GetRayToRoomEdge(origin, dir, _minBounds, _maxBounds);
        float distance = Vector3.Distance(origin, end);

        // 경고선 생성 (얇고 빨간색)
        Color warnColor = new Color(1f, 0f, 0f, 0.3f);
        GameObject warningLine = CreateLaserLine(start, end, _warnDuration, warnColor, _width * 0.5f);

        // 경고 시간 대기
        yield return new WaitForSeconds(_warnDuration);

        // 경고선 제거
        if (warningLine != null) Object.Destroy(warningLine);

        yield return null; 

        // 본 레이저 출력
        CreateLaserLine(start, end, 0.3f, Color.red, _width * 1.5f);

        // 데미지 처리
        DealLaserDamage(origin, dir, distance);

        yield return new WaitForSeconds(0.3f);

        enemy?.SetStop(false);
    }

    private void DealLaserDamage(Vector3 origin, Vector3 dir, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, distance);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlayerStatHandler playerStatHandler))
            {
                playerStatHandler.Health -= _damage;
            }
        }
    }

    private GameObject CreateLaserLine(Vector3 start, Vector3 end, float duration, Color color, float baseWidth)
    {
        GameObject go = new GameObject("LaserLine");
        var lr = go.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;

        lr.widthCurve = new AnimationCurve(
            new Keyframe(0f, baseWidth * 2f),
            new Keyframe(0.5f, baseWidth),
            new Keyframe(1f, baseWidth * 2f)
        );

        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        // 자동 파괴 예약
        Object.Destroy(go, duration + 0.1f);
        return go;
    }
    private IEnemy FindEnemyByOrigin(Vector3 origin)
    {
        Collider2D hit = Physics2D.OverlapPoint(origin);
        if (hit && hit.TryGetComponent(out IEnemy enemy))
            return enemy;
        return null;
    }
}
