using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlirtyTextContainer : MonoBehaviour {

	public GameObject flirtyTextParent;

	// Use this for initialization
	void Start () {
		GameManager.onGameStateUpdate += this.StateUpdated;
	}

	private void StateUpdated(GameState state)
	{
		if (state == GameState.Normal && this.flirtyTextParent.transform.childCount == 0)
		{
			GetComponent<RandomObjectGenerator>().GenerateRandomObjects();
		}
		else if (state == GameState.Seduction)
		{
			this.flirtyTextParent.SetActive(true);
		}
		else if (state == GameState.Transitioning)
		{
			GetComponent<RandomObjectGenerator>().ClearObjects();
		}
		else
		{
			this.flirtyTextParent.SetActive(false);
		}
	}
}
