using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public Text scoreText;

    public int coinCollected = 0;
    public GameObject levelCompletePanel;

    private int targetCoin = 4;
    private string nextLevelName = "Level2";

    public int playerHealth = 3;
    public int playerMaxHealth = 3;

    // menampilkan bintang di pannel
    public Image[] starImages;  // Array isi 3 image untuk bintang
    public Sprite filledStar;   // Sprite bintang penuh
    public Sprite emptyStar;    // Sprite bintang kosong
    public Text finalScoreText; // Text untuk menampilkan score akhir

    // void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         if (transform.parent == null)
    //         {
    //             DontDestroyOnLoad(gameObject);
    //             Debug.Log("GameManager: DontDestroyOnLoad applied");
    //         }
    //         else
    //         {
    //             Debug.LogWarning("GameManager harus di root GameObject supaya DontDestroyOnLoad bisa dipakai");
    //         }

    //         SceneManager.sceneLoaded += OnSceneLoaded;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager: DontDestroyOnLoad applied");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    else if (instance != this)
    {
        Debug.Log("Duplicate GameManager detected, destroying this one.");
        Destroy(gameObject);
    }
}

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        coinCollected = 0;
        score = 0;

        if (scene.name == "Level1")
        {
            playerHealth = playerMaxHealth;
            Debug.Log($"[GameManager] playerHealth di-reset ke maksimal: {playerHealth}");
        }
        else
        {
            Debug.Log($"[GameManager] playerHealth tidak di-reset, tetap: {playerHealth}");
        }

        // Cari ulang setiap kali scene load
        levelCompletePanel = GameObject.Find("Canvas/LevelCompletePanel");
        if (levelCompletePanel == null)
            Debug.LogWarning("LevelCompletePanel tidak ditemukan di scene!");
        else
            levelCompletePanel.SetActive(false);

        GameObject scoreGO = GameObject.Find("ScoreText");
        if (scoreGO != null)
            scoreText = scoreGO.GetComponent<Text>();
        else
            Debug.LogWarning("ScoreText tidak ditemukan di scene!");

        UpdateScoreUI();

        if (scene.name == "Level1")
        {
            targetCoin = 4;
            nextLevelName = "Level2";
        }
        else if (scene.name == "Level2")
        {
            targetCoin = 8;
            nextLevelName = "Level3";
        }
        else if (scene.name == "Level3")
        {
            targetCoin = 10;
            nextLevelName = "";
        }
        else
        {
            targetCoin = 4;
            nextLevelName = "";
        }

        Debug.Log($"TargetCoin: {targetCoin}, NextLevel: {nextLevelName}");

        // Pastikan tombol Next di panel LevelComplete punya listener yang benar
        SetupNextButtonListener();
    // -------------------------------
    // Tambahan untuk FinalScoreText & Star1-3:
    // -------------------------------
    Transform finalScoreTransform = levelCompletePanel.transform.Find("FinalScoreText");
if (finalScoreTransform != null)
    finalScoreText = finalScoreTransform.GetComponent<Text>();
else
    Debug.LogWarning("FinalScoreText tidak ditemukan di LevelCompletePanel!");

    // Cari star images (Star1, Star2, Star3)
    starImages = new Image[3];
for (int i = 0; i < 3; i++)
{
    Transform starTransform = levelCompletePanel.transform.Find($"Star{i+1}");
    if (starTransform != null)
        starImages[i] = starTransform.GetComponent<Image>();
    else
        Debug.LogWarning($"Star{i+1} tidak ditemukan di LevelCompletePanel!");
}

}

private void SetupNextButtonListener()
{
    if (levelCompletePanel != null)
    {
        Button nextButton = levelCompletePanel.transform.Find("NextButton")?.GetComponent<Button>();
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(LoadNextLevel);
            Debug.Log("[GameManager] Listener tombol Next di-set ulang");
        }
        else
        {
            Debug.LogWarning("Tombol Next tidak ditemukan di LevelCompletePanel");
        }
    }
}

    public void AddScore(int amount)
    {
        score += amount;
        coinCollected++;
        Debug.Log($"AddScore called: score={score}, coinCollected={coinCollected}/{targetCoin}");
        UpdateScoreUI();

        if (coinCollected >= targetCoin)
        {
            ShowLevelComplete();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText == null)
        {
            GameObject scoreGO = GameObject.Find("ScoreText");
            if (scoreGO != null)
            {
                scoreText = scoreGO.GetComponent<Text>();
                Debug.Log("[GameManager] ScoreText ditemukan ulang di UpdateScoreUI");
            }
            else
            {
                Debug.LogWarning("ScoreText masih null saat UpdateScoreUI dipanggil");
                return; // Jangan lanjut kalau null
            }
        }
        scoreText.text = score.ToString();
    }

    public void ShowLevelComplete()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            Debug.Log("[GameManager] Level Complete Panel ditampilkan");

               // Tampilkan score akhir
            if (finalScoreText != null)
                finalScoreText.text = "Score: " + score;

            // Hitung dan tampilkan bintang
            UpdateStars();

            // Bonus nyawa +1
            playerHealth += 1;
            if (playerHealth > playerMaxHealth)
                playerHealth = playerMaxHealth;

            Debug.Log($"[GameManager] Bonus nyawa +1, playerHealth sekarang: {playerHealth}");

         
        }
    }
    private void UpdateStars()
{
    int starCount = playerHealth;  // langsung sesuai jumlah nyawa

    // Pastikan starCount tidak lebih dari 3 dan tidak kurang dari 1
    starCount = Mathf.Clamp(starCount, 1, 3);

    Debug.Log($"[GameManager] Bintang yang didapat: {starCount}");

    // Update gambar bintang
    for (int i = 0; i < starImages.Length; i++)
    {
        if (starImages[i] != null)
            starImages[i].sprite = (i < starCount) ? filledStar : emptyStar;
    }
}



    public void LoadNextLevel()
    {
        Debug.Log("Tombol Next ditekan");
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Debug.Log($"Memuat level berikutnya: {nextLevelName} dengan nyawa tersisa: {playerHealth}");
            Time.timeScale = 1;
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.Log("Game selesai! Tidak ada level selanjutnya.");
        }
    }

    public void DamagePlayer(int damage)
    {
        playerHealth -= damage;
        if (playerHealth < 0)
            playerHealth = 0;

        Debug.Log($"[GameManager] Player took damage: {damage}, health sekarang: {playerHealth}");
    }
    public void ResetLevelData()
{
    // Panggil ulang OnSceneLoaded secara internal
    OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
}

}
