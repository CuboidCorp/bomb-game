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

    protected Timer timerScript;

    protected int nbStrikes;


    /// <summary>
    /// Modules de la bombe (timer inclus)
    /// </summary>
    protected GameObject[] modulesGo;
    protected int nbModulesFinished;

    public virtual void SetupBomb()
    {
        modulesPrefabs = Resources.LoadAll<GameObject>("Modules");
        nbModulesFinished = 0;
        nbStrikes = 0;
    }

    public void SetupModules(ModuleType[] modules, RuleHolder rules)
    {
        //On place le timer dans un des slots random
        int timerSlot = Random.Range(0, nbModules);
        Vector3 pos = modulePositions[timerSlot];
        modulePositions.Remove(pos);
        modulesGo[0] = Instantiate(Resources.Load<GameObject>("Timer"));
        modulesGo[0].transform.position = pos + transform.position;
        modulesGo[0].transform.SetParent(transform);
        modulesGo[0].name = "Timer";
        timerScript = modulesGo[0].GetComponent<Timer>();

        for (int i = 0; i < nbModules - 1; i++)
        {
            ModuleType moduleType = modules[i];
            Vector3 position = modulePositions[i];
            GameObject modulePrefab = modulesPrefabs[(int)moduleType];
            modulesGo[i] = Instantiate(modulePrefab);
            modulesGo[i].transform.position = position + transform.position;
            modulesGo[i].transform.SetParent(transform);
            modulesGo[i].name = $"Module n°{i + 1}-{moduleType}";
            modulesGo[i].GetComponent<Module>().SetupModule(rules);
            modulesGo[i].GetComponent<Module>().ModuleFail.AddListener(AddStrike);
            modulesGo[i].GetComponent<Module>().ModuleSuccess.AddListener(ModuleSuccess);

        }

    }

    public void StartBomb()
    {
        //Calcul du temps de la bombe
        TimeSpan time = new(0, 3 * (nbModules / 6), 0);
        timerScript.StartTimer(time);
        timerScript.TimerFinished.AddListener(ExplodeBomb);

    }

    public void AddStrike()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.MODULE_FAIL);
        nbStrikes++;
        Debug.Log("Adding strike");
        if (nbStrikes == 3)
        {
            ExplodeBomb();
        }
        else
        {
            timerScript.AddStrike();
        }
    }

    public void ModuleSuccess()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.MODULE_SUCCESS);
        nbModulesFinished++;
        if (nbModulesFinished == nbModules - 1)
        {
            BombSuccess();
        }
    }


    public void ExplodeBomb()
    {
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
    }

    private void BombSuccess()
    {
        timerScript.StopTimer();
        Debug.Log("GG");
    }

}
