using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    public float speed = 3f; // Khớp với tốc độ của PipeMove
    public float resetPositionX = -5.6f; // Mốc X khi mảnh đất trôi hết sang trái
    public float startPositionX = 5.6f;  // Vị trí X đưa mảnh đất quay lại bên phải

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= resetPositionX)
        {
            Vector3 pos = transform.position;
            pos.x = startPositionX;
            transform.position = pos;
        }
    }
}