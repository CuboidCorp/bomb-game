using TMPro;
using UnityEngine;

/// <summary>
/// Script de render du num�ro de s�rie
/// </summary>
public class SerialNumberRenderer : MonoBehaviour
{
    private string serialNumber;

    [SerializeField] private TMP_Text textUp;
    [SerializeField] private TMP_Text textDown;

    [SerializeField] private Vector3 lonCornerPos;

    [SerializeField] private Vector3 lagCornerPos;

    /// <summary>
    /// G�n�re le num�ro de s�rie et le positionne sur un des c�t�s de la bombe
    /// </summary>
    /// <param name="side">Cot� de la bombe sur lequel le g�n�rer</param>
    /// <param name="bombPos">Position de la bombe</param>
    public void Generate(int side, Vector3 bombPos)
    {
        //0 = Haut, 1 = Bas, 2 = Gauche, 3 = Droite
        Vector3 rotation = new(0, 90, 0);
        Vector3 position = bombPos;

        switch (side)
        {
            case 0:
                position += new Vector3(Random.Range(-lonCornerPos.x, lonCornerPos.x), lonCornerPos.y, Random.Range(-lonCornerPos.z, lonCornerPos.z));
                break;
            case 1:
                rotation.z = 180;
                position += new Vector3(Random.Range(-lonCornerPos.x, lonCornerPos.x), -lonCornerPos.y, Random.Range(-lonCornerPos.z, lonCornerPos.z));
                break;
            case 2:
                rotation.x = -90;
                position += new Vector3(-lagCornerPos.x, Random.Range(-lagCornerPos.y, lagCornerPos.y), Random.Range(-lagCornerPos.z, lagCornerPos.z));
                break;
            case 3:
                rotation.x = 90;
                position += new Vector3(lagCornerPos.x, Random.Range(-lagCornerPos.y, lagCornerPos.y), Random.Range(-lagCornerPos.z, lagCornerPos.z));
                break;

        }
        transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
    }

    /// <summary>
    /// Render le num�ro de s�rie
    /// </summary>
    public void RenderText()
    {
        textUp.text = serialNumber[..5];
        textDown.text = serialNumber[5..];
    }

    /// <summary>
    /// Set le num�ro de s�rie
    /// </summary>
    /// <param name="serialNumber">Le num�ro de s�rie</param>
    public void Setup(RuleHolder rules)
    {
        serialNumber = rules.serialNumberGenerator.GetSerialNumber();
    }
}
