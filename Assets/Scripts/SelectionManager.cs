using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
	[SerializeField]
	private List<PlayerInputConfiguration> playerInputs;
	[SerializeField]
	private GameObject characterSelectionPrefab;
	[SerializeField]
	private Transform parent;
	[SerializeField]
	private GameObject joinText;
	[SerializeField]
	private Color countdownColor;
	private Color defaultColor;
	[SerializeField]
	private Animator fadeAnimator;
	private List<CharacterSelection> characters;

	private bool[] active = new bool[]{ false, false, false, false };

	private int[] inputIndeces = new int[4]{ -1, -1, -1, -1 };

	private int count = 0;
	private int lockedInCount = 0;

	private bool started = false;
	private bool countdown = false;

	private void Start()
	{
		characters = new List<CharacterSelection>();
		defaultColor = joinText.GetComponent<Text>().color;
		SceneManager.UnloadSceneAsync("Splash");
		StartCoroutine(FadeIn(0.5f));
	}

	private IEnumerator FadeIn(float transitionTime)
	{
		yield return new WaitForSeconds(transitionTime);
		started = true;
		fadeAnimator.enabled = false;
	}

	private IEnumerator LoadArena(float transitionTime, List<PlayerData> playersData)
	{
		var load = SceneManager.LoadSceneAsync("AnimationTest", LoadSceneMode.Additive);
		load.allowSceneActivation = false;
		fadeAnimator.enabled = true;
		fadeAnimator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(transitionTime);
		load.allowSceneActivation = true;
		yield return 0;
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().InitRound(playersData);
	}

	private void OnLockIn()
	{
		lockedInCount++;
		if(lockedInCount == count && count > 1 && !countdown)
		{
			countdown = true;
			StartCoroutine(InitCountdown());
		}
	}

	private void OnJoined()
	{
		if(countdown)
		{
			StopCoroutine(InitCountdown());
			var field = joinText.GetComponent<Text>();
			field.text = "- Press 'A' To Join And Lock In -";
			field.color = defaultColor;
			countdown = false;
			joinText.GetComponent<Animator>().enabled = true;
		}
	}

	private IEnumerator InitCountdown()
	{
		var field = joinText.GetComponent<Text>();
		field.color = countdownColor;
		joinText.GetComponent<Animator>().enabled = false;
		for(int i = 10; i > 0; i--)
		{
			if(!countdown) break;
			field.text = $"- Match Starting In { i } -";
			yield return new WaitForSeconds(1);
		}
		field.text = "- Starting Match -";
		BeginRound();
	}

	private void BeginRound()
	{
		var data = new List<PlayerData>();
		for(int i = 0; i < characters.Count; i++)
		{
			var playerData = new PlayerData(playerInputs[inputIndeces[i]], (int)characters[i].character, inputIndeces[i], characters[i].GetSounds());
			data.Add(playerData);
		}
		StartCoroutine(LoadArena(1.0f, data));
	}

	private void Update()
	{
		if(!started) return;

		for(int i = 0; i < playerInputs.Count; i++)
			if(!active[i])
				if(Input.GetButtonDown(playerInputs[i].Jump))
				{
					inputIndeces[i] = count;
					var selection = Instantiate(characterSelectionPrefab, parent);
					var positions = new SelectionPositions(++count);
					characters.Add(selection.GetComponent<CharacterSelection>().Initialize(count - 1, playerInputs[i], OnLockIn));
					for(int ind = 0; ind < characters.Count; ind++)
						characters[ind].SetPosition(positions.positions[ind]);
					active[i] = true;
					if(count == 4)
						joinText.SetActive(false);
					OnJoined();
				}
	}

	private class SelectionPositions
	{
		public List<Vector2> positions;

		public SelectionPositions(int playerCount)
		{
			switch(playerCount)
			{
				case 1:
					positions = new List<Vector2>(){ new Vector2(0, 0) };
					break;
				case 2:
					positions = new List<Vector2>(){ new Vector2(-300, 0), new Vector2(300, 0) };
					break;
				case 3:
					positions = new List<Vector2>(){ new Vector2(-600, 0), new Vector2(0, 0), new Vector2(600, 0) };
					break;
				case 4:
					positions = new List<Vector2>(){ new Vector2(-650, 0), new Vector2(-200, 0), new Vector2(240, 0), new Vector2(680, 0) };
					break;
				default:
					break;
			}
		}
	}
}
