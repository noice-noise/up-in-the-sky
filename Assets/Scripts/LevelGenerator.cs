﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    public Transform playerTransform;
    public Transform triggerPoint;
    [SerializeField] private float triggerMoveDistance = 20f;

    public GameObject[] platforms;
    [SerializeField] private int platformCount = 5;

    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float xOffset = 5f;

    [SerializeField] private LevelGenData levelGenData;

    [SerializeField] private PlatformStats stillPlatfromStats;
    [SerializeField] private PlatformStats movingPlatfromStats;
    private int platformDirection = 1;

    private void Update() 
    {
        if (playerTransform.position.y > triggerPoint.position.y)
        {
            Generate();
            triggerPoint.position = new Vector3(triggerPoint.position.x, triggerPoint.position.y + triggerMoveDistance, transform.position.z);
        }
    }

    internal PlatformStats GetPlatformStats(PlatformType type)
    {
        switch (type)
        {
            case PlatformType.Still:
                return stillPlatfromStats;
            case PlatformType.Moving:
                return movingPlatfromStats;
            default:
                Debug.LogError("Invalid PlatformType.");
                return null;
        }
    }

    public void Generate()
    {
        Vector3 newPosition = transform.position;

        for (int i = 0; i < platformCount; i++)
        {
            newPosition.y += yOffset;
            newPosition.x = Random.Range(xOffset, -xOffset);

            GameObject p = platforms[Random.Range(0, platforms.Length)];
            var ps = p.GetComponent<Platform>();
            if (ps.Type == PlatformType.Moving)
            {
                // p.GetComponent<Platform>().InitialDirection = platformDirection;
                ps.initialDirection = platformDirection;
                Debug.Log("PD: " + platformDirection);
                platformDirection *= -1;
            }
            
            Instantiate(p, newPosition, Quaternion.identity);
            transform.position = newPosition;

        }
    }
}