using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour des fonctions utilitaires generales
/// </summary>
public static class Functions
{
    /// <summary>
    /// Utilisation de l'algorithme de Fisher-Yates pour mélanger une liste
    /// </summary>
    /// <typeparam name="T">Le type des valeurs de la liste</typeparam>
    /// <param name="list">La liste qu'il faut mélanger</param>
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int randomIndex = Random.Range(i, n);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

}
