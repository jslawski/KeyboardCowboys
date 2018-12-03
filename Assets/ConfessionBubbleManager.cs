using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfessionBubbleManager : MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI confessionBubbleText;

	// Use this for initialization
	void Awake () {
		GameManager.onGameStateUpdate += this.StateUpdated;
	}

	void OnDestroy()
	{
		GameManager.onGameStateUpdate -= this.StateUpdated;
	}

	private void StateUpdated(GameState state)
	{
		if (state == GameState.Confessing)
		{
			this.confessionBubbleText.text = GameManager.instance.currentCharacter.evidenceConfession;
		}
	}
}
