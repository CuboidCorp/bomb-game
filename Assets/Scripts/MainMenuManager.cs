using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private PlayerControls actions;
    private InputAction _pos;

    private Camera mainCamera;

    private UIDocument doc;
    private VisualElement currentRootElement;

    [SerializeField] private AudioMixer mainAudioMixer;

    [SerializeField] private VisualTreeAsset mainMenuUI;
    [SerializeField] private VisualTreeAsset optionsMenuUI;

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text playLabel;
    [SerializeField] private TMP_Text optionsLabel;
    [SerializeField] private TMP_Text quitLabel;

    private string targetLocaleCode;

    #region MainMenu
    private Label versionLabel;

    private List<Locale> locales;
    private DropdownField languageDropdown;

    #endregion

    #region OptionsMenu
    private Slider generalVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Button saveButton;
    private Button cancelButton;
    #endregion

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        currentRootElement = doc.rootVisualElement;
        mainCamera = Camera.main;

        UnityEngine.Cursor.visible = true;
    }

    private void OnEnable()
    {
        actions = new PlayerControls();
        actions.Enable();
        SetupActions();
    }

    private void OnDisable()
    {
        actions.Disable();
        UnSetupActions();
    }

    private void Start()
    {
        LoadPrefs();
        SetupMainMenuUI();
    }

    private void SetupMainMenuUI()
    {
        doc.visualTreeAsset = mainMenuUI;

        currentRootElement = doc.rootVisualElement;
        versionLabel = currentRootElement.Q<Label>("versionLabel");
        versionLabel.text = "v" + Application.version;

        languageDropdown = doc.rootVisualElement.Q<DropdownField>("changeLocale");

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(targetLocaleCode);

        locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (Locale locale in locales)
        {
            string nativeName = locale.Identifier.CultureInfo.NativeName;
            string nativeNameFormatted = char.ToUpper(nativeName[0]) + nativeName[1..];
            languageDropdown.choices.Add(nativeNameFormatted);
        }

        languageDropdown.value = char.ToUpper(LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName[0])
                         + LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName[1..];

        languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);

        title.text = TextLocalizationHandler.LoadString("MainMenu", "MAIN_TITLE");
        playLabel.text = TextLocalizationHandler.LoadString("MainMenu", "PLAY_BTN");
        optionsLabel.text = TextLocalizationHandler.LoadString("MainMenu", "OPTIONS_BTN");
        quitLabel.text = TextLocalizationHandler.LoadString("MainMenu", "QUIT_BTN");
    }

    private void UnSetupMainMenuUI()
    {
        languageDropdown.UnregisterValueChangedCallback(OnLanguageChanged);
    }

    /// <summary>
    /// Fonction appelée quand la langue est changée
    /// </summary>
    /// <param name="evt">L'evenement de changement</param>
    private void OnLanguageChanged(ChangeEvent<string> evt)
    {
        string localeName = evt.newValue;
        foreach (Locale locale in locales)
        {
            string nativeName = locale.Identifier.CultureInfo.NativeName;
            string nativeNameFormatted = char.ToUpper(nativeName[0]) + nativeName[1..];
            if (nativeNameFormatted == localeName)
            {
                PlayerPrefs.SetString("locale", locale.Identifier.Code);
                PlayerPrefs.Save();
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }

        //Juste pour le menu principal on change les valeurs des textes
        title.text = TextLocalizationHandler.LoadString("MainMenu", "MAIN_TITLE");
        playLabel.text = TextLocalizationHandler.LoadString("MainMenu", "PLAY_BTN");
        optionsLabel.text = TextLocalizationHandler.LoadString("MainMenu", "OPTIONS_BTN");
        quitLabel.text = TextLocalizationHandler.LoadString("MainMenu", "QUIT_BTN");
    }

    private void SetupActions()
    {
        _pos = actions.Player.Position;

        actions.Player.Tap.performed += OnTap;
    }

    private void UnSetupActions()
    {
        actions.Player.Tap.performed -= OnTap;
    }

    private void OnTap(InputAction.CallbackContext _)
    {
        Vector2 pos = _pos.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, 10))
        {
            //On check si on a touché un des boutons du menu ou l'engrenage des options
            if (hit.collider.CompareTag("MainMenuInteractable"))
            {
                switch (hit.collider.name)
                {
                    case "GearOptions":
                        OpenOptionsMenu();
                        break;
                    case "PlayButton":
                        PlayGame();
                        break;
                    case "QuitButton":
                        QuitGame();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Quitte le jeu avec un bruit d'explosion
    /// </summary>
    private void QuitGame()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.BOMB_EXPLOSION);

        StartCoroutine(QuitGameAfter1Sec());
    }

    /// <summary>
    /// Quitte le jeu après 1 seconde
    /// </summary>
    /// <returns></returns>
    private IEnumerator QuitGameAfter1Sec()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    /// <summary>
    /// Lance le jeu
    /// </summary>
    private void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }


    public void OpenOptionsMenu()
    {
        UnSetupMainMenuUI();
        UnSetupActions();
        doc.visualTreeAsset = optionsMenuUI;
        currentRootElement = doc.rootVisualElement;

        generalVolumeSlider = currentRootElement.Q<Slider>("generalSlider");
        musicVolumeSlider = currentRootElement.Q<Slider>("musicSlider");
        sfxVolumeSlider = currentRootElement.Q<Slider>("sfxSlider");
        saveButton = currentRootElement.Q<Button>("saveBtn");
        cancelButton = currentRootElement.Q<Button>("cancelBtn");
        saveButton.clicked += SaveOptions;
        cancelButton.clicked += CloseOptionsMenu;
        LoadOptions();

    }

    /// <summary>
    /// Charge les options depuis les PlayerPrefs
    /// </summary>
    public void LoadOptions()
    {
        generalVolumeSlider.value = PlayerPrefs.GetFloat("generalVolume", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);

        LoadPrefs();
    }

    private void LoadPrefs()
    {
        targetLocaleCode = PlayerPrefs.GetString("locale", "en");

        mainAudioMixer.SetFloat("mainVolume", Mathf.Log10(PlayerPrefs.GetFloat("generalVolume", 1)) * 20);
        mainAudioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume", 1)) * 20);
        mainAudioMixer.SetFloat("sfxVolume", Mathf.Log10(PlayerPrefs.GetFloat("sfxVolume", 1)) * 20);
    }

    /// <summary>
    /// Sauvegarde les options dans les PlayerPrefs et les applique
    /// </summary>
    private void SaveOptions()
    {
        Debug.Log("Save options");
        PlayerPrefs.SetFloat("generalVolume", generalVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);

        PlayerPrefs.Save();

        LoadPrefs();
        CloseOptionsMenu();

    }

    /// <summary>
    /// Ferme le menu des options et reouvre le menu de pause
    /// </summary>
    private void CloseOptionsMenu()
    {
        SetupActions();
        saveButton.clicked -= SaveOptions;
        cancelButton.clicked -= CloseOptionsMenu;
        SetupMainMenuUI();

    }
}
