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

    [SerializeField] private float verticalOffset = 5f;
    [SerializeField] private float horizontalOffset = 5f;

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
            newPosition.y += verticalOffset;
            newPosition.x = Random.Range(horizontalOffset, -horizontalOffset);

            GameObject p = platforms[Random.Range(0, platforms.Length)];

            Instantiate(p, newPosition, Quaternion.identity);
            transform.position = newPosition;
        }
    }
}
