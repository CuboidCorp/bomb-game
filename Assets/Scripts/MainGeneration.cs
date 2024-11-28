using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MainGeneration : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    [SerializeField] private BombTypes bombType;

    public bool isDebug = false;

    public static MainGeneration Instance;

    private RuleHolder ruleHolder;


    private ModuleType[] modules;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        Random.InitState(seed);

        ruleHolder = new();
    }

    /// <summary>
    /// Set le seed de la game 
    /// </summary>
    /// <param name="seed">Le seed a mettre</param>
    public void SetSeed(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
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

    public VisualElement[] GenerateManuel()
    {
        List<VisualElement> modulesRules = new();
        List<ModuleType> differentModulesList = new();

        VisualTreeAsset[] visualTreeAssets = Resources.LoadAll<VisualTreeAsset>("ManuelModules");

        foreach (ModuleType module in modules)
        {
            if (!differentModulesList.Contains(module))
            {
                differentModulesList.Add(module);
            }
        }

        foreach (ModuleType module in differentModulesList)
        {
            VisualElement visualElement = visualTreeAssets[(int)module].CloneTree();

            switch (module)
            {
                case ModuleType.WIRES:
                    visualElement.Q<WireRulesElement>().Init(ruleHolder.wireRuleGenerator);
                    break;
                default:
                    throw new NotImplementedException();
            }

            modulesRules.Add(visualElement);
        }

        return modulesRules.ToArray();
    }

}
