using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button[] buttons;

    public void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenLevel(int _)
    {
        SceneManager.LoadScene("GameScene");
    }
}
