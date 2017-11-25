using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private Animator fadeAnimator;
	private List<CharacterSelection> characters;

	private bool[] active = new bool[]{ false, false, false, false };

	private int count = 0;

	private bool started = false;

	private void Start()
	{
		characters = new List<CharacterSelection>();
		SceneManager.UnloadSceneAsync("Splash");
		StartCoroutine(FadeIn(0.5f));
	}

	private IEnumerator FadeIn(float transitionTime)
	{
		yield return new WaitForSeconds(transitionTime);
		started = true;
		fadeAnimator.enabled = false;
	}

	private void Update()
	{
		if(!started) return;

		for(int i = 0; i < playerInputs.Count; i++)
			if(!active[i])
				if(Input.GetButtonDown(playerInputs[i].Jump))
				{
					var selection = Instantiate(characterSelectionPrefab, parent);
					var positions = new SelectionPositions(++count);
					characters.Add(selection.GetComponent<CharacterSelection>().Initialize(count - 1, playerInputs[i]));
					for(int ind = 0; ind < characters.Count; ind++)
						characters[ind].SetPosition(positions.positions[ind]);
					active[i] = true;
					if(count == 4)
						joinText.SetActive(false);
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
