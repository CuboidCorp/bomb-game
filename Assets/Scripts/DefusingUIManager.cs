using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestion de l'UI sur la scène de désamorçage
/// </summary>
public class DefusingUIManager : MonoBehaviour
{
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Button strikeTempButton;

    private void Start()
    {
        strikeTempButton.onClick.AddListener(AddStrike);
    }

    private void AddStrike()
    {
        Debug.Log("Button clicked");
        TestBombScript.Instance.bomb.GetComponent<Bomb>().AddStrike();
    }
}
