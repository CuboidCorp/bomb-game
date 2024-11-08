using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainGeneration : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    [SerializeField] private BombTypes bombType;

    public static MainGeneration Instance;

    private RuleHolder ruleHolder;


    private ModuleType[] modules;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        Random.InitState(seed);

        ruleHolder = new();
    }

    public void GenerateModules()
    {
        int nbModules = bombType switch
        {
            BombTypes.SIX_SLOTS => 5,
            BombTypes.TWELVE_SLOTS => 11,
            _ => throw new NotImplementedException(),
        };

        modules = new ModuleType[nbModules];
        for (int i = 0; i < nbModules; i++)
        {
            modules[i] = (ModuleType)Random.Range(0, Enum.GetValues(typeof(ModuleType)).Length);
        }

        ruleHolder.Generate(modules);
    }


    public GameObject GenerateBomb(Vector3 position, string resourcesPath)
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>(resourcesPath), position, Quaternion.identity);
        bomb.GetComponent<Bomb>().SetupBomb();
        bomb.GetComponent<Bomb>().SetupModules(modules, ruleHolder);
        bomb.GetComponent<Bomb>().StartBomb();
        return bomb;
    }

    public void GenerateManuel(Vector3 position, string resourcesPath)
    {
        //TODO : Faire la generation du manuel avec les regles
    }
}
