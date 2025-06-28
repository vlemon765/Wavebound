using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 5f;
    private Vector3 startPosition;
    private Vector3 direction;

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        startPosition = transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
