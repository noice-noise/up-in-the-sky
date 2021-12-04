using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Transform playerTransform;
    public Transform triggerPoint;
    [SerializeField] private float triggerMoveDistance = 20f;

    public GameObject[] platforms;
    [SerializeField] private int platformCount = 5;

    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float xOffset = 5f;

    [SerializeField] private LevelGenData levelGenData;

    private void Update() 
    {

        if (playerTransform.position.y > triggerPoint.position.y)
        {
            Generate();
            triggerPoint.position = new Vector3(triggerPoint.position.x, triggerPoint.position.y + triggerMoveDistance, transform.position.z);
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

            Instantiate(p, newPosition, Quaternion.identity);
            transform.position = newPosition;
        }
    }
}
