using System.Linq;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
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
                Destroy(player.gameObject);

                var manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                var alivePlayers = manager.players.Where(p => p == true);
                if (alivePlayers.Count() == 1)
                {
                    var alivePlayer = alivePlayers.FirstOrDefault(p => p == true);
                    manager.EndGame(alivePlayer.data.playerIndex, alivePlayer.data.characterIndex);
                }
            }
        }
    }
}
