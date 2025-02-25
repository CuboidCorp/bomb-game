using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    private PlayerControls actions;
    private InputAction _pos;

    private Camera mainCamera;

    [SerializeField] private GameObject optionsMenu;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        actions = new PlayerControls();
        actions.Enable();
        SetupActions();
    }

    private void OnDisable()
    {
        actions.Disable();
        UnSetupActions();
    }

    public void SetupActions()
    {
        _pos = actions.Player.Position;

        actions.Player.Tap.performed += OnTap;
    }

    protected void UnSetupActions()
    {
        actions.Player.Tap.performed -= OnTap;
    }

    private void OnTap(InputAction.CallbackContext _)
    {
        Vector2 pos = _pos.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, 10))
        {
            //On check si on a touché un des boutons du menu ou l'engrenage des options
            if (hit.collider.CompareTag("MainMenuInteractable"))
            {
                switch (hit.collider.name)
                {
                    case "GearOptions":
                        OpenOptions();
                        break;
                    case "PlayButton":
                        PlayGame();
                        break;
                    case "QuitButton":
                        QuitGame();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Quitte le jeu avec un bruit d'explosion
    /// </summary>
    private void QuitGame()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.BOMB_EXPLOSION);

        StartCoroutine(QuitGameAfter1Sec());
    }

    /// <summary>
    /// Quitte le jeu après 1 seconde
    /// </summary>
    /// <returns></returns>
    private IEnumerator QuitGameAfter1Sec()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    /// <summary>
    /// Ouvre le menu des options
    /// </summary>
    private void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }

    /// <summary>
    /// Lance le jeu
    /// </summary>
    private void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

}
