/// <summary>
/// Liste des conditions que l'on peut calculer � partir du num�ro de s�rie
/// </summary>
public enum SerialNumberConditions
{
    //Check utilis� par tout en vrai
    HAS_CHAR, //Contient X caract�re
    HAS_REPEATING_CHARACTERS, //Est ce qu'il y a des caract�res qui se repete ?
    //Check du max, min ou autre :
    HAS_NUMBER_GREATER_THAN, //A un nombre sup�rieur �
    HAS_NUMBER_LESSER_THAN, //A un nombre inf�rieur �
    HAS_NUMBER_EQUAL_TO, //A un nombre �gal � (HAS_CHAR) camoufl�
    //Somme des nombres : 
    //Somme des nombres moyenne est 12.5 A tester un peu
    HAS_SUM_GREATER_THAN,
    HAS_SUM_LESSER_THAN,
    HAS_SUM_EQUAL_TO,
    //Nb voyelles/consonnes sup�rieure inf�rieure � x:
    //Nb moyen de voyelle 1.67
    HAS_NB_VOWEL_GREATER_THAN,
    HAS_NB_VOWEL_LESSER_THAN,
    HAS_NB_VOWEL_EQUAL_TO,
    //Nb moyen de consonne 5.56
    HAS_NB_CONSONANT_GREATER_THAN,
    HAS_NB_CONSONANT_LESSER_THAN,
    HAS_NB_CONSONANT_EQUAL_TO,
}