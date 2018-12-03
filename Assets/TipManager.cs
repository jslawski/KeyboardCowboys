using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipManager : MonoBehaviour {

	public static TipManager instance;

	float tipUptime = 5f;
	float tipFadeSpeed = 0.01f;

	public string tipText;
	[SerializeField]
	private Text tip;
	[SerializeField]
	private Image tipBox;


	// Use this for initialization
	void Start ()
	{
		instance = this;	
	}

	public void DisplayTip()
	{
		StartCoroutine(this.DisplayTipCoroutine());
	}

	private IEnumerator DisplayTipCoroutine()
	{
		float startingAlpha = 0.0f;

		tip.text = tipText;

		while (startingAlpha < 1)
		{
			this.tipBox.color = new Color(this.tipBox.color.r, this.tipBox.color.g, this.tipBox.color.b, startingAlpha);

			this.tip.color = new Color(this.tip.color.r, this.tip.color.g, this.tip.color.b, startingAlpha);

			startingAlpha += this.tipFadeSpeed * Time.deltaTime;
		}

		yield return new WaitForSeconds(this.tipUptime);

		while (startingAlpha > 0)
		{
			this.tipBox.color = new Color(this.tipBox.color.r, this.tipBox.color.g, this.tipBox.color.b, startingAlpha);

			this.tip.color = new Color(this.tip.color.r, this.tip.color.g, this.tip.color.b, startingAlpha);

			startingAlpha -= this.tipFadeSpeed * Time.deltaTime;
		}
	}
}
