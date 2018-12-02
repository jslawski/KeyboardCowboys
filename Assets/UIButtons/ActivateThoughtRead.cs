using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateThoughtRead : MonoBehaviour {

	private Button thisButton;

	void Awake()
	{
		this.thisButton = GetComponent<Button>();
		this.thisButton.onClick.AddListener(this.UpdateGameState);
	}

	private void UpdateGameState()
	{
		if (GameManager.instance.state != GameState.Thought)
		{
			GameManager.instance.UpdateGameState(GameState.Thought);
		}
		else
		{
			GameManager.instance.UpdateGameState(GameState.Normal);
		}
	}
}
