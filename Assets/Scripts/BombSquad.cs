using UnityEngine;

/// <summary>
/// Classe pour gérer la scene de désamorçage
/// </summary>
public class BombSquad : MonoBehaviour
{
    [SerializeField] private Vector3 bombPos = new(0, 5, 0);

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
        Application.runInBackground = true;
        MainGeneration.Instance.GenerateModules(); //TODO : A enlever, ce sera mis lors de la generation du seed depuis le menu principal
        bomb = MainGeneration.Instance.GenerateBomb(bombPos, "Bomb/6Bomb");
    }

}
