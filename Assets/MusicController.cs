using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

	public AudioSource myAudio;
	bool playing = false;

	// Use this for initialization
	void Awake () {

		if (GameObject.FindGameObjectsWithTag("Music").Length == 1)
		{ 
			myAudio.Play();
			this.playing = true;
		}

		DontDestroyOnLoad(myAudio);

		GameManager.onGameStateUpdate += this.UpdateState;
	}

	private void UpdateState(GameState state)
	{
		if (state == GameState.GameOver)
		{
			Destroy(this.gameObject);
		}
	}

	void OnDestroy()
	{
		GameManager.onGameStateUpdate -= this.UpdateState;
	}
}
