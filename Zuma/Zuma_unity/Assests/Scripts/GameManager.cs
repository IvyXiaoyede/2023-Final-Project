using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BallType
{
    Red,
    Blue,
    Green,
    Bomb,
}
[System.Serializable]
public class BallTypeSprite
{
    public BallType ballType;
    public Sprite sprite;
}

public enum GameState//枚举球的类型
{
    Game,
    Fail,
    Succ
}
public enum MoveState
{
    Forword,
    Back
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public MapConfig mapConfig;
    public List<BallTypeSprite> ballTypeSpriteList = new List<BallTypeSprite>();
    public GameObject ballPrefab;
    public float moveSpeed = 2f;
    public ObjectPool<Ball> ballPool;

    /// <summary>
    /// 球队 段 集合，存放的是首位球
    /// </summary>
    private List<Ball> ballSegmentList = new List<Ball>();
    private Dictionary<BallType, Sprite> ballTypeSpriteDic;
    public Sprite GetSpriteByBallType(BallType type)
    {
        if (ballTypeSpriteDic.ContainsKey(type) == false) return null;
        return ballTypeSpriteDic[type];
    }
    /// <summary>
    /// 新插入的球标记为搜索消除集合
    /// </summary>
    private List<Ball> searchDestroyList = new List<Ball>();
    /// <summary>
    /// 需要回退的球集合
    /// </summary>
    private List<Ball> fallbackList = new List<Ball>();
    private bool isBomb = false;
    private GameState gameState = GameState.Game;
    private MoveState moveState = MoveState.Forword;

    private void Awake()
    {
        ballTypeSpriteDic = new Dictionary<BallType, Sprite>();
        foreach (var item in ballTypeSpriteList)
        {
            ballTypeSpriteDic.Add(item.ballType, item.sprite);
        }
        Instance = this;
    }
    private IEnumerator Start()
    {
        mapConfig.InitMapConfig();
        ballPool = new ObjectPool<Ball>(InstanceBallFunc, 10);

        AudioSource bgAudio = gameObject.AddComponent<AudioSource>();
        bgAudio.clip = SoundManager.GetAudioClip("Bg");
        bgAudio.volume = 0.3f;
        bgAudio.loop = true;
        bgAudio.Play();

        moveSpeed = 0.5f;
        SoundManager.PlayFastMove();
        yield return new WaitForSeconds(1f);
        moveSpeed = 0.5f;
    }
    private Ball InstanceBallFunc()
    {
        GameObject ball = Instantiate(ballPrefab, transform.Find("BallPool"));
        ball.SetActive(false);
        Ball ballScript = ball.AddComponent<Ball>();
        ballScript.SpawnInit();
        return ballScript;
    }

