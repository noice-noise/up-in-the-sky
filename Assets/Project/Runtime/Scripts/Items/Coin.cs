using UnityEngine;

public class Coin : MonoBehaviour
{ 
    [SerializeField] private GameObject prefab;
    [SerializeField] private int value;


    private void OnTriggerEnter(Collider other)
    {
        if (other. CompareTag("Player Ground Check"))
        {
            Debug.Log("Coin");
            ScoreManager.Instance.AddScore(value);
            Destroy(gameObject, 1f);
        }
    }

}