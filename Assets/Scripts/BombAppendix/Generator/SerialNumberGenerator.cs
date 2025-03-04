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
    /// Renvoie le nombre de consonnes dans le num�ro de s�rie
    /// </summary>
    /// <returns>Le nombre de consonnes</returns>
    public int GetNbConsonant()
    {
        int nbConsonant = 0;
        for (int i = 0 ; i < serialNumber.Length ; i++)
        {
            if (!IsVoyelle(serialNumber[i]))
            {
                nbConsonant++;
            }
        }

        return nbConsonant;
    }

    /// <summary>
    /// Renvoie le nombre de voyelles dans le num�ro de s�rie
    /// </summary>
    /// <returns>Entier qui repr�sente le nombre de voyelles</returns>
    public int GetNbVowels()
    {
        int nbVowels = 0;
        for (int i = 0 ; i < serialNumber.Length ; i++)
        {
            if (IsVoyelle(serialNumber[i]))
            {
                nbVowels++;
            }
        }

        return nbVowels;
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
    /// Check si un caract�re est une voyelle
    /// </summary>
    /// <param name="c">Le caract�re � check</param>
    /// <returns>True si il y a </returns>
    private bool IsVoyelle(char c)
    {
        return "AEIOUY".Contains(c);
    }


}
