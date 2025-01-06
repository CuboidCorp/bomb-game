using UnityEngine;

/// <summary>
/// Classe pour gérer la scene de désamorçage
/// </summary>
public class BombSquad : MonoBehaviour
{
    [SerializeField] private Vector3 bombPos = new(0, 5, 0);
    [SerializeField] private Vector3 bombRot = new(0, 0, 0);

    public static BombSquad Instance;

    public GameObject bomb;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        MainGeneration.Instance.GenerateModules();
        bomb = MainGeneration.Instance.GenerateBomb(bombPos, bombRot);
    }

}
