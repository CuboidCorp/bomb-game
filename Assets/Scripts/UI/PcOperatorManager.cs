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

    [Header("Manual")]
    [SerializeField] private VisualTreeAsset introManual;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        ClearRenderTexture();
        Instance = this;
        InstanceAbs = this;
        annexes = Resources.LoadAll<VisualTreeAsset>("Applis");
        Debug.Log("Annexes : " + annexes.Length);
    }

    /// <summary>
    /// Ouvre le manuel avec tous les modules
    /// </summary>
    public void OpenManual()
    {
        screenHolder.Clear();
        screenHolder.Add(manualWindow.CloneTree());
        VisualElement[] modules = ManuelSquad.Instance.GetModuleRules();
        VisualElement main = doc.rootVisualElement.Q<VisualElement>("scrollManualZone");
        //On rajoute l'intro en premier
        VisualElement intro = introManual.CloneTree();
        main.Add(intro);
        foreach (VisualElement module in modules)
        {
            main.Add(module);
        }
    }

    /// <summary>
    /// Ouvre la calculatrice NYI
    /// </summary>
    public void OpenCalculator()
    {
        screenHolder.Clear();
        screenHolder.Add(calcWindow.CloneTree());
    }

    /// <summary>
    /// Ouvre l'appli pour l'alphabet morse
    /// </summary>
    public void OpenMorse()
    {
        screenHolder.Clear();
        screenHolder.Add(morseWindow.CloneTree());
        doc.rootVisualElement.Q<MorseAppliElement>().Init(MainGeneration.Instance.GetRuleHolder().GetMorseRuleGenerator().GetAlphabet(), annexes[(int)ComputerAppliAnnexe.MORSE_CHARACTER_ELEMENT]);
    }

    /// <summary>
    /// Ouvre l'appli pour voir les différents types de fils
    /// </summary>
    public void OpenWire()
    {
        screenHolder.Clear();
        screenHolder.Add(wireWindow.CloneTree());
    }

    /// <summary>
    /// Setup la barre de taches
    /// </summary>
    protected override void SetupToolBar()
    {
        ToolBarElement toolbar = doc.rootVisualElement.Q<ToolBarElement>();
        toolbar.Init();
    }


}
