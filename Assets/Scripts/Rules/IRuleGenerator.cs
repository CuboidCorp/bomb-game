/// <summary>
/// Interface qui montre la fonction a implementer.
/// Note : 
/// J'aurais bien aimer rendre le tout plus strict en utilisant des parametres de type g�n�rique mais cela contraint trop les diff�rents besoins
/// Donc pas de <T>
/// </summary>
public interface IRuleGenerator
{
    public void SetupRules();
}