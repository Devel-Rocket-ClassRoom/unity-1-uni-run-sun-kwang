using Unity.VisualScripting;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platformPrefabs;
    [SerializeField]
    private int count = 3;
    [SerializeField]
    private Vector2 spawnTimeRange = new Vector2(1.25f, 2.25f); // x = min, y = max
    [SerializeField]
    private float timeSpawn;
    [SerializeField]
    private Vector2 yRange = new Vector2(-3.5f, 1.5f);

    private float xPos = 15f;

    private GameObject[,] platforms;
    private int platformIndex = 0;
    private int[] currentIndices;
    private float lastSpawnTime;
    private bool start = true;
    //private GameManager gameManager;

    private void Awake()
    {
        platforms = new GameObject[platformPrefabs.Length, count];
        currentIndices = new int[count];
        for (int i = 0; i < platformPrefabs.Length; i++)
        {
            for (int j = 0; j < count; j++)
            {
                platforms[i, j] = Instantiate(platformPrefabs[i]);
                platforms[i, j].SetActive(false);
            }
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

        if (start || Time.time >= lastSpawnTime + timeSpawn)
        {
            lastSpawnTime = Time.time;
            timeSpawn = 0.99f;
            platformIndex = Random.Range(0, platformPrefabs.Length);
            Vector2 pos;

            pos.x = xPos;
            //pos.y = Random.Range(yRange.x, yRange.y);
            pos.y = -4.5f;
            platforms[platformIndex, currentIndices[platformIndex]].transform.position = pos;

            platforms[platformIndex, currentIndices[platformIndex]].SetActive(false);
            platforms[platformIndex, currentIndices[platformIndex]].SetActive(true);
            currentIndices[platformIndex] = (int)Mathf.Repeat(currentIndices[platformIndex] + 1, count);
            start = false;
        }
    }
}
