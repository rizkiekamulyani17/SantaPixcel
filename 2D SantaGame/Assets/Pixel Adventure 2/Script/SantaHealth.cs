using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SantaHealth : MonoBehaviour
{
    public static SantaHealth instance;
    public int maxHealth = 3;
    private int currentHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameObject gameOverPanel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable() // <== FIX disini ganti dari Start() ke OnEnable()
    {
        currentHealth = GameManager.instance.playerHealth;
        maxHealth = GameManager.instance.playerMaxHealth;

        UpdateHeartsUI();
        Debug.Log($"[SantaHealth] OnEnable: currentHealth = {currentHealth}, maxHealth = {maxHealth}");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

  


    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
        }
    }

    void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("[SantaHealth] Game Over!");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        GameManager.instance.playerHealth = currentHealth;

        UpdateHeartsUI();

        if (currentHealth == 0)
        {
            Debug.Log("Game Over!");
            GameOver();
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        GameManager.instance.playerHealth = currentHealth;

        UpdateHeartsUI();
    }

public void RestartGame()
{
    Time.timeScale = 1f; // Balikkan ke normal

    // Reset nyawa ke penuh
    GameManager.instance.playerHealth = GameManager.instance.playerMaxHealth;
    Debug.Log($"[GameOverUI] Reset playerHealth ke: {GameManager.instance.playerHealth}");

    // Restart scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Kelelawar") || collision.CompareTag("Badak") || collision.CompareTag("Dino"))
    {
        TakeDamage(1);
        Debug.Log($"Santa kena {collision.tag}! Nyawa berkurang.");
    }
    else if (collision.CompareTag("Laut"))
    {
        Debug.Log("Santa jatuh ke laut! Langsung Game Over.");

        // Set health ke 0 biar langsung Game Over
        currentHealth = 0;
        GameManager.instance.playerHealth = currentHealth;

        UpdateHeartsUI();
        GameOver();
    }
}


}
