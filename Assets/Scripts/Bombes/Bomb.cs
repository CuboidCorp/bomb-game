using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Classe de base pour tous les types de bombes
/// </summary>
public abstract class Bomb : MonoBehaviour
{
    private GameObject[] modulesPrefabs;

    [SerializeField] private BombTypes type;
    protected List<Vector3> modulePositions;

    protected int nbModules;
    private int nbModulesToDefuse;

    protected Timer timerScript;

    protected int nbStrikes;

    public bool isBombDefused = false;
    public bool isBombExploded = false;

    /// <summary>
    /// Modules de la bombe (timer inclus)
    /// </summary>
    protected GameObject[] modulesGo;
    protected int nbModulesFinished;

    private const int MAX_STRIKES = 3;

    private bool isTutorial = false;

    /// <summary>
    /// Initialisation des variables nécessaire pour la bombe
    /// </summary>
    public virtual void SetupBomb()
    {
        modulesPrefabs = Resources.LoadAll<GameObject>("Modules");
        nbModulesFinished = 0;
        nbStrikes = 0;
    }

    /// <summary>
    /// Instancie et configure les différents modules de la bombe
    /// </summary>
    /// <param name="modules">L'array qui stocke tous les modules de la bombe</param>
    /// <param name="rules">L'objet qui contient les générateur de regles pour les différents modules</param>
    public void SetupModules(ModuleType[] modules, RuleHolder rules)
    {
        nbModulesToDefuse = nbModules - 1;
        //On place le timer dans un des slots random
        int timerSlot = Random.Range(0, nbModules);
        Vector3 pos = modulePositions[timerSlot];
        modulePositions.Remove(pos);
        modulesGo[0] = Instantiate(Resources.Load<GameObject>("Timer"));
        modulesGo[0].transform.position = pos + transform.position;
        modulesGo[0].transform.SetParent(transform);
        if (pos.z < 0)
        {
            modulesGo[0].transform.Rotate(0, 180, 0);
        }
        modulesGo[0].name = "Module n°1-Timer";
        timerScript = modulesGo[0].GetComponent<Timer>();

        for (int i = 1; i < nbModules; i++)
        {
            Module mod;
            ModuleType moduleType = modules[i - 1];
            Vector3 position = modulePositions[i - 1];
            GameObject modulePrefab = modulesPrefabs[(int)moduleType];
            modulesGo[i] = Instantiate(modulePrefab);
            modulesGo[i].transform.position = position + transform.position;
            if (position.z < 0)
            {
                if (modulesGo[i].TryGetComponent(out mod))
                {
                    modulesGo[i].transform.position -= mod.GetOffset();
                }
                modulesGo[i].transform.Rotate(0, 180, 0);
            }
            else
            {
                if (modulesGo[i].TryGetComponent(out mod))
                {
                    modulesGo[i].transform.position += mod.GetOffset();
                }
            }
            modulesGo[i].transform.SetParent(transform);
            modulesGo[i].name = $"Module n°{i + 1}-{moduleType}";

            if (modulesGo[i].TryGetComponent(out mod))
            {
                mod.SetupModule(rules);
                mod.ModuleFail.AddListener(AddStrike);
                mod.ModuleSuccess.AddListener(ModuleSuccess);
            }
            else if (moduleType == ModuleType.EMPTY)
            {
                nbModulesToDefuse--;
            }
        }

    }

    /// <summary>
    /// Setup les annexes
    /// </summary>
    /// <param name="rules">Le generateur des regles</param>
    public void SetupAppendixes(RuleHolder rules)
    {
        string prefabPath = "BombAppendixes/";

        string currentPrefabPath = prefabPath + rules.serialNumberGenerator.GetPathToPrefab();

        GameObject serialNumberGo = Instantiate(Resources.Load<GameObject>(currentPrefabPath));
        SerialNumberRenderer snRender = serialNumberGo.GetComponent<SerialNumberRenderer>();
        snRender.Setup(rules);
        snRender.Generate(Random.Range(0, 4), transform.position);
        snRender.RenderText();
        serialNumberGo.transform.SetParent(transform);

    }

