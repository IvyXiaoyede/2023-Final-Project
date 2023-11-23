using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T>
{
    /// <summary>
    /// 计数器
    /// </summary>
    public int counter = 0;

    private List<T> pool = new List<T>();
    private Func<T> func;

    public ObjectPool(Func<T> func, int count)
    {
        this.func = func;
        InstanceObject(count);
    }
    /// <summary>
    /// 从对象池里面取东西
    /// </summary>
    /// <returns></returns>
    public T GetObject()
    {
        int i = pool.Count;
        while (i-- > 0)
        {
            T t = pool[i];
            pool.RemoveAt(i);
            return t;
        }

        InstanceObject(3);
        return GetObject();
    }
    public void AddObject(T t)
    {
        pool.Add(t);
    }
    /// <summary>
    /// 实例化对象
    /// </summary>
    /// <param name="count"></param>
    private void InstanceObject(int count)
    {
        for (int i = 0; i < count; i++)
        {
            pool.Add(func());
        }
    }
}
