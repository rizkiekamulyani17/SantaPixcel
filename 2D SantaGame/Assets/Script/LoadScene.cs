using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void paused()
    {
        Time.timeScale = 0;
    }

    public void played()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        // Pastikan timeScale kembali normal
        Time.timeScale = 1;

        // Ambil scene yang sedang aktif, lalu muat ulang
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
