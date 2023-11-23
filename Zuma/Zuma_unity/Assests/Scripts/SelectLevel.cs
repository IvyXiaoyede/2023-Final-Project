using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    public Button btn_World_1, btn_World_2, btn_World_3;
    public int worldIndex = 0, levelIndex = 0;
    public int WorldBgCount_1 = 10;
    public int WorldBgCount_2 = 10;

    private void Awake()
    {
        //GameData.LevelIndex = levelIndex;
        btn_World_1.onClick.AddListener(() =>
            {
               levelIndex = 0;
                GameData.LevelIndex = levelIndex;
                SceneManager.LoadScene("Game");
            });
            btn_World_2.onClick.AddListener(() =>
            {
                levelIndex = 1;
                GameData.LevelIndex = levelIndex;
                SceneManager.LoadScene("Game");
            });
            btn_World_3.onClick.AddListener(() =>
            {
                levelIndex = 2;
                GameData.LevelIndex = levelIndex;
                SceneManager.LoadScene("Game");       
            });


    }
}
