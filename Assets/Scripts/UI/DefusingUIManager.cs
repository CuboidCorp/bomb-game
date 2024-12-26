using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestion de l'UI sur la scène de désamorçage
/// </summary>
public class DefusingUIManager : MonoBehaviour
{
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        pauseMenuButton.onClick.AddListener(OnPauseMenuButtonClicked);
    }

    private void OnPauseMenuButtonClicked()
    {
        Debug.Log("Pause menu button clicked");
    }
}
