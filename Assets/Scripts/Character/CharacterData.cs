using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string playerName;
    public string characterName;
    public string characterId; // �� ĳ���� ���� ID (�߿�)

    public float moveSpeed;

    public float maxHealth;

    public float damage;
    public float attackRate;
    public float attackDelay;
    public float attackSpeed;
    public float attackRange;
    public int attackCount;
    public float attackAngle;
    public float knockbackForce;
    public float attackDuration;
    public float projectileSize;
}
