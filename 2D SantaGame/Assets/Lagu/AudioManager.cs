using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public Slider volumeSlider;

    public Button musicOnButton;
    public Button musicOffButton;

    private bool isMusicOn = true;

    void Awake()
    {
        // Singleton: biar tidak dobel saat pindah scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Tetap hidup antar scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan yang dobel
        }
    }

    void Start()
    {
        volumeSlider.value = musicSource.volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicOnButton.onClick.AddListener(TurnOffMusic);
        musicOffButton.onClick.AddListener(TurnOnMusic);

        UpdateMusicButtons();
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void TurnOffMusic()
    {
        isMusicOn = false;
        musicSource.mute = true;
        UpdateMusicButtons();
    }

    public void TurnOnMusic()
    {
        isMusicOn = true;
        musicSource.mute = false;
        UpdateMusicButtons();
    }

    private void UpdateMusicButtons()
    {
        musicOnButton.gameObject.SetActive(isMusicOn);
        musicOffButton.gameObject.SetActive(!isMusicOn);
    }
}
