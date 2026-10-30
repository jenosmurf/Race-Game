using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider, musicSlider, engineSlider;
    public GameObject MainUI, PlayUI, SettingUI, PauseUI, GameUI;
    public GameObject[] MenuUI;
    public bool paused;
    float MasterValue = 1f, MusicValue = 1f, EngineValue = 1f;

    void Start()
    {
        Time.timeScale = 1f;
        paused = false;

        if (!PlayerPrefs.HasKey("AudioSettingsInitialized"))
        {
            PlayerPrefs.SetFloat("MasterAudio", 1f);
            PlayerPrefs.SetFloat("EngineAudio", 1f);
            PlayerPrefs.SetFloat("MusicAudio", 1f);
            PlayerPrefs.SetInt("AudioSettingsInitialized", 1);
            PlayerPrefs.Save();
        }

        MasterValue = PlayerPrefs.GetFloat("MasterAudio", 1f);
        EngineValue = PlayerPrefs.GetFloat("EngineAudio", 1f);
        MusicValue = PlayerPrefs.GetFloat("MusicAudio", 1f);
        if (masterSlider != null)
        {
            masterSlider.SetValueWithoutNotify(MasterValue);
        }
        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(MusicValue);
        }
        if (engineSlider != null)
        {
            engineSlider.SetValueWithoutNotify(EngineValue);
        }

        ApplyAudio("MasterAudio", MasterValue);
        ApplyAudio("EngineAudio", EngineValue);
        ApplyAudio("MusicAudio", MusicValue);
    }

    void Update()
    {
        if (GameUI != null && (GameUI.activeSelf || paused))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    void LateUpdate()
    {
        if (paused && GameUI != null)
        {
            GameUI.SetActive(false);
        }
    }

    public void TogglePause()
    {
        if (SettingUI != null && SettingUI.activeSelf)
        {
            SettingUI.SetActive(false);
        }
        SetPaused(!paused);
    }

    void SetPaused(bool value)
    {
        paused = value;
        Time.timeScale = paused ? 0f : 1f;

        if (PauseUI != null)
        {
            PauseUI.SetActive(paused);
        }
        if (GameUI != null)
        {
            GameUI.SetActive(!paused);
        }
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
    public void PlayBtn()
    {
        MainUI.SetActive(false);
        PlayUI.SetActive(true);
    }
    public void SettingBtn()
    {
        if (paused)
        {
            PauseUI.SetActive(false);
            SettingUI.SetActive(true);
        }
        else
        {
            MainUI.SetActive(false);
            SettingUI.SetActive(true);
        }
    }
    public void ExitBtn()
    {
        Application.Quit();
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("New Scene (1) (1)");
    }
    public void PlayZandvoort()
    {
        SceneManager.LoadScene("New Scene (1)");
    }
    public void PlayNürburgring()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void BackBtn()
    {
        if (PlayUI.activeSelf)
        {
            PlayUI.SetActive(false);
            MainUI.SetActive(true);
        }
        if (SettingUI.activeSelf)
        {
            SettingUI.SetActive(false);
            MainUI.SetActive(true);
        }
    }

    public void AidsBtn()
    {
        if (SettingUI.activeSelf)
        {
            SettingUI.SetActive(false);
            PauseUI.SetActive(true);
        }
    }

    public void MasterAudio()
    {
        if (masterSlider != null)
        {
            MasterValue = masterSlider.value;
        }
        ApplyAudio("MasterAudio", MasterValue);
    }
    public void MasterAudio(float value)
    {
        MasterValue = value;
        MasterAudio();
    }
    public void MusicAudio()
    {
        if (musicSlider != null)
        {
            MusicValue = musicSlider.value;
        }
        ApplyAudio("MusicAudio", MusicValue);
    }
    public void MusicAudio(float value)
    {
        MusicValue = value;
        MusicAudio();
    }
    public void EngineAudio()
    {
        if (engineSlider != null)
        {
            EngineValue = engineSlider.value;
        }
        ApplyAudio("EngineAudio", EngineValue);
    }
    public void EngineAudio(float value)
    {
        EngineValue = value;
        EngineAudio();
    }

    void ApplyAudio(string parameter, float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp01(value) * 0.9999f + 0.0001f) * 20f;
        audioMixer.SetFloat(parameter, volume);
        PlayerPrefs.SetFloat(parameter, Mathf.Clamp01(value));
        PlayerPrefs.Save();
    }

}
