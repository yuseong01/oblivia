using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string playerName;
    public string characterName;
    public float damage;
    public float attackRate;
    public float attackDelay;
    public float attackSpeed;
    public float attackRange;
    public string characterId; // 각 캐릭터 고유 ID (중요)
}
