using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe de base pour tous les modules de bombe
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class Module : MonoBehaviour
{
    protected ModuleType moduleType;

    [SerializeField] protected Lamp lampScript;

    [HideInInspector] public UnityEvent ModuleSuccess;
    [HideInInspector] public UnityEvent ModuleFail;

    /// <summary>
    /// Initialise le module
    /// </summary>
    /// <param name="rules">Les regles pour intialiser le module</param>
    public abstract void SetupModule(RuleHolder rules);

    /// <summary>
    /// Gère l'interaction du module
    /// </summary>
    /// <param name="rayInteract">Le ray qui interagit avec le module</param>
    public abstract void ModuleInteract(Ray rayInteract);

    /// <summary>
    /// Quand on hold le module
    /// </summary>
    public abstract void OnModuleHoldStart(Ray rayInteract);

    /// <summary>
    /// Quand on finit le hold du module
    /// </summary>
    public abstract void OnModuleHoldEnd();

    /// <summary>
    /// Quand le module est réussi
    /// Si override appelé base.Success() après le code spécifique
    /// </summary>
    protected virtual void Success()
    {
        lampScript.Activate();
        ModuleSuccess.Invoke();
        Destroy(this);
    }

    /// <summary>
    /// Quand le module est raté
    /// </summary>
    protected virtual void Fail()
    {
        ModuleFail.Invoke();
    }
}
