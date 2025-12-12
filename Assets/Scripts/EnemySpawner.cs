using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float xRange = 8f; 
    public float yRange = 3f;
    public HUD hud;

    
    private float timer;
    public float cooldown =30f;
    public float maxEnemies = 10f;
    private float eCounter;


 
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && eCounter < 10 && !hud.IsGameOver())
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
        else if (eCounter >= 10)
        {
            timer = cooldown;
            eCounter = 0;
        }
    }

    void SpawnEnemy()
    {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, yRange);
        Vector3 pos = new Vector3(x, y, 0f);
        var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity).GetComponent<Enemy>();

        enemy.movingDirection = new Vector2 (-Mathf.Sign(x), -Mathf.Sign(y));
        enemy.hud = hud;
        eCounter++;

        Destroy(enemy.gameObject,20f);
    }
    
}
