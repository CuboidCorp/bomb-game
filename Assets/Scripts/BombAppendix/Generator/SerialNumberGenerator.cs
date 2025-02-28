using UnityEngine;

/// <summary>
/// Script de g�n�ration de num�ro de s�rie
/// </summary>
public class SerialNumberGenerator
{
    private string serialNumber;
    private const int SERIAL_NUMBER_LENGTH = 10;
    private const string SERIAL_NUMBER_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private const string PATH_TO_PREFAB = "SerialNumber";

    /// <summary>
    /// G�n�re un num�ro de s�rie al�atoire
    /// </summary>
    public void GenerateSerialNumber()
    {
        serialNumber = string.Empty;

        for (int i = 0; i < SERIAL_NUMBER_LENGTH; i++)
        {
            serialNumber += SERIAL_NUMBER_CHARS[Random.Range(0, SERIAL_NUMBER_CHARS.Length)];
        }
    }

    /// <summary>
    /// Renvoie le num�ro de s�rie
    /// </summary>
    /// <returns>Le num�ro de s�rie</returns>
    public string GetSerialNumber()
    {
        return serialNumber;
    }

    /// <summary>
    /// Renvoie le chemin vers le prefab
    /// </summary>
    /// <returns>Renvoie le chemin vers le prefab</returns>
    public string GetPathToPrefab()
    {
        return PATH_TO_PREFAB;
    }

    /// <summary>
    /// Renvoie la somme des chiffres du num�ro de s�rie
    /// </summary>
    /// <returns>Renvoie la somme des chiffres du num�ro de s�rie</returns>
    public int GetSumOfSerialNumber()
    {
        int sum = 0;

        for (int i = 0; i < serialNumber.Length; i++)
        {
            if (char.IsDigit(serialNumber[i]))
            {
                sum += int.Parse(serialNumber[i].ToString());
            }
        }

        return sum;
    }

    /// <summary>
    /// V�rifie si le num�ro de s�rie contient un caract�re
    /// </summary>
    /// <param name="c">Le caract�re � v�rifier</param>
    /// <returns>True si y a le caract�re, false sinon</returns>
    public bool ContainsChar(char c)
    {
        return serialNumber.Contains(c);
    }

    /// <summary>
    /// V�rifie si il y a une consonne dans le num�ro de s�rie
    /// </summary>
    /// <returns>True si il y en a une, false sinon</returns>
    public bool ContainsConsonne()
    {
        for (int i = 0; i < serialNumber.Length; i++)
        {
            if (!IsVoyelle(serialNumber[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// V�rifie si il y a une voyelle dans le num�ro de s�rie
    /// </summary>
    /// <returns>True si y a en a une, false sinon</returns>
    public bool ContainsVoyelle()
    {
        for (int i = 0; i < serialNumber.Length; i++)
        {
            if (IsVoyelle(serialNumber[i]))
            {
                return true;
            }
        }
        return false;

    }

    /// <summary>
    /// Check si un caract�re est une voyelle
    /// </summary>
    /// <param name="c">Le caract�re � check</param>
    /// <returns>True si il y a </returns>
    private bool IsVoyelle(char c)
    {
        return "AEIOUY".Contains(c);
    }


}
