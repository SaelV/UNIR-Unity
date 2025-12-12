using UnityEngine;

public enum BulletOwner
{
    Enemy,
    Player
}

public class Bullet : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 1;
    public BulletOwner owner = BulletOwner.Enemy;
    public float lifeTime = 5f;

    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private GameObject enemySpawned;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void Reflect(Vector2 newDir)
    {
        owner = BulletOwner.Player;
        SetDirection(newDir);
        speed *= 2f; // un buff rico al reflejar
       
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.aquamarine, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner == BulletOwner.Enemy && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (owner == BulletOwner.Player && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetEnemySpawned(GameObject obj) 
    {
        enemySpawned = obj;
    }

    public Transform GetEnemySpawned() 
    {
        return enemySpawned.transform;
    }
}
