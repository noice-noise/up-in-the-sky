using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;   // the singleton instance variable

    public Transform player;
    public Transform deadzone;
    public Transform particleSystems;
    [SerializeField] private float distanceRequirement = 20f;
    [SerializeField] private float moveSpeed = 10f;

    private void Update()
    {
        if ((player.position.y - deadzone.position.y) > distanceRequirement)
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        deadzone.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        particleSystems.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
    }

    public static GameManager Instance 
    { 
        get 
        {
            if (instance == null)
            {
                instance = new GameManager();
            } 
            
            return instance;
        }
    }

}
