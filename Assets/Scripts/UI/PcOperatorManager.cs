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
    }

    private void Start()
    {
        windows = new Dictionary<string, VisualElement>();
        InitializeWindow("manual", manualWindow);
        InitializeWindow("morse", morseWindow);
        InitializeWindow("wire", wireWindow);
        InitializeWindow("calculator", calcWindow);

    }

    private void InitializeWindow(string key, VisualTreeAsset asset)
    {
        VisualElement window = asset.CloneTree();
        window.style.display = DisplayStyle.None;
        Button closeBtn = window.Q<Button>("closeBtn");
        closeBtn.clicked += () => CloseWindow(key);
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

    private void ShowWindow(string key)
    {
        if (windows.TryGetValue(key, out VisualElement window))
        {
            window.style.display = DisplayStyle.Flex;
            window.BringToFront();
        }
        else
        {
            Debug.LogWarning($"Window with key '{key}' not found.");
        }
    }

    private void CloseWindow(string key)
    {
        if (windows.TryGetValue(key, out VisualElement window))
        {
            window.style.display = DisplayStyle.None;
        }
        else
        {
            Debug.LogWarning($"Window with key '{key}' not found.");
        }
    }

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


}
