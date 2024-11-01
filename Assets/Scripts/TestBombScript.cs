using UnityEngine;

/// <summary>
/// Classe pour tester la bombe
/// </summary>
public class TestBombScript : MonoBehaviour
{
    public static TestBombScript Instance;
    public int seed;
    public GameObject bomb;


    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        bomb = Instantiate(Resources.Load<GameObject>("Bomb/6Bomb"), new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 0));
        bomb.GetComponent<Bomb>().SetupBomb(seed);
        bomb.GetComponent<Bomb>().SetupModules();
        bomb.GetComponent<Bomb>().StartBomb();
    }

}
