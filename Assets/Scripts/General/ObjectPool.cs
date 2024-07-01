using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A flexible object pooling class containing the core create, get, and return methods.
/// </summary>
public class ObjectPool<T> where T : Component
{
    private Queue<T> objectsQueue = new Queue<T>();
    private T prefab;

    public ObjectPool(T prefab, int initialAmount)
    {
        this.prefab = prefab;

        //Create a pool of a given prefab with a given amount when initialized.
        for (int i = 0; i < initialAmount; i++)
        {
            T obj = Object.Instantiate(prefab);
            obj.gameObject.SetActive(false);
            objectsQueue.Enqueue(obj);
        }
    }

    public T Get()
    {
        if (objectsQueue.Count > 0)
        {
            T obj = objectsQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            //Create a new object and return that if the pool is currently empty.
            T obj = Object.Instantiate(prefab);
            return obj;
        }
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objectsQueue.Enqueue(obj);
    }

}
