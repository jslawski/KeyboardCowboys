using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyLevel { Easy, Medium, Hard, None }

public enum GameState { Normal, Thought, Vision, Seduction }

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public DateCharacter currentCharacter;

	public GameState state = GameState.Normal;

	void Awake()
	{
		GameManager.instance = this;
	}

	public void UpdateGameState(GameState state)
	{
		this.state = state;
	}

}