    private void Update()
    {
        if (gameState == GameState.Game)
        {
            if (moveState == MoveState.Forword)
            {
                FirstSegmentMove();
                CheckGameFail();
            }
            else if (moveState == MoveState.Back)
            {
                SegmentBack();
            }
            ShootBallInsert();
            SearchDestroy();
            CheckFallbackBall();
            BallSegmentConnect();
        }
    }
    /// <summary>
    /// 开始回退
    /// </summary>
    public void StartBack()
    {
        gameState = GameState.Game;
        moveState = MoveState.Back;

        ScheduleOnce.Start(this, () =>
        {
            moveState = MoveState.Forword;
        }, 3f);
    }
    /// <summary>
    /// 第一段移动
    /// </summary>
    private void FirstSegmentMove()
    {
        int spawnCount = GameStrategy.SpawnBallCount(SceneManager.GetActiveScene().buildIndex);
        //场景中一段球都没有，先初始化一段
        if (ballPool.counter < spawnCount && ballSegmentList.Count == 0)
        {
            ballPool.counter++;
            Ball ball = ballPool.GetObject().Init(this, GameStrategy.SpawnBallStrategy());
            ballSegmentList.Add(ball);
            return;
        }

        if (ballSegmentList.Count <= 0)
        {
            gameState = GameState.Succ;
            GameUI.Instance.ShowSuccPanel();
            return;
        }

        Ball fb = ballSegmentList[0];
        //如果第一段第一个球已经出了洞口，填充新的球在洞口
        if (ballPool.counter < spawnCount && fb.IsExitStartHole())
        {
            ballPool.counter++;
            Ball ball = ballPool.GetObject().Init(this, GameStrategy.SpawnBallStrategy());
            ball.Next = fb;
            fb.Pre = ball;

            ballSegmentList[0] = ball;
            fb = ball;
        }
        fb.progress += Time.deltaTime * moveSpeed;

        while (fb.Next != null)
        {
            if (fb.Next.progress < fb.progress + 1)
            {
                fb.Next.progress = fb.progress + 1;
            }

            fb = fb.Next;
        }
    }
    /// <summary>
    /// 检查游戏结束
    /// 最后一段的最后一个球是否到达终点
    /// </summary>
    private void CheckGameFail()
    {
        int count = ballSegmentList.Count;
        if (count == 0) return;
        Ball fb = ballSegmentList[count - 1];
        if (fb.Tail.IsArriveFailHole())
        {
            SoundManager.PlayFail();
            gameState = GameState.Fail;
            GameUI.Instance.ShowOverPanel();
        }
    }
    /// <summary>
    /// 发射球的插入
    /// </summary>
    private void ShootBallInsert()
    {
        float distance = 0.3f;

        List<ShootBall> shootBallList = ShootBallManager.Instance.shootBallList;
        int i = shootBallList.Count;
        while (i-- > 0)
        {
            bool isHit = false;
            ShootBall shootBall = shootBallList[i];

            int j = ballSegmentList.Count;
            while (j-- > 0)
            {
                Ball fb = ballSegmentList[j];
                do
                {
                    if (shootBall.IsCross(fb.transform.position, distance))
                    {
                        //代表找到了距离射击球最近的点
                        if (shootBall.ballType != BallType.Bomb)
                        {
                            Ball insert = ballPool.GetObject().Init(this, shootBall.ballType);
                            Ball next = fb.Next;
                            fb.Next = insert;
                            insert.Pre = fb;
                            insert.Next = next;
                            if (next != null) next.Pre = insert;
                            insert.progress = fb.progress + 1;
                            searchDestroyList.Add(insert);
                            isHit = true;
                        }
                        else//是炸弹球
                        {
                            fb.DeleteFlag = true;
                            Ball ball = fb.Pre;
                            int count = GameStrategy.BombDestroyCount / 2;
                            while (ball != null && count-- > 0)
                            {
                                ball.DeleteFlag = true;
                                ball = ball.Pre;
                            }
                            count = GameStrategy.BombDestroyCount / 2;
                            ball = fb.Next;
                            while (ball != null && count-- > 0)
                            {
                                ball.DeleteFlag = true;
                                ball = ball.Next;
                            }
                            isBomb = true;
                            SoundManager.PlayBomb();
                        }

                        shootBallList.RemoveAt(i);
                        ShootBallManager.Instance.Recovery(shootBall);
                        break;
                    }
                    fb = fb.Next;
                } while (fb != null);

                if (isHit)
                {
                    UpdateBallProgress(ballSegmentList[j]);
                    break;
                }
                if (isBomb) break;
            }
        }
    }
    private void SearchDestroy()
    {
        bool isSame = false;
        int i = searchDestroyList.Count;
        while (i-- > 0)
        {
            List<Ball> list;
            Ball ball = searchDestroyList[i];
            if (ball.SameColorCount(out list) >= 3)
            {
                isSame = true;
                foreach (var item in list)
                {
                    item.DeleteFlag = true;
                }
                SoundManager.PlayDestroy();
            }
            else
            {
                SoundManager.PlayInsert();
            }
        }
        searchDestroyList.Clear();

        if (isSame == false && isBomb == false) return;
        isBomb = false;

        int x = ballSegmentList.Count;
        while (x-- > 0)
        {
            Ball fb = ballSegmentList[x];
            Ball head = fb.Head;
            Ball tail = fb.Tail;
            bool isDelete = false;

            do
            {
                if (fb.DeleteFlag)
                {
                    isDelete = true;
                    //切断链接
                    if (fb.Pre != null) fb.Pre.Next = null;
                    if (fb.Next != null) fb.Next.Pre = null;

                    //如果当前这个fb是头部球，就代表头部球被销毁了，就把head置为空
                    if (fb == head) head = null;
                    if (tail == fb) tail = null;

                    FXManager.Instance.ShowDestroyFX(fb.transform.position);
                    fb.Recovery();
                }
                fb = fb.Next;
            } while (fb != null);

            if (isDelete == false) continue;

            //处理分裂
            if (head != null)
            {
                ballSegmentList[x] = head;
                if (tail != null && head != tail.Head)
                {
                    ballSegmentList.Insert(x + 1, tail.Head);
                }
            }
            else
            {
                if (tail != null)
                {
                    ballSegmentList[x] = tail.Head;
                }
                else
                {
                    ballSegmentList.RemoveAt(x);
                }
            }

            //回退
            Ball target = null;
            if (head != null) target = head.Tail;
            else if (x > 0) target = ballSegmentList[x - 1].Tail;
            if (target != null)
            {
                Ball p = null;
                if (tail != null) p = tail.Head;
                else if (x + 1 <= ballSegmentList.Count - 1) p = ballSegmentList[x + 1];

                if (p != null && p.ballType == target.ballType)
                {
                    p.FallbackTarget = target;
                    fallbackList.Add(p);
                }
            }
        }
    }
    /// <summary>
    /// 处理检测有回退趋势的球
    /// </summary>
    private void CheckFallbackBall()
    {
        int i = fallbackList.Count;
        while (i-- > 0)
        {
            Ball ball = fallbackList[i];
            if (ball.gameObject.activeSelf == false || ball.FallbackTarget.gameObject.activeSelf == false)
            {
                fallbackList.RemoveAt(i);
                continue;
            }
            ball.progress -= Time.deltaTime * 15;
            UpdateBallProgress(ball);
            //代表回退到FallbackTarget位置了
            if (ball.progress <= ball.FallbackTarget.progress + 1)
            {
                searchDestroyList.Add(ball);
                fallbackList.RemoveAt(i);
            }
        }
    }
    private void UpdateBallProgress(Ball ball)
    {
        while (ball != null)
        {
            if (ball.Next != null)
                ball.Next.progress = ball.progress + 1;
            ball = ball.Next;
        }
    }
    /// <summary>
    /// 处理球段之间的链接
    /// </summary>
    private void BallSegmentConnect()
    {
        int i = ballSegmentList.Count;
        while (i-- > 1)
        {
            Ball nextSeg = ballSegmentList[i];
            Ball preSeg = ballSegmentList[i - 1];
            Ball tail = preSeg.Tail;
            if (tail.progress >= nextSeg.progress - 1)
            {
                nextSeg.progress = tail.progress + 1;
                UpdateBallProgress(nextSeg);
                tail.Next = nextSeg;
                nextSeg.Pre = tail;
                ballSegmentList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 队伍返回
    /// </summary>
    private void SegmentBack()
    {
        int i = ballSegmentList.Count;
        if (i <= 0) return;
        Ball p = ballSegmentList[i - 1].Tail;
        p.progress -= Time.deltaTime * 16;
        while (p.Pre != null)
        {
            if (p.Pre.progress > p.progress - 1)
                p.Pre.progress = p.progress - 1;
            p = p.Pre;

            //退出出洞口
            if (p.progress < 0)
            {
                ballSegmentList[i - 1] = p.Next;
                p.Next.Pre = null;

                do
                {
                    ballPool.counter--;
                    p.Recovery();
                    p = p.Pre;
                } while (p != null);
                break;
            }
        }
    }
}
