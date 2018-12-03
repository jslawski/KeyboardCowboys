using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerdictButton : MonoBehaviour {

	[SerializeField]
	private bool verdict;
	private Button thisButton;

	void Awake() {
		this.thisButton = GetComponent<Button>();
		this.thisButton.onClick.AddListener(this.SendVerdict);
		GameManager.onGameStateUpdate += this.StateUpdated;
	}

	private void StateUpdated(GameState state)
	{
		switch (state)
		{
			case GameState.Transitioning:
				this.gameObject.SetActive(false);
				break;
			default:
				this.gameObject.SetActive(true);
				break;
		}
	}

	private void SendVerdict()
	{
		DateCharacterManager.instance.DismissCurrentCharacter(this.verdict);
	}
}
