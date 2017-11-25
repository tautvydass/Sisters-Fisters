using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private List<PlayerInputConfiguration> inputs;
	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private WinScreen winScreen;

	public List<Player> players = new List<Player>();

	public int PlayerCount { get; private set; }

	private void Start()
	{
		foreach(var input in inputs)
			players.Add(Instantiate(playerPrefab).GetComponent<Player>().Init(input, null));
	}
	public void InitRound(List<PlayerData> playersData)
	{
		foreach(var data in playersData)
			players.Add(Instantiate(playerPrefab).GetComponent<Player>().Init(null, null));
	}
}
