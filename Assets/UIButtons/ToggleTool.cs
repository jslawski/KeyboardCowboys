using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTool : MonoBehaviour {

	private Button thisButton;
	public GameState stateToChangeTo;

	void Start()
	{
		this.thisButton = GetComponent<Button>();
		this.thisButton.onClick.AddListener(this.UpdateGameState);
		GameManager.onGameStateUpdate -= this.StateUpdated;
		GameManager.onGameStateUpdate += this.StateUpdated;
	}

	private void UpdateGameState()
	{
		if (GameManager.instance.state == GameState.Transitioning)
		{
			return;
		}

		if (GameManager.instance.state == GameState.Displaying && this.stateToChangeTo == GameState.Vision)
		{
			GameManager.instance.UpdateGameState(GameState.Normal);
		}
		else if (GameManager.instance.state != stateToChangeTo)
		{
			GameManager.instance.UpdateGameState(stateToChangeTo);
		}
		else
		{
			GameManager.instance.UpdateGameState(GameState.Normal);
		}
	}

	private void StateUpdated(GameState state)
	{
		if (state == GameState.Normal || state == GameState.Transitioning)
		{
			this.gameObject.SetActive(true);
		}
		else if (state != this.stateToChangeTo)
		{
			this.gameObject.SetActive(false);
		}

		if (state == GameState.Displaying && this.stateToChangeTo == GameState.Vision)
		{
			this.gameObject.SetActive(true);
		}
	}
}
