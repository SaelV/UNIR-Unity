using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public int maxHealth = 2;
    public float shootInterval = 1.5f;
    public GameObject bulletPrefab;
    public int score = 50;

    private int currentHealth;
    private Transform player;
    private float shootTimer;

    public Vector2 movingDirection = Vector2.down;
    public HUD hud;

    public GameObject lifePickUp;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // Movimiento básico (hacia abajo)
        transform.Translate(movingDirection * moveSpeed * Time.deltaTime);

        if (player == null) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootAtPlayer();
            shootTimer = shootInterval;
        }
    }

    void ShootAtPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        bullet.owner = BulletOwner.Enemy;
        bullet.SetDirection(dir);
        bullet.SetEnemySpawned(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if(hud) hud.AddScore(score);
            if (Random.Range(0f, 1f) < 0.2f)
            {
                Instantiate(lifePickUp, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            
        }
    }
}
