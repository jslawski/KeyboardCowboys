using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordButton : MonoBehaviour {

	public bool isGreen = false;
	public Button thisButton;

	// Use this for initialization
	void Start () {
		this.thisButton = GetComponent<Button>();
		this.thisButton.onClick.AddListener(this.ButtonClicked);
	}

	private void ButtonClicked()
	{
		if (isGreen == true)
		{
			VisionManager.instance.GreenClicked(this);
		}
		else
		{
			VisionManager.instance.RedClicked();
		}
	}
}
