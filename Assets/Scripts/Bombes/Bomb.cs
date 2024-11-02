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

    public virtual void SetupBomb(int seed)
    {
        modulesPrefabs = Resources.LoadAll<GameObject>("Modules");
        Random.InitState(seed);
        nbStrikes = 0;
    }

    public void SetupModules()
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
            ModuleType moduleType = (ModuleType)Random.Range(0, Enum.GetValues(typeof(ModuleType)).Length);
            Vector3 position = modulePositions[i];
            Debug.Log($"Module n°{i + 1} at pos : {position} type : {moduleType}");
            GameObject modulePrefab = modulesPrefabs[(int)moduleType];
            modulesGo[i] = Instantiate(modulePrefab);
            modulesGo[i].transform.position = position + transform.position;
            modulesGo[i].transform.SetParent(transform);
            modulesGo[i].name = $"Module n°{i + 1}-{moduleType}";
            modulesGo[i].GetComponent<Module>().SetupModule();
        }

    }

    public void StartBomb()
    {
        //Calcul du temps de la bombe
        //TimeSpan time = new(0, 5 * (nbModules / 6), 0);
        TimeSpan time = new(0, 0, 30);
        timerScript.StartTimer(time);
        timerScript.TimerFinished.AddListener(ExplodeBomb);

    }

    public void AddStrike()
    {
        nbStrikes++;
        Debug.Log("Adding strike");
        if (nbStrikes == 3)
        {
            ExplodeBomb();
        }
        timerScript.AddStrike();
    }

    public void ExplodeBomb()
    {
        AudioManager.Instance.PlaySoundEffect(SoundEffects.BOMB_EXPLOSION);
        Debug.Log("BOOM");
    }

}
