using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeDatabase", menuName = "ScriptableObjects/ChallengeDatabase")]
public class ChallengeDatabase : ScriptableObject
{
    public List<Challenge> challenges;
}
