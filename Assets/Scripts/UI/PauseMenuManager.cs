using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{

    public static PauseMenuManager Instance;
    private UIDocument uiDoc;
    private VisualElement currentRootElement;
    [SerializeField] private VisualTreeAsset inGameMenu;
    [SerializeField] private VisualTreeAsset pauseMenu;
    [SerializeField] private VisualTreeAsset optionsMenu;

    [SerializeField] private AudioMixer mainAudioMixer;

    private bool isPaused;

    #region PauseMenu
    private Button resumeButton;
    private Button optionsButton;
    private Button quitButton;
    #endregion

    #region OptionsMenu
    private Slider generalVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Button saveButton;
    private Button cancelButton;
    private Button backButton;
    #endregion

    private void Awake()
    {
        uiDoc = GetComponent<UIDocument>();
        Instance = this;
        SetupInGameMenu();
        isPaused = false;
    }

    /// <summary>
    /// Setup le menu de jeu
    /// </summary>
    private void SetupInGameMenu()
    {
        if (inGameMenu != null)
        {
            SetVisualTreeAsset(inGameMenu);
        }
        else
        {
            uiDoc.visualTreeAsset = null;
        }
    }

    /// <summary>
    /// Désactive le menu de jeu
    /// </summary>
    private void UnSetupInGameMenu()
    {
    }

    /// <summary>
    /// Ouvre le menu de pause
    /// </summary>
    public void OpenPauseMenu()
    {
        isPaused = true;
        UnSetupInGameMenu();

        if (PcLevelSelectManager.Instance != null)
        {
            PcLevelSelectManager.Instance.UnSetupDoc();
        }
        if (PcOperatorManager.Instance != null)
        {
            PcOperatorManager.Instance.UnSetupDoc();
        }

        SetVisualTreeAsset(pauseMenu);
        resumeButton = currentRootElement.Q<Button>("cancelBtn");
        optionsButton = currentRootElement.Q<Button>("optionsBtn");
        quitButton = currentRootElement.Q<Button>("quitBtn");
        resumeButton.clicked += ClosePauseMenu;
        optionsButton.clicked += OpenOptionsMenu;
        quitButton.clicked += ReturnToMainMenu;
    }

    /// <summary>
    /// Ferme le menu de pause
    /// </summary>
    private void ClosePauseMenu()
    {
        isPaused = false;
        SetupInGameMenu();
        if (PcLevelSelectManager.Instance != null)
        {
            LevelSelectInteract.Instance.SetupActions();
            PcLevelSelectManager.Instance.SetupIfStarted();
        }
        if (PcOperatorManager.Instance != null)
        {
            ManualInteract.Instance.SetupActions();
            PcOperatorManager.Instance.SetupIfStarted();
        }
        DisablePauseMenu();
    }

    /// <summary>
    /// Ouvre ou ferme le menu de pause et renvoie true si le jeu est en pause
    /// </summary>
    /// <returns>True si le jeu est en pause, ou false</returns>
    public bool OpenOrClose()
    {
        if (isPaused)
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }

        return isPaused;
    }

    /// <summary>
    /// Retourne au menu principal
    /// </summary>
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Désactive les boutons du menu de pause
    /// </summary>
    private void DisablePauseMenu()
    {
        resumeButton.clicked -= ClosePauseMenu;
        optionsButton.clicked -= OpenOptionsMenu;
        quitButton.clicked -= ReturnToMainMenu;

        resumeButton = null;
        optionsButton = null;
        quitButton = null;
    }

    /// <summary>
    /// Ouvre le menu des options
    /// </summary>
    public void OpenOptionsMenu()
    {
        DisablePauseMenu();
        SetVisualTreeAsset(optionsMenu);
        generalVolumeSlider = currentRootElement.Q<Slider>("generalSlider");
        musicVolumeSlider = currentRootElement.Q<Slider>("musicSlider");
        sfxVolumeSlider = currentRootElement.Q<Slider>("sfxSlider");
        backButton = currentRootElement.Q<Button>("returnBtn");
        saveButton = currentRootElement.Q<Button>("saveBtn");
        cancelButton = currentRootElement.Q<Button>("cancelBtn");

        LoadOptions();

        saveButton.clicked += SaveOptions;
        cancelButton.clicked += LoadOptions;
        backButton.clicked += CloseOptionsMenu;
    }

    /// <summary>
    /// Ferme le menu des options et reouvre le menu de pause
    /// </summary>
    private void CloseOptionsMenu()
    {
        cancelButton.clicked -= LoadOptions;
        saveButton.clicked -= SaveOptions;
        backButton.clicked -= CloseOptionsMenu;
        generalVolumeSlider = null;
        musicVolumeSlider = null;
        OpenPauseMenu();
    }

    /// <summary>
    /// Charge les options depuis les PlayerPrefs
    /// </summary>
    public void LoadOptions()
    {
        generalVolumeSlider.value = PlayerPrefs.GetFloat("generalVolume", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);

        mainAudioMixer.SetFloat("mainVolume", Mathf.Log10(generalVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);

    }

    /// <summary>
    /// Sauvegarde les options dans les PlayerPrefs et les applique
    /// </summary>
    private void SaveOptions()
    {
        //TODO : PlayerPrefs
        PlayerPrefs.SetFloat("generalVolume", generalVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);

        PlayerPrefs.Save();

        mainAudioMixer.SetFloat("mainVolume", Mathf.Log10(generalVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);

    }

    /// <summary>
    /// Change le VisualTreeAsset de l'UIDocument et met à jour le rootElement
    /// </summary>
    /// <param name="vta">Le nouveau visual tree asset</param>
    private void SetVisualTreeAsset(VisualTreeAsset vta)
    {
        uiDoc.visualTreeAsset = vta;
        currentRootElement = uiDoc.rootVisualElement;
    }
}
