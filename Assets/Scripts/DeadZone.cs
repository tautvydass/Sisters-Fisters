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
                var manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                manager.players.Remove(player);
                Destroy(player.gameObject);
                var alivePlayers = manager.players.Count;
                if (alivePlayers == 1)
                {
                    var alivePlayer = manager.players[0];
                    manager.EndGame(alivePlayer.data.playerIndex, alivePlayer.data.characterIndex);
                }
            }
        }
    }
}
