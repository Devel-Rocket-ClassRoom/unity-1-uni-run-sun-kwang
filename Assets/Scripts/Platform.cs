using System.Threading;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject[] items;
    public float obstacleRatio = 0.3f;
    //private GameManager gameManager;
    private bool isStepped = false;

    private void Start()
    {
        //gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        if (obstacles != null)
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.SetActive(Random.value < obstacleRatio);
            }
        }
        foreach (var item in items)
        {
            //item.transform.localPosition = item.GetComponent<Item>().orgPos;
            item.SetActive(true);
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
