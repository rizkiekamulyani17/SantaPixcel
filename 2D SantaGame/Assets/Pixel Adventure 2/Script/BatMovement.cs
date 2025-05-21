using UnityEngine;

public class BatMovement : MonoBehaviour
{
    public float speed = 2f;      // Kecepatan kelelawar
    public float patrolDistance = 3f; // Jarak maksimal ke kanan dan kiri dari posisi awal

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Gerakkan kelelawar maju mundur di sumbu X
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Jika sudah mencapai batas patrol, balik arah
        if (transform.position.x > startPosition.x + patrolDistance)
            direction = -1;
        else if (transform.position.x < startPosition.x - patrolDistance)
            direction = 1;
    }
}
