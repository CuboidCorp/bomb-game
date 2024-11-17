public static class TextFR
{
    #region Introduction

    public const string INTRO_TITLE = "Introduction";
    public const string INTRO_DESC_1 = "Votre objectif en tant qu'op�rateur est d'aider l'agent sur le terrain � d�samorcer la bombe. La bombe en face de l'agent aura plusieurs modules qu'il faut desactiver un � un, un module est desactiv� quand la diode du module est verte.";
    public const string INTRO_DESC_2 = "Pour desactiver un module l'agent devra vous aider � identifier le type de module, parmi les modules pr�sents dans ce manuel. Il faudra alors vous r�f�rer aux r�gles sp�cifiques au module pour le desactiver.";

    #endregion

    #region WireRule
    public const string WIRE_RULE_TITLE = "R�gles des fils";
    public const string WIRE_RULE_DESC = "Ce module est compos� de {nb_wires_min} � {nb_wires_max} fils. L'objectif est de trouver la premi�re condition qui est remplie et d'effectuer l'action associ�e. Les conditions se lisent de haut en bas.";
    #endregion

    #region LabyRule
    public const string LABY_RULE_TITLE = "R�gles du labyrinthe";
    public const string LABY_RULE_DESC = "Ce module est compos� d'un labyrinthe de {nb_laby}x{nb_laby} cases. L'objectif est d'aider l'agent � d�placer le curseur du labyrithe du d�but du labyrinthe (Case verte) vers la fin du labyrinthe (Case bleue) sans passer par les murs que l'agent ne peut pas voir'";
    #endregion
}
