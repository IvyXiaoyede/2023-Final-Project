using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBallManager : MonoBehaviour
{
    public static ShootBallManager Instance;
    public GameObject ballPrefab;

    private ObjectPool<ShootBall> pool;
    /// <summary>
    /// 存放当前发射的球集合
    /// </summary>
    public List<ShootBall> shootBallList = new List<ShootBall>();

    private void Awake()
    {
        Instance = this;
        pool = new ObjectPool<ShootBall>(InstanceObject, 3);
    }
    private ShootBall InstanceObject()
    {
        GameObject ball = Instantiate(ballPrefab, transform);
        ball.SetActive(false);
        ShootBall shootBall = ball.AddComponent<ShootBall>();
        return shootBall;
    }
    private void Update()
    {
        for (int i = shootBallList.Count - 1; i >= 0; i--)
        {
            shootBallList[i].Move();
            if (shootBallList[i].IsOutOfBounds())
            {
                Recovery(shootBallList[i]);
                shootBallList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 回收到对象池
    /// </summary>
    /// <param name="ball"></param>
    public void Recovery(ShootBall ball)
    {
        ball.gameObject.SetActive(false);
        pool.AddObject(ball);
    }
    /// <summary>
    /// 发射一个球
    /// </summary>
    /// <param name="type"></param>
    public void Shoot(BallType type, Sprite sp, Transform tran)
    {
        ShootBall shootBall = pool.GetObject();
        shootBall.Init(type, sp, tran);
        shootBallList.Add(shootBall);
    }
}
