using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour {

	public Image menuNav;
	int menuIndex = 0;
	public List<Sprite> menus;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			menuIndex++;
			if (menuIndex < menus.Count)
			{
				menuNav.sprite = menus[menuIndex];
			}
			else
			{
				Application.LoadLevel("MainScene");
			}
		}
	}
}
