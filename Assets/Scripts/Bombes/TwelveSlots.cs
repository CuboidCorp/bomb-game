using UnityEngine;

public class TwelveSlots : Bomb
{
    public override void SetupBomb()
    {
        base.SetupBomb();
        nbModules = 12;
        modulePositions = new()//Les slots vont de haut a gauche a bas a droite
        {
            new Vector3(1f, .5f, .45f),
            new Vector3(0, .5f, .45f),
            new Vector3(-1f, .5f, .45f),
            new Vector3(1f, -.5f, .45f),
            new Vector3(0, -.5f, .45f),
            new Vector3(-1f, -.5f, .45f),
            //Slots de l'autre coté
            new Vector3(1f, .5f, -.45f),
            new Vector3(0, .5f, -.45f),
            new Vector3(-1f, .5f, -.45f),
            new Vector3(1f, -.5f, -.45f),
            new Vector3(0, -.5f, -.45f),
            new Vector3(-1f, -.5f, -.45f)
        };

        modulesGo = new GameObject[nbModules];
    }
}
