using UnityEngine;

/// <summary>
/// Classe pour les bombes à 6 slots de modules
/// </summary>
public class SixSlotBomb : Bomb
{
    public override void SetupBomb(int seed)
    {
        base.SetupBomb(seed);
        nbModules = 6;
        modulePositions = new()//Les slots vont de haut a gauche a bas a droite
        {
            new Vector3(1f, .5f, .45f),
            new Vector3(0, .5f, .45f),
            new Vector3(-1f, .5f, .45f),
            new Vector3(1f, -.5f, .45f),
            new Vector3(0, -.5f, .45f),
            new Vector3(-1f, -.5f, .45f)
        };

        modulesGo = new GameObject[nbModules];
    }
}
