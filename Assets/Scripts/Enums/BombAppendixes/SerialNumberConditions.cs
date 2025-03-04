/// <summary>
/// Liste des conditions que l'on peut calculer à partir du numéro de série
/// </summary>
public enum SerialNumberConditions
{
    //Check utilisé par tout en vrai
    HAS_CHAR, //Contient X caractère
    HAS_REPEATING_CHARACTERS, //Est ce qu'il y a des caractères qui se repete ?
    //Check du max, min ou autre :
    HAS_NUMBER_GREATER_THAN, //A un nombre supérieur à
    HAS_NUMBER_LESSER_THAN, //A un nombre inférieur à
    HAS_NUMBER_EQUAL_TO, //A un nombre égal à (HAS_CHAR) camouflé
    //Somme des nombres : 
    //Somme des nombres moyenne est 12.5 A tester un peu
    HAS_SUM_GREATER_THAN,
    HAS_SUM_LESSER_THAN,
    HAS_SUM_EQUAL_TO,
    //Nb voyelles/consonnes supérieure inférieure à x:
    //Nb moyen de voyelle 1.67
    HAS_NB_VOWEL_GREATER_THAN,
    HAS_NB_VOWEL_LESSER_THAN,
    HAS_NB_VOWEL_EQUAL_TO,
    //Nb moyen de consonne 5.56
    HAS_NB_CONSONANT_GREATER_THAN,
    HAS_NB_CONSONANT_LESSER_THAN,
    HAS_NB_CONSONANT_EQUAL_TO,
}