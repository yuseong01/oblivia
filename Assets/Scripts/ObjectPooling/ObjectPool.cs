using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
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

    public T Get()
    {
        if(pool.Count==0)
        {
            T objTemp = Object.Instantiate(_prefab, _parent);
            objTemp.gameObject.SetActive(true);
            return objTemp;
        }
        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        obj.OnSpawned();

        return obj;
    }

    public void Return(T obj)
    {
        obj.OnDespawned();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
