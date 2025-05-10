using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    Queue<T> pool = new Queue<T>();
    private T _prefab; 
    private Transform _parent;


    public ObjectPool(T prefab, int size, Transform parent = null)
    {
        this._prefab = prefab;
        this._parent= parent;

        for(int i=0; i<size; i++)
        {
            T obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    
}
