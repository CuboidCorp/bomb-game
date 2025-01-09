using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur dans la scene de desamorcage
/// </summary>
public class BombInteract : MonoBehaviour
{


    private PlayerControls actions;
    private InputAction _pos;

    private Camera mainCamera;

    [Header("Positions")]
    [SerializeField] private Vector3 bombTargetPosition;
    [SerializeField] private Vector3 bombTargetRotation;

    [Header("Settings")]
    [SerializeField] private float interactDistance = 10f;
    [SerializeField] private float grabSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Booleans")]
    [SerializeField] private bool isGrabbingBomb = false;
    [SerializeField] private bool isBombGrabbed = false;
    [SerializeField] private bool isRotating = false;

    private Vector2 lastPos;

    private Transform bombTransform = null;

    private Module targetModule = null;

    private int bombLayerMask;
    private int moduleLayerMask;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        bombLayerMask = LayerMask.GetMask("Bomb");
        moduleLayerMask = LayerMask.GetMask("Module");
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
        actions.Player.Disable();
        actions.UI.Disable();
        UnSetupActions();
    }

    private void SetupActions()
    {
        _pos = actions.Player.Position;

        actions.Player.Press.started += PressStartModule;
        actions.Player.Press.canceled += PressEndModule;

        actions.Player.Rotate.started += StartRotate;
        actions.Player.Rotate.canceled += StopRotate;
    }

    private void UnSetupActions()
    {
        actions.Player.Press.started -= PressStartModule;
        actions.Player.Press.canceled -= PressEndModule;


        actions.Player.Rotate.started -= StartRotate;
        actions.Player.Rotate.canceled -= StopRotate;
    }

    private void Update()
    {
        if (isRotating)
        {
            if (isBombGrabbed && bombTransform != null)
            {
                RotateObject();
            }
        }
        if (isGrabbingBomb)
        {
            GrabBomb();
        }
    }

    private void PressStartModule(InputAction.CallbackContext context)
    {
        //On fait un ray pr tester la position
        Ray ray = mainCamera.ScreenPointToRay(_pos.ReadValue<Vector2>());

        if (isBombGrabbed == false)
        {
            //On check sur le bomblayermask si y a qqch on grab
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, bombLayerMask))
            {
                bombTransform = hit.transform;
                isGrabbingBomb = true;
            }
        }
        else
        {
            if (isGrabbingBomb)
            {
                return;
            }
            //On check sur le modulelayermask si y a qqch on hold
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, moduleLayerMask))
            {
                if (hit.transform.TryGetComponent(out targetModule))
                {
                    targetModule = hit.transform.GetComponent<Module>();
                    targetModule.OnModuleHoldStart(ray);
                }

            }
        }

    }

    private void PressEndModule(InputAction.CallbackContext context)
    {
        if (!isBombGrabbed || isGrabbingBomb)
        {
            return;
        }
        Ray ray = mainCamera.ScreenPointToRay(_pos.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, moduleLayerMask))
        {
            if (hit.transform.TryGetComponent(out targetModule))
            {
                targetModule.OnModuleHoldEnd();
                targetModule.ModuleInteract(ray);
                targetModule = null;
            }

        }
    }

    /// <summary>
    /// Attrape la bombe et la rapproche
    /// </summary>
    private void GrabBomb()
    {
        if (Vector3.Distance(bombTransform.position, bombTargetPosition) > 0.1f)
        {
            bombTransform.SetPositionAndRotation(Vector3.Lerp(bombTransform.position, bombTargetPosition, grabSpeed * Time.deltaTime), Quaternion.Lerp(bombTransform.rotation, Quaternion.Euler(bombTargetRotation), grabSpeed * Time.deltaTime));
        }
        else
        {
            bombTransform.GetComponent<Collider>().enabled = false;
            isGrabbingBomb = false;
            isBombGrabbed = true;
        }
    }

    #region Rotation
    /// <summary>
    /// Commencer la rotation
    /// </summary>
    /// <param name="ctx">Inutile</param>
    private void StartRotate(InputAction.CallbackContext ctx)
    {
        isRotating = true;
        lastPos = _pos.ReadValue<Vector2>();
    }

    /// <summary>
    /// Stop la rotation
    /// </summary>
    /// <param name="ctx">Inutile</param>
    private void StopRotate(InputAction.CallbackContext ctx)
    {
        isRotating = false;
    }

    /// <summary>
    /// Fait tourner la bombe
    /// </summary>
    private void RotateObject()
    {
        Vector2 currentPos = _pos.ReadValue<Vector2>();
        Vector2 deltaPos = currentPos - lastPos; // Calcul de la différence de position
        float rotationY;
        if (Vector3.Dot(bombTransform.up, Vector3.up) >= 0)
        {
            rotationY = Vector3.Dot(deltaPos, mainCamera.transform.right) * rotationSpeed * Time.deltaTime;
        }
        else
        {
            rotationY = -Vector3.Dot(deltaPos, mainCamera.transform.right) * rotationSpeed * Time.deltaTime;
        }

        // Rotation autour de l'axe Y (horizontal) et X (vertical)
        float rotationX = Vector3.Dot(deltaPos, mainCamera.transform.up) * rotationSpeed * Time.deltaTime;

        // Appliquer la rotation
        bombTransform.Rotate(bombTransform.up, rotationY, Space.World);
        bombTransform.Rotate(mainCamera.transform.right, rotationX, Space.World);

        lastPos = currentPos; // Mettre à jour la dernière position
    }

    #endregion
}
