using UnityEngine;

public class LifePickUp : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player)
        {
            player.AddLife();
            Destroy(gameObject);
        }
    }
}
