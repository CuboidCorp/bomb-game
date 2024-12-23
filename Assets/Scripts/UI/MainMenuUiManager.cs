using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class MainMenuUiManager : MonoBehaviour
{
    private UIDocument doc;
    private IntegerField seedField;
    private Button randSeedBtn;

    private Button startOpGameBtn;
    private Button startAgtGameBtn;

    private List<Locale> locales;
    private DropdownField languageDropdown;



    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        seedField = doc.rootVisualElement.Q<IntegerField>("seed");
        randSeedBtn = doc.rootVisualElement.Q<Button>("randomizeSeed");

        startOpGameBtn = doc.rootVisualElement.Q<Button>("startOpBtn");
        startAgtGameBtn = doc.rootVisualElement.Q<Button>("startAgtBtn");

        languageDropdown = doc.rootVisualElement.Q<DropdownField>("changeLocale");
        locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (Locale locale in locales)
        {
            languageDropdown.choices.Add(locale.Identifier.CultureInfo.NativeName);
        }
        languageDropdown.value = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.NativeName;
    }

    private void OnEnable()
    {
        randSeedBtn.clicked += RandomizeSeed;

        startOpGameBtn.clicked += StartOperatorGame;
        startAgtGameBtn.clicked += StartAgentGame;

        languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);
    }

    private void OnDisable()
    {
        randSeedBtn.clicked -= RandomizeSeed;

        startOpGameBtn.clicked -= StartOperatorGame;
        startAgtGameBtn.clicked -= StartAgentGame;

        languageDropdown.UnregisterValueChangedCallback(OnLanguageChanged);
    }

    /// <summary>
    /// Randomise le seed
    /// </summary>
    private void RandomizeSeed()
    {
        seedField.value = Random.Range(0, 100000);
    }

    private void GenerateSeedHolder()
    {
        int seed = seedField.value;

        GameObject mainGen = Instantiate(Resources.Load<GameObject>("MainGen"));

        mainGen.GetComponent<MainGeneration>().SetSeed(seed);
    }

    /// <summary>
    /// Start la scene de manuel
    /// </summary>
    private void StartOperatorGame()
    {
        Debug.Log("Starting Operator Game");
        if (MainGeneration.Instance == null)
        {
            GenerateSeedHolder();
        }
        SceneManager.LoadScene("Operator");
    }

    /// <summary>
    /// Start la scene de desamorcage
    /// </summary>
    private void StartAgentGame()
    {
        Debug.Log("Starting Agent Game");
        if (MainGeneration.Instance == null)
        {
            GenerateSeedHolder();
        }
        SceneManager.LoadScene("Agent");
    }

    /// <summary>
    /// Fonction appel�e quand la langue est chang�e
    /// </summary>
    /// <param name="evt">L'evenement de changement</param>
    private void OnLanguageChanged(ChangeEvent<string> evt)
    {
        string localeName = evt.newValue;
        foreach (Locale locale in locales)
        {
            if (locale.Identifier.CultureInfo.NativeName == localeName)
            {
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }
    }
}
