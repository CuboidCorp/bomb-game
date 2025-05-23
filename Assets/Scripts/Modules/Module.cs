using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Classe de base pour tous les modules de bombe
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class Module : MonoBehaviour
{
    protected ModuleType moduleType;

    [SerializeField] protected Lamp lampScript;
    [SerializeField] private Vector3 moduleOffset = new(0, 0, 0);

    [HideInInspector] public UnityEvent ModuleSuccess;
    [HideInInspector] public UnityEvent ModuleFail;

    /// <summary>
    /// Retourne l'offset du module
    /// </summary>
    /// <returns>L'offset du module</returns>
    public Vector3 GetOffset()
    {
        return moduleOffset;
    }

    /// <summary>
    /// Initialise le module
    /// </summary>
    /// <param name="rules">Les regles pour intialiser le module</param>
    public abstract void SetupModule(RuleHolder rules);

    /// <summary>
    /// G�re l'interaction du module
    /// </summary>
    /// <param name="rayInteract">Le ray qui interagit avec le module</param>
    public abstract void ModuleInteract(Ray rayInteract);

    /// <summary>
    /// Quand on hold le module
    /// </summary>
    public abstract void OnModuleHoldStart(Ray rayInteract, InputAction pos);

    /// <summary>
    /// Quand on finit le hold du module
    /// </summary>
    public abstract void OnModuleHoldEnd();

    /// <summary>
    /// Quand le module est r�ussi
    /// Si override appel� base.Success() apr�s le code sp�cifique
    /// </summary>
    protected virtual void Success()
    {
        lampScript.Activate();
        ModuleSuccess.Invoke();
        Destroy(this);
    }

    /// <summary>
    /// Quand le module est rat�
    /// </summary>
    protected virtual void Fail()
    {
        ModuleFail.Invoke();
    }
}
