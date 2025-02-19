using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class MainMenuUiManager : MonoBehaviour
{
    private UIDocument doc;

    private Button playBtn;
    private Button optionsBtn;
    private Button quitBtn;

    private Label versionLabel;

    private List<Locale> locales;
    private DropdownField languageDropdown;

    private void Awake()
    {

        doc = GetComponent<UIDocument>();

        playBtn = doc.rootVisualElement.Q<Button>("playBtn");
        optionsBtn = doc.rootVisualElement.Q<Button>("optionsBtn");
        quitBtn = doc.rootVisualElement.Q<Button>("quitBtn");

        versionLabel = doc.rootVisualElement.Q<Label>("versionLabel");
        versionLabel.text = "v" + Application.version;

        languageDropdown = doc.rootVisualElement.Q<DropdownField>("changeLocale");
        locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (Locale locale in locales)
        {
            string nativeName = locale.Identifier.CultureInfo.NativeName;
            string nativeNameFormatted = char.ToUpper(nativeName[0]) + nativeName[1..];
            languageDropdown.choices.Add(nativeNameFormatted);
        }

        languageDropdown.value = char.ToUpper(LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName[0])
                         + LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName[1..];

        UnityEngine.Cursor.visible = true;
    }

    private void OnEnable()
    {
        playBtn.clicked += StartGame;
        optionsBtn.clicked += OpenOptions;
        quitBtn.clicked += QuitGame;

        languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);
    }

    private void OnDisable()
    {
        playBtn.clicked -= StartGame;
        optionsBtn.clicked -= OpenOptions;
        quitBtn.clicked -= QuitGame;

        languageDropdown.UnregisterValueChangedCallback(OnLanguageChanged);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    private void OpenOptions()
    {
        OptionsMainMenuManager.Instance.OpenOptionsMenu();
    }

    private void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
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
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }
    }
}
