using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur, a extendre pour plus d'options
/// </summary>
public abstract class BaseInteract : MonoBehaviour
{
    protected PlayerControls actions;
    protected InputAction _pos;

    protected bool isZooming = false;

    [Header("Settings")]

    [SerializeField] protected float zoomSpeed = 1.5f;
    protected const float rayDistance = 10;

    protected Camera mainCamera;
    [SerializeField] protected Vector3 mainCameraBasePosition;
    protected Vector3 zoomPosition;

    private Coroutine pinchDetectCoroutine;


    private void Awake()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        actions = new PlayerControls();

        mainCamera = Camera.main;
        mainCamera.transform.position = mainCameraBasePosition;
    }

    protected virtual void SetupActions()
    {
        _pos = actions.Player.Position;

        actions.Player.Tap.performed += _ => OnTap(_pos.ReadValue<Vector2>());

        actions.Player.Return.performed += _ => UnZoom();
        actions.Player.SecondFingerContact.started += _ => PinchDetectStart();
        actions.Player.SecondFingerContact.canceled += _ => PinchDetectEnd();
    }

    /// <summary>
    /// Surement inutile car le unsub d'un lambda anonyme n'existe pas, faudrait mettre dans des actions nommées
    /// Mais c'est pour garder l'idée qu'il faut unsub des events
    /// Pas nécessaire dans ce cas car le script est tjrs activé
    /// </summary>
    protected virtual void UnSetupActions()
    {
        actions.Player.Tap.performed -= _ => OnTap(_pos.ReadValue<Vector2>());

        actions.Player.Return.performed -= _ => UnZoom();
        actions.Player.SecondFingerContact.started -= _ => PinchDetectStart();
        actions.Player.SecondFingerContact.canceled -= _ => PinchDetectEnd();
    }

    private void OnEnable()
    {
        actions.Enable();
        SetupActions();
    }

    private void OnDisable()
    {
        actions.Disable();
        UnSetupActions();
        PinchDetectEnd();
    }


    private void Update()
    {
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.position, zoomPosition) <= .25f)
            {
                isZooming = false;
            }
        }
    }

    protected abstract void OnTap(Vector2 pos);

    private void PinchDetectStart()
    {
        pinchDetectCoroutine = StartCoroutine(PinchDetection());
    }

    private void PinchDetectEnd()
    {
        if (pinchDetectCoroutine != null)
            StopCoroutine(pinchDetectCoroutine);
    }

    private IEnumerator PinchDetection()
    {
        float previousDistance = 0f, distance;
        while (true)
        {
            distance = Vector2.Distance(actions.Player.Position.ReadValue<Vector2>(), actions.Player.SecondFingerPosition.ReadValue<Vector2>());

            if (distance > previousDistance)
            {
                UnZoom();
            }
            previousDistance = distance;
        }
    }

    protected abstract void UnZoom();

    /// <summary>
    /// Zoomme vers un peu en face de l'objet donné
    /// </summary>
    /// <param name="targetTransform">Le transform qu'on veut regarder en face</param>
    protected void ZoomOnTransform(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            zoomPosition = targetTransform.position + targetTransform.forward * 2f; // Ajustez cette valeur pour un meilleur effet de zoom}
        }
        else
        {
            zoomPosition = mainCameraBasePosition;
        }
        isZooming = true;
    }

    /// <summary>
    /// Fait que la camera zoome jusqu'à la position donnée
    /// </summary>
    /// <param name="pos">La position </param>
    protected void ZoomTo(Vector3 pos)
    {
        isZooming = true;
        zoomPosition = pos;
    }

}
