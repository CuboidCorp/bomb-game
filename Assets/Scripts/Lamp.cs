using UnityEngine;

/// <summary>
/// Classe pour controler la lampe d'un module de bombe
/// </summary>
public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject lampObject;

    [Header("Materials")]

    [SerializeField] private Material activatedMaterial;
    [SerializeField] private Material deactivatedMaterial;

    private void Awake()
    {
        lampObject.GetComponent<Renderer>().material = deactivatedMaterial;
    }

    public void Activate()
    {
        lampObject.GetComponent<Renderer>().material = activatedMaterial;
    }

}
