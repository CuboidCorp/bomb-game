using UnityEngine;

public class MainGeneration : MonoBehaviour
{
    [SerializeField] private int seed = 0;

    private void Awake()
    {
        Random.InitState(seed);
    }
}
