using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private static void Play(string clipName)
    {
        AudioSource.PlayClipAtPoint(GetAudioClip(clipName), Vector3.zero);
    }
    public static AudioClip GetAudioClip(string clipName)
    {
        return Resources.Load("Sound/" + clipName, typeof(AudioClip)) as AudioClip;
    }
    public static void PlayDestroy() { Play("Eliminate"); }
    public static void PlayShoot() { Play("Shoot"); }
    public static void PlayInsert() { Play("BallEnter"); }
    public static void PlayBomb() { Play("Bomb"); }
    public static void PlayFail() { Play("Fail"); }
    public static void PlayFastMove() { Play("FastMove"); }
}
