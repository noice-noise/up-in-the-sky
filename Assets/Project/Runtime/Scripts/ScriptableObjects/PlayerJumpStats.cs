using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpStats", menuName = "Player/PlayerJumpStats", order = 0)]
public class PlayerJumpStats : ScriptableObject
{
    public float primaryJump;
    public float secondaryJump;
}
