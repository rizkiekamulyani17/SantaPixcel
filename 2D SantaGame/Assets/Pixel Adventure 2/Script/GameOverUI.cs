using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    private bool sceneLoaded = false;
    private string currentSceneName;

    public void RestartGame()
    {
        Time.timeScale = 1f;

        currentSceneName = SceneManager.GetActiveScene().name;

        if (SantaHealth.instance != null)
            SantaHealth.instance.RestartGame();
        else
            Debug.LogWarning("SantaHealth instance tidak ditemukan!");

        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        sceneLoaded = false;
        SceneManager.sceneLoaded += OnSceneLoaded;  // daftarkan method dengan tipe UnityAction

        SceneManager.LoadScene(currentSceneName);

        while (!sceneLoaded)
            yield return null;

        SceneManager.sceneLoaded -= OnSceneLoaded; // hapus listener

        if (GameManager.instance != null)
        {
            GameManager.instance.ResetLevelData();
        }
        else
        {
            Debug.LogWarning("GameManager instance tidak ditemukan saat restart!");
        }
    }

    // Method yang sesuai delegate UnityAction<Scene, LoadSceneMode>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == currentSceneName)
        {
            sceneLoaded = true;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Keluar Game!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
