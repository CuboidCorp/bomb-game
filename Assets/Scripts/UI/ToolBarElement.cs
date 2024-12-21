using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ToolBarElement : VisualElement
{
#nullable enable
    private ComputerLocation computerLocation;

    private PcUiManager pcUiManager;

    private const string selectedBtnClass = "selected-task-bar-item";

    private Button HomeButton => this.Q<Button>("mainMenuBtn");
    private Button ManualButton => this.Q<Button>("manualBtn");
    private Button CalculatorButton => this.Q<Button>("calcBtn");
    private Button MorseButton => this.Q<Button>("morseBtn");
    private Button WireButton => this.Q<Button>("wireBtn");

    public void Init()
    {
        Debug.Log("Init toolbar");
        pcUiManager = PcUiManager.Instance;
        computerLocation = ComputerLocation.HOME;
        HomeButton.clicked += GoToDesktop;
        ManualButton.clicked += GoToManual;
        CalculatorButton.clicked += GoToCalculator;
        MorseButton.clicked += GoToMorse;
        WireButton.clicked += GoToWire;
    }

    private void GoToDesktop()
    {
        Debug.Log("Go to desktop");
        computerLocation = ComputerLocation.HOME;
        pcUiManager.OpenDesktop();
    }

    private void GoToManual()
    {
        Debug.Log("Go to manual");
        computerLocation = ComputerLocation.MANUAL;
        pcUiManager.OpenManual();
    }

    private void GoToCalculator()
    {
        Debug.Log("Go to calculator");
        computerLocation = ComputerLocation.CALCULATOR;
        pcUiManager.OpenCalculator();
    }

    private void GoToMorse()
    {
        Debug.Log("Go to morse");
        computerLocation = ComputerLocation.MORSE;
        pcUiManager.OpenMorse();
    }

    private void GoToWire()
    {
        Debug.Log("Go to wire");
        computerLocation = ComputerLocation.WIRE;
        pcUiManager.OpenWire();
    }

    /// <summary>
    /// Ajoute le style selected à un bouton
    /// </summary>
    /// <param name="location">L'endroit associé au bouton</param>
    private void AddSelectedStyle(ComputerLocation location)
    {
        Button? button = GetButtonFromLocation(location);
        if (button != null)
        {
            Debug.Log("Add selected style");
            button.AddToClassList(selectedBtnClass);
        }
    }

    /// <summary>
    /// Enleve le style selected d'un bouton
    /// </summary>
    /// <param name="location">L'endroit associé au bouton</param>
    private void RemoveSelectedStyle(ComputerLocation location)
    {
        Button? button = GetButtonFromLocation(location);
        if (button != null)
        {
            button.RemoveFromClassList(selectedBtnClass);
        }
    }

    /// <summary>
    /// Renvoie le bouton associé à un emplacement
    /// </summary>
    /// <param name="location">L'emplacement dont on veut le bouton</param>
    /// <returns>Le bouton associé</returns>
    private Button? GetButtonFromLocation(ComputerLocation location)
    {
        return location switch
        {
            ComputerLocation.HOME => HomeButton,
            ComputerLocation.MANUAL => ManualButton,
            ComputerLocation.CALCULATOR => CalculatorButton,
            ComputerLocation.MORSE => MorseButton,
            ComputerLocation.WIRE => WireButton,
            _ => null,
        };
    }

    public ToolBarElement() { }

}
