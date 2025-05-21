using UnityEngine;

public class BadakMovement : MonoBehaviour
{
    public float speed = 1.5f;      // Kecepatan badak
    public float patrolDistance = 4f; // Jarak maksimal patrol

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
