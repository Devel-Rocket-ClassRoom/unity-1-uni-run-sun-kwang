using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject[] items;
    public GameObject[] powerItems;
    public float obstacleRatio = 0.3f;
    public float powerItemRatio = 0.1f;

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
        if (powerItems != null)
        {
            for (int i = 0; i < powerItems.Length; i++)
            {
                powerItems[i].SetActive(false);
            }
            if (Random.value < powerItemRatio)
            {
                int index = Random.Range(0, powerItems.Length);
                powerItems[index].SetActive(true);
            }
        }
    }
}
