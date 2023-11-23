using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooter : MonoBehaviour
{
    public Shooter Instance;
    private SpriteRenderer shootBall;
    private BallType curBallType;

   // public SerialController serialController;
    private void Awake()
    {
       Instance = this;
  
    }
    private void Start()
    {
      //  serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        shootBall = transform.Find("Ball").GetComponent<SpriteRenderer>();
        RefrashBallType();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//后续变成螺旋仪旋转角度
            float y = mousePos.y - transform.position.y;
            float x = mousePos.x - transform.position.x;
            //弧度角 弧度转度
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle - 90);
        }
        if (Input.GetMouseButtonDown(0))
        {
            //发射一个球Input.GetMouseButtonUp(0)&& 
            Shoot();
        }
    }
    /// <summary>
    /// 刷新球的类型
    /// </summary>
    private void RefrashBallType()
    {
        curBallType = GameStrategy.SpawnShootBallStrategy();
        shootBall.sprite = GameManager.Instance.GetSpriteByBallType(curBallType);
        shootBall.gameObject.SetActive(true);
    }
    public void Shoot()
    {
        if (shootBall.gameObject.activeSelf == false) return;
        SoundManager.PlayShoot();
        ShootBallManager.Instance.Shoot(curBallType, shootBall.sprite, transform);
        shootBall.gameObject.SetActive(false);

        ScheduleOnce.Start(this, RefrashBallType, 0.2f);
    }
}
