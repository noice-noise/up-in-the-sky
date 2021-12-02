using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jumping", menuName = "Player/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    public float primaryJump;
    public float secondaryJump;
}
