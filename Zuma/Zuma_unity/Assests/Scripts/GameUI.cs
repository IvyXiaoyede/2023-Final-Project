using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    public GameObject overPanel;
    public GameObject SuccPanel;

    private void Awake()
    {
        Instance = this;
        overPanel.transform.Find("btn_Reset").GetComponent<Button>().onClick.AddListener(() =>
        {
            overPanel.SetActive(false);
            SuccPanel.SetActive(false);
            GameManager.Instance.StartBack();
        });
        overPanel.transform.Find("btn_Replay").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        overPanel.transform.Find("btn_Home").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start");
        });
        SuccPanel.transform.Find("btn_Home").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start");
        });
        SuccPanel.transform.Find("btn_Next").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameData.LevelIndex++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
    public void ShowOverPanel()
    {
        overPanel.SetActive(true);
    }
    public void ShowSuccPanel()
    {
        SuccPanel.SetActive(true);
    }
}
