using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DifficultyLevel { Easy, Medium, Hard, None }

public enum GameState { Normal, Transitioning, Thought, Vision, Seduction, Ready, GameOver, Confessing }

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public Camera mainCamera;

	public delegate void GameStateUpdated(GameState state);
	public static event GameStateUpdated onGameStateUpdate;

	[HideInInspector]
	public DateCharacter currentCharacter;

	[SerializeField]
	private GameObject gameOverScreen;

	public GameState state = GameState.Normal;

	void Awake()
	{
		GameManager.instance = this;
	}

	public void UpdateGameState(GameState state)
	{
		this.state = state;

		if (GameManager.onGameStateUpdate != null)
		{
			GameManager.onGameStateUpdate(this.state);
		}

		if (this.state == GameState.GameOver)
		{
			this.gameOverScreen.SetActive(true);
		}
	}

}
