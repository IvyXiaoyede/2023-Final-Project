using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    public GameObject destroyFXPrefab;

    private ObjectPool<GameObject> destroyFXPool;

    private void Awake()
    {
        Instance = this;
        destroyFXPool = new ObjectPool<GameObject>(InstanceObject, 10);
    }
    private GameObject InstanceObject()
    {
        GameObject fx = Instantiate(destroyFXPrefab, transform);
        fx.SetActive(false);
        return fx;
    }
    public void ShowDestroyFX(Vector3 pos)
    {
        GameObject fx = destroyFXPool.GetObject();
        fx.SetActive(true);
        fx.transform.localPosition = pos;

        //延时0.5f执行回收操作
        ScheduleOnce.Start(this, () =>
         {
             fx.SetActive(false);
             destroyFXPool.AddObject(fx);
         }, 0.5f);
    }
}
