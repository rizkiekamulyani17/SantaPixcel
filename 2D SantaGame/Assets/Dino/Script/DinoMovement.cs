using UnityEngine;

public class DinoMovement : MonoBehaviour
{
    public float speed = 2f;             // Kecepatan Dino (bisa diatur beda dari Badak)
    public float patrolDistance = 5f;    // Jarak patrol Dino (misal lebih jauh)

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x > startPosition.x + patrolDistance)
            direction = -1;
        else if (transform.position.x < startPosition.x - patrolDistance)
            direction = 1;
    }
}
