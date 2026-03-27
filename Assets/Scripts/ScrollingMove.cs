using UnityEngine;

public class ScrollingMove : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        // transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World); // 둘이 같음


        // transform.position += transform.right * -1f * speed * Time.deltaTime;
        // transform.localPosition += Vector3.left * speed * Time.deltaTime;
        // transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self); // 셋이 같음
    }
}
