using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour des fonctions utilitaires generales
/// </summary>
public static class Functions
{
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int randomIndex = Random.Range(i, n);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
