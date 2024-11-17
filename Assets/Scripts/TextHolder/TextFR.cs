public static class TextFR
{
    #region Introduction

    public const string INTRO_TITLE = "Introduction";
    public const string INTRO_DESC_1 = "Votre objectif en tant qu'opérateur est d'aider l'agent sur le terrain à désamorcer la bombe. La bombe en face de l'agent aura plusieurs modules qu'il faut desactiver un à un, un module est desactivé quand la diode du module est verte.";
    public const string INTRO_DESC_2 = "Pour desactiver un module l'agent devra vous aider à identifier le type de module, parmi les modules présents dans ce manuel. Il faudra alors vous référer aux règles spécifiques au module pour le desactiver.";

    #endregion

    #region WireRule
    public const string WIRE_RULE_TITLE = "Règles des fils";
    public const string WIRE_RULE_DESC = "Ce module est composé de {nb_wires_min} à {nb_wires_max} fils. L'objectif est de trouver la première condition qui est remplie et d'effectuer l'action associée. Les conditions se lisent de haut en bas.";
    #endregion

    #region LabyRule
    public const string LABY_RULE_TITLE = "Règles du labyrinthe";
    public const string LABY_RULE_DESC = "Ce module est composé d'un labyrinthe de {nb_laby}x{nb_laby} cases. L'objectif est d'aider l'agent à déplacer le curseur du labyrithe du début du labyrinthe (Case verte) vers la fin du labyrinthe (Case bleue) sans passer par les murs que l'agent ne peut pas voir'";
    #endregion
}
