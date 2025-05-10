using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = int.MaxValue)]
public class CharacterData : ScriptableObject
{
    public string playerName;
    public string characterName;
    public float damage;
    public float attackRate;
    public float attackDelay;
    public float attackSpeed;
    public float attackRange;
}
