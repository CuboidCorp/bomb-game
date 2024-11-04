using UnityEngine;

/// <summary>
/// Classe pour tester la bombe
/// </summary>
public class TestBombScript : MonoBehaviour
{
    public static TestBombScript Instance;
    public int seed;
    public GameObject bomb;

    [SerializeField] private Vector3 bombPosition = new(0, 0, 0);

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        //bomb = Instantiate(Resources.Load<GameObject>("Bomb/6Bomb"), bombPosition, Quaternion.identity);
        //bomb.GetComponent<Bomb>().SetupBomb(seed);
        //bomb.GetComponent<Bomb>().SetupModules();
        //bomb.GetComponent<Bomb>().StartBomb();

        WireRuleGenerator wireRuleGenerator = new();
        wireRuleGenerator.SetupRules();
    }

}
