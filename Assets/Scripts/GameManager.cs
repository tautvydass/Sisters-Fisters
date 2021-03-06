﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private List<PlayerInputConfiguration> inputs;
	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private WinScreen winScreen;
	[SerializeField]
	private List<Transform> spawnPoints;
	[SerializeField]
	private List<Sprite> numSprites;

	public List<Player> players = new List<Player>();

	public int PlayerCount { get; private set; }

	public void InitRound(List<PlayerData> playersData)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapForest"));
		for(int i = 0; i < playersData.Count; i++)
			players.Add(Instantiate(playerPrefab).GetComponent<Player>().Init(playersData[i], spawnPoints[i].position, numSprites[playersData[i].playerIndex]));
		SceneManager.UnloadSceneAsync("Character Selection");
		StartCoroutine(FadeIn());
	}

	private IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(4.5f);
		foreach(var player in players)
			player.Enable();
	}

	public void EndGame(int playerIndex, int characterIndex, Action victoryChantCallback)
	{
		winScreen.AnnounceWinner(playerIndex, characterIndex, victoryChantCallback);
	}
}
