using UnityEngine;

/// <summary>
/// Script pour gérer toutes les interactions du joueur dans la scene de desamorcage
/// </summary>
public class BombInteract : BaseInteract
{

    [SerializeField] private float rotationSpeed = 50f;

    [Header("Booleans")]
    [SerializeField] private bool isZoomedOnBomb = false;
    [SerializeField] private bool isZoomedOnModule = false;
    [SerializeField] private bool isRotating = false;

    private Vector2 lastPos;

    private Transform bombTransform = null;
    private Transform moduleTransform = null;

    private int bombLayerMask;
    private int moduleLayerMask;

    protected override void Setup()
    {
        base.Setup();
        bombLayerMask = LayerMask.GetMask("Bomb");
        moduleLayerMask = LayerMask.GetMask("Module");
    }

    protected override void SetupActions()
    {
        base.SetupActions();
        actions.Player.Hold.performed += _ => StartRotate();
        actions.Player.Hold.canceled += _ => StopRotate();
    }

    protected override void UnSetupActions()
    {
        base.UnSetupActions();
        actions.Player.Hold.performed -= _ => StartRotate();
        actions.Player.Hold.canceled -= _ => StopRotate();
    }

    private void StartRotate()
    {
        isRotating = true;
        lastPos = _pos.ReadValue<Vector2>();
    }

    private void StopRotate()
    {
        isRotating = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            if (bombTransform != null && isZoomedOnBomb && !isZoomedOnModule)
            {
                RotateObject();
            }
        }
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.position, zoomPosition) <= .25f)
            {
                isZooming = false;
            }
        }
    }

    protected override void OnTap(Vector2 pos)
    {
        Debug.Log("Tap " + pos);
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (isZoomedOnBomb)
        {
            if (isZoomedOnModule)
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green, 5);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, moduleLayerMask))
                {
                    moduleTransform.GetComponent<Collider>().enabled = false;
                    if (hit.transform.TryGetComponent(out Module module))
                    {
                        module.ModuleInteract(ray);
                    }

                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 5);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, moduleLayerMask))
                {
                    isZoomedOnModule = true;
                    moduleTransform = hit.transform;
                    ZoomOnTransform(moduleTransform);
                }
            }
        }
        else
        {
            //On zoome sur la bombe
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 5);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, bombLayerMask))
            {
                isZoomedOnBomb = true;
                bombTransform = hit.transform;
                bombTransform.GetComponent<Collider>().enabled = false;
                ZoomOnTransform(bombTransform);
            }

        }
    }

    protected override void UnZoom()
    {
        if (isZoomedOnModule)
        {
            isZoomedOnModule = false;
            moduleTransform.GetComponent<Collider>().enabled = true;
            ZoomOnTransform(bombTransform);
        }
        else if (isZoomedOnBomb)
        {
            isZoomedOnBomb = false;
            bombTransform.GetComponent<Collider>().enabled = true;
            ZoomOnTransform(null);
        }
        else
        {
            ZoomOnTransform(null);
        }
    }

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
}
