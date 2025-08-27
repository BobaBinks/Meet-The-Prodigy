using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenLevel(int levelID)
    {
        string levelname = "level 1" + levelID;
        SceneManager.LoadScene(levelname);
    }
}
