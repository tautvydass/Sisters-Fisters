using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject playerUIPrefab;
	[SerializeField]
	private Transform parent;

	private List<PlayerHUD> players = new List<PlayerHUD>();

	public PlayerHUD SetupPlayer(int playerIndex) =>
		Instantiate(playerUIPrefab, parent).GetComponent<PlayerHUD>().Init(playerIndex);
}
