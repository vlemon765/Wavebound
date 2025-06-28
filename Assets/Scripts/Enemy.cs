using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Sprite[] walkRight;

    private Transform player;
    private SpriteRenderer sr;

    private float animTimer = 0f;
    private int frameIndex = 0;
    private float frameRate = 0.2f;
    private int enemyHealth = 0;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        AnimateWalk(direction);
    }

    void AnimateWalk(Vector2 dir)
    {
        animTimer += Time.deltaTime;
        if (animTimer >= frameRate)
        {
            animTimer = 0f;
            frameIndex = (frameIndex + 1) % 3;

            Sprite[] currentAnim = walkRight;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                currentAnim = walkRight;
                sr.flipX = dir.x < 0;
            }
            else
            {
                sr.flipX = false;
            }

            sr.sprite = currentAnim[frameIndex];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager gm = FindAnyObjectByType<GameManager>();
        if (collision.CompareTag("Projectile"))
        {
            enemyHealth++;
            if (enemyHealth == 2)
            {
                Destroy(gameObject); // Enemy dies
                gm.DecreaseEnemyCount();
            }
            Destroy(collision.gameObject); // Destroy projectile too
        }
        // Debug.Log("Collided with: " + collision.name);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(1);
            }
        }
    }
}
