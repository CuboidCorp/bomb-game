using UnityEngine;
using UnityEngine.UIElements;

public class EndMenuManager : MonoBehaviour
{
    public static EndMenuManager Instance;

    private UIDocument doc;

    private void Awake()
    {
        Instance = this;
        doc = GetComponent<UIDocument>();
    }
}
