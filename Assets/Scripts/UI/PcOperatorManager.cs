using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PcOperatorManager : PcUiManager
{
    public static PcOperatorManager Instance;

    [SerializeField] private VisualTreeAsset manualWindow;
    [SerializeField] private VisualTreeAsset calcWindow;
    [SerializeField] private VisualTreeAsset morseWindow;
    [SerializeField] private VisualTreeAsset wireWindow;

    private VisualTreeAsset[] annexes;

    private Dictionary<string, VisualElement> windows;

    [Header("Manual")]
    [SerializeField] private VisualTreeAsset introManual;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        ClearRenderTexture();
        Instance = this;
        InstanceAbs = this;
        annexes = Resources.LoadAll<VisualTreeAsset>("Applis");
        windows = new Dictionary<string, VisualElement>();
    }

    protected override void OnComputerLoggedIn()
    {
        doc.visualTreeAsset = mainScreenWindow;
        screenHolder = doc.rootVisualElement.Q("screenHolder");
        SetupToolBar();
        InitializeWindow("desktop", desktopWindow);
        InitializeWindow("manual", manualWindow);
        InitializeWindow("morse", morseWindow);
        InitializeWindow("wire", wireWindow);
        InitializeWindow("calculator", calcWindow);
        OpenDesktop();

        PopupManager.Instance.InitPopups();
    }

    private void InitializeWindow(string key, VisualTreeAsset asset)
    {
        VisualElement window = asset.CloneTree();
        window.style.display = DisplayStyle.None;
        if (key != "desktop")
        {
            Button closeBtn = window.Q<Button>("closeBtn");
            closeBtn.clicked += OpenDesktop;
        }

        switch (key)
        {
            case "manual":
                VisualElement main = window.Q<VisualElement>("scrollManualZone");
                main.Add(introManual.CloneTree());
                VisualElement[] modules = ManuelSquad.Instance.GetModuleRules();
                foreach (VisualElement module in modules)
                {
                    main.Add(module);
                }
                break;
            case "morse":
                window.Q<MorseAppliElement>().Init(MainGeneration.Instance.GetRuleHolder().GetMorseRuleGenerator().GetAlphabet(), annexes[(int)ComputerAppliAnnexe.MORSE_CHARACTER_ELEMENT]);
                break;
        }

        windows.Add(key, window);
        screenHolder.Add(window);
    }

    private void HideAllWindows()
    {
        foreach (VisualElement window in windows.Values)
        {
            window.style.display = DisplayStyle.None;
        }
    }

    private void ShowWindow(string key)
    {
        if (windows.TryGetValue(key, out VisualElement window))
        {
            HideAllWindows();
            window.style.display = DisplayStyle.Flex;
            window.BringToFront();
        }
        else
        {
            Debug.LogWarning($"Window with key '{key}' not found.");
        }
    }

    public override void OpenDesktop() => ShowWindow("desktop");

    /// <summary>
    /// Ouvre le manuel avec tous les modules
    /// </summary>
    public void OpenManual() => ShowWindow("manual");

    /// <summary>
    /// Ouvre la calculatrice NYI
    /// </summary>
    public void OpenCalculator() => ShowWindow("calculator");

    /// <summary>
    /// Ouvre l'appli pour l'alphabet morse
    /// </summary>
    public void OpenMorse() => ShowWindow("morse");

    /// <summary>
    /// Ouvre l'appli pour voir les différents types de fils
    /// </summary>
    public void OpenWire() => ShowWindow("wire");

    /// <summary>
    /// Setup la barre de taches
    /// </summary>
    protected override void SetupToolBar()
    {
        ToolBarElement toolbar = doc.rootVisualElement.Q<ToolBarElement>();
        toolbar.Init();
    }

    /// <summary>
    /// Recupere la taille du screen Holder
    /// (Bricolage car resolvedStyle et style ne marche pas)
    /// </summary>
    /// <returns>Width en x et height en y</returns>
    public Vector2 GetScreenSize()
    {
        return new Vector2(1024, 448);
    }

    /// <summary>
    /// Ajoute un popup a l'ecran
    /// </summary>
    /// <param name="popup">Le popup a ajouter</param>
    public void AddPopup(VisualElement popup)
    {
        screenHolder.Add(popup);
    }

    /// <summary>
    /// Supprime un popup de l'ecran
    /// </summary>
    /// <param name="popup">Le popup a supprimer</param>
    public void RemovePopup(VisualElement popup)
    {
        screenHolder.Remove(popup);
    }

    /// <summary>
    /// Recupere un element par son nom
    /// </summary>
    /// <param name="name">Le nom de l'elem qu'on veut recup</param>
    /// <returns>L'element qu'on voulait, null si il y est pas</returns>
    public VisualElement GetElementByName(string name)
    {
        VisualElement elem = screenHolder.Q(name);
        if (elem == null)
        {
            Debug.LogWarning($"Element with name '{name}' not found.");
        }
        return elem;
    }
}
