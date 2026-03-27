using Unity.VisualScripting;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject platformPrefab;
    [SerializeField]
    private int count = 3;
    [SerializeField]
    private Vector2 spawnTimeRange = new Vector2(1.25f, 2.25f); // x = min, y = max
    [SerializeField]
    private float timeSpawn;
    [SerializeField]
    private Vector2 yRange = new Vector2(-3.5f, 1.5f);

    private float xPos = 20f;

    private GameObject[] platforms;
    private int currentIndex = 0;
    private float lastSpawnTime;
    //private GameManager gameManager;

    private void Awake()
    {
        platforms = new GameObject[count];
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i] = Instantiate(platformPrefab);
            platforms[i].SetActive(false);
        }
    }

    private void Start()
    {
        //gameManager = GameObject.FindWithTag("GameController")?.GetComponent<GameManager>();
        lastSpawnTime = 0f;
        timeSpawn = 0f;
    }

    private void Update()
    {
        //if (gameManager.IsGameOver)
        //    return;

        if (Time.time >= lastSpawnTime + timeSpawn)
        {
            lastSpawnTime = Time.time;
            timeSpawn = Random.Range(spawnTimeRange.x, spawnTimeRange.y);

            Vector2 pos;

            pos.x = xPos;
            pos.y = Random.Range(yRange.x, yRange.y);

            platforms[currentIndex].transform.position = pos;

            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);
            currentIndex = (int)Mathf.Repeat(currentIndex + 1, platforms.Length);
        }
    }
}
