using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _data;
    [SerializeField] private LayerMask _targetLayers;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _data.Icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (((1 << collision.gameObject.layer) & _targetLayers) != 0)
        {
            var itemManager = collision.GetComponent<ItemEffectManager>();
            itemManager.AddItem(_data);
            Destroy(this.gameObject);
        }
    }
}
