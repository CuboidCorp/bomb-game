using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class OptionsMainMenuManager : MonoBehaviour
{
    private UIDocument uiDoc;
    private VisualElement currentRootElement;
    public static OptionsMainMenuManager Instance;

    [SerializeField] private AudioMixer mainAudioMixer;

    #region OptionsMenu
    private Slider generalVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Button saveButton;
    private Button cancelButton;
    #endregion

    private void Awake()
    {
        Instance = this;
        uiDoc = GetComponent<UIDocument>();
        OpenOptionsMenu();
    }

    public void OpenOptionsMenu()
    {
        uiDoc.enabled = true;
        currentRootElement = uiDoc.rootVisualElement;
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

        mainAudioMixer.SetFloat("mainVolume", Mathf.Log10(generalVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);

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

        mainAudioMixer.SetFloat("mainVolume", Mathf.Log10(generalVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        mainAudioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);

        CloseOptionsMenu();

    }

    /// <summary>
    /// Ferme le menu des options et reouvre le menu de pause
    /// </summary>
    private void CloseOptionsMenu()
    {
        uiDoc.enabled = false;
        saveButton.clicked -= SaveOptions;
        cancelButton.clicked -= CloseOptionsMenu;
    }
}
