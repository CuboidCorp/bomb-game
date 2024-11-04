using UnityEngine;

/// <summary>
/// Classe de base pour tous les modules de bombe
/// </summary>
public abstract class Module : MonoBehaviour
{
    protected ModuleType moduleType;

    [SerializeField] private Lamp lampScript;

    public abstract void SetupModule();

    public abstract void ModuleInteract(Ray rayInteract);
}
