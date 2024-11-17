using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe de base pour tous les modules de bombe
/// </summary>
public abstract class Module : MonoBehaviour
{
    protected ModuleType moduleType;

    [SerializeField] protected Lamp lampScript;

    [HideInInspector] public UnityEvent ModuleSuccess;
    [HideInInspector] public UnityEvent ModuleFail;

    public abstract void SetupModule(RuleHolder rules);

    public abstract void ModuleInteract(Ray rayInteract);
}