    /// <summary>
    /// Lance la bombe en commençant le timer et en ajoutant l'evenement d'explosion a la fin du timer
    /// </summary>
    public void StartBomb()
    {
        //TODO : Tester et trouver une bonne valeur pour le temps de la bombe
        TimeSpan time = new(0, 5 * (nbModules / 6), 0); //TODO : Nouvelle version de calcul du temps  (Le nb de modules ne sera pas multiple de 6)
        timerScript.StartTimer(time);
        timerScript.TimerFinished.AddListener(ExplodeBomb);
    }

    /// <summary>
    /// Ajoute une erreur à la bombe, ce qui accelere le timer et fait exploser la bombe au bout de 3 erreurs
    /// </summary>
    public void AddStrike()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.MODULE_FAIL);
        nbStrikes++;
        Debug.Log("Adding strike");
        if (nbStrikes == MAX_STRIKES)
        {
            ExplodeBomb();
        }
        else
        {
            timerScript.AddStrike();
        }
    }

    /// <summary>
    /// Desamorce un module, et la bombe si tous les modules sont desamorcés
    /// </summary>
    public void ModuleSuccess()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.MODULE_SUCCESS);
        nbModulesFinished++;
        if (nbModulesFinished == nbModulesToDefuse)
        {
            BombSuccess();
        }
    }

    /// <summary>
    /// Methode appelée lorsque la bombe explose, detruit les modules et le timer pour eviter les erreurs
    /// </summary>
    public void ExplodeBomb()
    {
        //TODO : Afficher explosion
        timerScript.StopTimer();
        foreach (GameObject module in modulesGo)
        {
            if (module != null)
            {
                Destroy(module.GetComponent<Module>());
            }
        }
        AudioManager.Instance.PlaySoundEffect(SoundEffects.BOMB_EXPLOSION);
        Debug.Log("BOOM");
        isBombExploded = true;
        if (!isTutorial)
        {
            EndMenuManager.Instance.OpenEndMenu(timerScript.GetTimeLeft(), nbStrikes, false);
        }

    }

    /// <summary>
    /// Methode appelée lorsque la bombe est desamorcée
    /// </summary>
    private void BombSuccess()
    {
        //TODO : Afficher confettis
        timerScript.StopTimer();
        isBombDefused = true;
        if (!isTutorial)
        {
            EndMenuManager.Instance.OpenEndMenu(timerScript.GetTimeLeft(), nbStrikes, true);
        }

    }

    /// <summary>
    /// Retourne le nombre d'erreurs de la bombe
    /// </summary>
    /// <returns>Le nombre de strikes sur la bombe</returns>
    public int GetNbStrikes()
    {
        return nbStrikes;
    }

    #region Tutoriel
    /// <summary>
    /// Setup la bombe du tutoriel
    /// </summary>
    public void SetupTutorialBomb()
    {
        isTutorial = true;

        TimeSpan time = new(0, 3 * (nbModules / 6), 0);
        timerScript.SetupTimer(time);
        foreach (GameObject module in modulesGo)
        {
            if (module.TryGetComponent(out Collider col))
            {
                col.enabled = false;
            }
        }
    }

    /// <summary>
    /// Active les colliders des modules pour le tutoriel
    /// </summary>
    public void EnableCollider()
    {
        foreach (GameObject module in modulesGo)
        {
            if (module.TryGetComponent(out Collider col))
            {
                col.enabled = true;
            }
        }
    }

    /// <summary>
    /// Desactive les colliders des modules pour le tutoriel
    /// </summary>
    public void DisableCollider()
    {
        foreach (GameObject module in modulesGo)
        {
            if (module.TryGetComponent(out Collider col))
            {
                col.enabled = false;
            }
        }
    }

    /// <summary>
    /// Lance la bombe pour le tutoriel
    /// </summary>
    public void StartTutorialBomb()
    {
        isBombExploded = false;
        isBombDefused = false;
        TimeSpan time = new(0, 3 * (nbModules / 6), 0);
        timerScript.SetupTimer(time);
        timerScript.LaunchTimer();
    }

    #endregion

}
