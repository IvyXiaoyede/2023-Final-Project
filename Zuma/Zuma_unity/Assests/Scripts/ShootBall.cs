using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    public BallType ballType;

    public void Init(BallType type, Sprite sp, Transform tran)
    {
        ballType = type;
        GetComponent<SpriteRenderer>().sprite = sp;
        transform.localPosition = tran.position;
        transform.rotation = tran.rotation;
        gameObject.SetActive(true);
    }
    public void Move()
    {
        transform.localPosition += transform.up * Time.deltaTime * 10;
    }
    /// <summary>
    /// 是否超出边界
    /// </summary>
    /// <returns></returns>
    public bool IsOutOfBounds()
    {
        if (transform.localPosition.x > 3 || transform.localPosition.x < -3 ||
            transform.localPosition.y > 5 || transform.localPosition.y < -5)
            return true;
        return false;
    }
    public bool IsCross(Vector3 targetPos,float dis)
    {
        return Vector3.Distance(transform.position, targetPos) <= dis;
    }
}
