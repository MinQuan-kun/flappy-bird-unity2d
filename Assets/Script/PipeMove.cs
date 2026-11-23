using UnityEngine;

public class PipeMove : MonoBehaviour
{
    [Header("Di chuyển màn hình dọc")]
    public float speed = 3f;
    public float destroyX = -4.5f; // Khuất hẳn mép trái khung 9:16

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}