using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles;
    public float obstacleRatio = 0.3f;
    //private GameManager gameManager;
    private bool isStepped = false;

    private void Start()
    {
        //gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }


    private void OnEnable()
    {
        foreach (var obstacle in obstacles)
        {
            obstacle.SetActive(Random.value < obstacleRatio);
        }
        isStepped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isStepped)
        {
            isStepped = true;
            //gameManager.AddScore(1);
        }
    }
}
