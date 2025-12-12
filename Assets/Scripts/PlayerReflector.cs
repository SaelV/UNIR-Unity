using UnityEngine;

public class PlayerReflector : MonoBehaviour
{
    PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (!bullet) return;
        if (bullet.owner != BulletOwner.Enemy) return;
        if (!playerController.IsReflectActive()) return;

        var dir = (bullet.transform.position - playerController.transform.position).normalized;
        try
        {
            dir = (bullet.GetEnemySpawned().position - playerController.transform.position).normalized;
        }
        catch
        {
            dir = (bullet.transform.position - playerController.transform.position).normalized;
        }


        if (playerController.IsInParryWindow())
        {
            bullet.Reflect(dir);
            playerController.RegenEnergyOnPerfectReflect();
            //CameraShake
        }
        else
        {
            if (playerController.currentEnergy > 0)
            {
                playerController.SpendEnergyOnFailedReflect();
                bullet.Reflect(dir);
            }
            else
            {
                //PlayDrySound
            }
        }

    }

}
