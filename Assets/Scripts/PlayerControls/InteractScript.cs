using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur
/// </summary>
public class InteractScript : MonoBehaviour
{
    private PlayerControls actions;
    private Transform selectedObject;
    private bool isZoomed = false;
    private Camera mainCamera;

    private void Awake()
    {
        actions = new PlayerControls();
        actions.Enable();
        mainCamera = Camera.main;

        actions.Player.Tap.performed += ctx => OnTap(ctx.ReadValue<Vector2>());
        actions.Player.Hold.performed += ctx => OnHold(ctx.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void OnTap(Vector2 pos)
    {
        Debug.Log("Tap " + pos);
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Si on touche un objet et que celui-ci est déjà sélectionné
            if (selectedObject == hit.transform)
            {
                // Si déjà zoomé, on clique dehors pour dézoomer
                DeselectObject();
            }
            else
            {
                // Sinon, on sélectionne l'objet et on applique le zoom
                SelectObject(hit.transform);
            }
        }
        else
        {
            // Si on touche en dehors, dézoomer si déjà zoomé
            if (isZoomed)
            {
                DeselectObject();
            }
        }
    }

    private void OnHold(Vector2 pos)
    {
        Debug.Log("Hold " + pos);
        // Rotation de l'objet sélectionné si un objet est zoomé
        if (selectedObject != null && isZoomed)
        {
            RotateObject();
        }
    }

    private void SelectObject(Transform obj)
    {
        // Enregistrer l'objet sélectionné
        selectedObject = obj;
        isZoomed = true;

        // Appliquer le zoom en déplaçant la caméra vers l'objet
        Vector3 zoomPosition = selectedObject.position + selectedObject.forward * -2f; // Ajustez cette valeur pour un meilleur effet de zoom
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, 0.5f);
    }

    private void DeselectObject()
    {
        // Dézoomer en réinitialisant la caméra à sa position d'origine
        mainCamera.transform.position = new Vector3(0, 0, -10); // Position d'origine de la caméra, ajustez si nécessaire
        isZoomed = false;
        selectedObject = null;
    }

    private void RotateObject()
    {
        // Rotation autour de l'axe Y de l'objet sélectionné
        float rotationSpeed = 50f;
        float rotation = rotationSpeed * Time.deltaTime;
        selectedObject.Rotate(Vector3.up, rotation, Space.World);
    }
}
