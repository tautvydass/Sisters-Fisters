using System.Linq;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public Transform[] SpawnPoints;

	void Start ()
    {
        SpawnPoints = transform.GetComponentsInChildren<Transform>().Where(t => t.gameObject != this).ToArray();
    }

    public static Player Spawn(Player player)
    {
        var go = GameObject.FindGameObjectWithTag("SpawningManager");
        if (go)
        {
            var manager = go.GetComponent<SpawningManager>();
            return manager.SpawnInternal(player);
        }
        else
        {
            Debug.LogError("Cannot spawn players because spawning manager does not have tag 'SpawningManager' or does not exist.");
            return player;
        }
    }

    public Player SpawnInternal(Player player)
    {
        player.transform.position = GetRandomPos();
        return player;
    }

    public Vector3 GetRandomPos()
    {
        var ran = Random.Range(0, SpawnPoints.Length - 1);
        return SpawnPoints[ran].position;
    }
}
