using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.gameObject.GetComponent<Player>();
        if (player)
        {
            player.Lives--;
            if (player.Lives > 0)
            {
                player.Health = 0;
                SpawningManager.Spawn(player);
            }
            else
            {
                Destroy(player);
            }
        }
    }
}
