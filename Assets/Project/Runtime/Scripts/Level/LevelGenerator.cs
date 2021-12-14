using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoSingleton<LevelGenerator>
{

    // public Transform playerTransform;
    public Transform triggerPoint;
    [SerializeField] private float triggerMoveDistance = 20f;

    public List<GameObject> platforms;
    [SerializeField] private int platformCount = 5;

    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float xOffset = 5f;



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
                    ps.InitialDirection = platformDirection;
                    platformDirection *= -1;
                }
            }

            Quaternion targetAngle = Quaternion.identity;

            Instantiate(p, newPosition, targetAngle);
            transform.position = newPosition;

        }
    }
}
