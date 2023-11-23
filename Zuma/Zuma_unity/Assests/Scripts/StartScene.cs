using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Button btn_Start;

    private void Awake()
    {
        btn_Start.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SelectLevel");
        });
    }
}
