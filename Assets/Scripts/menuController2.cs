using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenuController : MonoBehaviour
{
    public GameObject LoadingScene;
    public GameObject Menu;
    public void PlayGame()
    {
        Menu.SetActive(false);
        LoadingScene.SetActive(true);
        Invoke("startSceneLevel1", 1f);
    }
 
    public void ResumeGame()
    {
        Debug.Log("ResumeGame");
        Time.timeScale = 1;
        SceneManager.UnloadScene(2);
    }
    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    private void startSceneLevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
