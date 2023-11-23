using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏策略类
/// </summary>
public class GameStrategy : MonoBehaviour
{
    static int typeNumber = 0;
    static int ballType;

    public static BallType SpawnBallStrategy()
    {
        if (typeNumber <= 0)
        {
            //2
            typeNumber = Random.Range(1, 3);
            ballType = Random.Range(0, 3);
        }
        typeNumber--;
        return (BallType)ballType;
    }
    public static BallType SpawnShootBallStrategy()
    {
        return (BallType)Random.Range(0, 4);
    }
    public static int BombDestroyCount = 5;
    public static int SpawnBallCount(int sceneIndex)
    {
        return GameData.LevelIndex * 10 + 50;
    }
}
