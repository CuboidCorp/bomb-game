using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour g�rer toutes les interactions du joueur
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
            // Si on touche un objet et que celui-ci est d�j� s�lectionn�
            if (selectedObject == hit.transform)
            {
                // Si d�j� zoom�, on clique dehors pour d�zoomer
                DeselectObject();
            }
            else
            {
                // Sinon, on s�lectionne l'objet et on applique le zoom
                SelectObject(hit.transform);
            }
        }
        else
        {
            // Si on touche en dehors, d�zoomer si d�j� zoom�
            if (isZoomed)
            {
                DeselectObject();
            }
        }
    }

    private void OnHold(Vector2 pos)
    {
        Debug.Log("Hold " + pos);
        // Rotation de l'objet s�lectionn� si un objet est zoom�
        if (selectedObject != null && isZoomed)
        {
            RotateObject();
        }
    }

    private void SelectObject(Transform obj)
    {
        // Enregistrer l'objet s�lectionn�
        selectedObject = obj;
        isZoomed = true;

        // Appliquer le zoom en d�pla�ant la cam�ra vers l'objet
        Vector3 zoomPosition = selectedObject.position + selectedObject.forward * -2f; // Ajustez cette valeur pour un meilleur effet de zoom
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, 0.5f);
    }

    private void DeselectObject()
    {
        // D�zoomer en r�initialisant la cam�ra � sa position d'origine
        mainCamera.transform.position = new Vector3(0, 0, -10); // Position d'origine de la cam�ra, ajustez si n�cessaire
        isZoomed = false;
        selectedObject = null;
    }

    private void RotateObject()
    {
        // Rotation autour de l'axe Y de l'objet s�lectionn�
        float rotationSpeed = 50f;
        float rotation = rotationSpeed * Time.deltaTime;
        selectedObject.Rotate(Vector3.up, rotation, Space.World);
    }
}
