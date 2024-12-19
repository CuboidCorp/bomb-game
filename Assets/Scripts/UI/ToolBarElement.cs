using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ToolBarElement : VisualElement
{
#nullable enable
    private ComputerLocation computerLocation;

    private PcUiManager pcUiManager;

    private const string selectedBtnClass = "selected-task-bar-item";

    private Button homeButton => this.Q<Button>("mainMenuBtn");
    private Button manualButton => this.Q<Button>("manualBtn");
    private Button calculatorButton => this.Q<Button>("calcBtn");
    private Button morseButton => this.Q<Button>("morseBtn");
    private Button wireButton => this.Q<Button>("wireBtn");

    public void Init()
    {
        Debug.Log("Inut toolbar");
        pcUiManager = PcUiManager.Instance;
        computerLocation = ComputerLocation.HOME;
        homeButton.clicked += GoToDesktop;
        manualButton.clicked += GoToManual;
        calculatorButton.clicked += GoToCalculator;
        morseButton.clicked += GoToMorse;
        wireButton.clicked += GoToWire;
    }

    private void GoToDesktop()
    {
        Debug.Log("Go to desktop");
        RemoveSelectedStyle(computerLocation);
        computerLocation = ComputerLocation.HOME;
        AddSelectedStyle(computerLocation);
        pcUiManager.OpenDesktop();
    }

    private void GoToManual()
    {
        Debug.Log("Go to manual");
        RemoveSelectedStyle(computerLocation);
        computerLocation = ComputerLocation.MANUAL;
        AddSelectedStyle(computerLocation);
        pcUiManager.OpenManual();
    }

    private void GoToCalculator()
    {
        Debug.Log("Go to calculator");
        RemoveSelectedStyle(computerLocation);
        computerLocation = ComputerLocation.CALCULATOR;
        AddSelectedStyle(computerLocation);
        pcUiManager.OpenCalculator();
    }

    private void GoToMorse()
    {
        Debug.Log("Go to morse");
        RemoveSelectedStyle(computerLocation);
        computerLocation = ComputerLocation.MORSE;
        AddSelectedStyle(computerLocation);
        pcUiManager.OpenMorse();
    }

    private void GoToWire()
    {
        Debug.Log("Go to wire");
        RemoveSelectedStyle(computerLocation);
        computerLocation = ComputerLocation.WIRE;
        AddSelectedStyle(computerLocation);
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
            ComputerLocation.HOME => homeButton,
            ComputerLocation.MANUAL => manualButton,
            ComputerLocation.CALCULATOR => calculatorButton,
            ComputerLocation.MORSE => morseButton,
            ComputerLocation.WIRE => wireButton,
            _ => null,
        };
    }

    public ToolBarElement() { }

}
