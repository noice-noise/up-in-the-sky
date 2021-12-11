using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{

    // public Transform playerTransform;
    public Transform triggerPoint;
    [SerializeField] private float triggerMoveDistance = 20f;

    public List<GameObject> platforms;
    [SerializeField] private int platformCount = 5;

    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float xOffset = 5f;

    [SerializeField] private PlatformStats stillPlatfromStats;
    [SerializeField] private PlatformStats movingPlatfromStats;
    [SerializeField] private PlatformStats boosterPlatformStats;
    [SerializeField] private PlatformStats fragilePlatformStats;

    private int platformDirection = 1;

    public void HandleLevelGeneration()
    {
        bool isPlayerAboveTriggerPoint = LevelManager.Instance.currentPlayerPosition > triggerPoint.position.y;

        if (isPlayerAboveTriggerPoint)
        {
            Generate();
            MoveTriggerPoint();
        }
    }

    private void MoveTriggerPoint()
    {
        triggerPoint.position = new Vector3(triggerPoint.position.x, triggerPoint.position.y + triggerMoveDistance, transform.position.z);
    }

    public PlatformStats GetPlatformStats(PlatformType type)
    {
        switch (type)
        {
            case PlatformType.Still:
                return stillPlatfromStats;
            case PlatformType.Moving:
                return movingPlatfromStats;
            case PlatformType.Booster:
                return boosterPlatformStats;
            case PlatformType.Fragile:
                return fragilePlatformStats;
            default:
                Debug.LogError("Invalid PlatformType.");
                return null;
        }
    }

    private void Generate()
    {
        Vector3 newPosition = transform.position;

        for (int i = 0; i < platformCount; i++)
        {
            newPosition.y += yOffset;
            newPosition.x = Random.Range(xOffset, -xOffset);

            GameObject p = platforms[Random.Range(0, platforms.Count)];

            if (p.CompareTag("Platform"))
            {
                var ps = p.GetComponent<PlatformController>();
                if (ps.Type == PlatformType.Moving)
                {
                    ps.initialDirection = platformDirection;
                    platformDirection *= -1;
                }
            }

            Quaternion targetAngle = Quaternion.identity;

            // while (targetAngle.z % 90f == 0)
            // {
            //     targetAngle = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            // }

            Instantiate(p, newPosition, targetAngle);
            transform.position = newPosition;

        }
    }
}
