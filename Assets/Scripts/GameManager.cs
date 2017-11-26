using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("AnimationTest"));
		for(int i = 0; i < playersData.Count; i++)
			players.Add(Instantiate(playerPrefab).GetComponent<Player>().Init(playersData[i], spawnPoints[i].position, numSprites[playersData[i].playerIndex]));
		SceneManager.UnloadSceneAsync("Character Selection");
	}

	private IEnumerator FadeIn()
	{

		yield return new WaitForSeconds(1);
	}
}
