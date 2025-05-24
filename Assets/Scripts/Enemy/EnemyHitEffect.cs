using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitEffect : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _isHit;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayEffect()
    {
        if (!_isHit)
            StartCoroutine(HitColor(_spriteRenderer));
    }

    private IEnumerator HitColor(SpriteRenderer spriteRenderer)
    {
        _isHit = true;
        Color original = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = original;
        _isHit = false;
    }
}
