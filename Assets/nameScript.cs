using UnityEngine;
using System.Collections;

public class nameScript : MonoBehaviour {
	public Font defaultFont;
	string playerName;
	GUIStyle boxStyle;
	GUIStyle buttonStyle;
	bool alreadyHasName;
	
	void Start () {
		playerName = "Name";
		Destroy(GameObject.Find("Game Handler"));
		if(PlayerPrefs.GetString("Player Name").Length > 0)
			alreadyHasName = true;
	}
	
	void Update(){
		if(alreadyHasName == true)
			Application.LoadLevel(1);
	}
	
	void OnGUI() {
		//Change the font size for the input player name
		boxStyle = GUI.skin.box;
		boxStyle.font = defaultFont;
		boxStyle.fontSize = 15;
		buttonStyle = GUI.skin.button;
		buttonStyle.fontSize = 15;
		buttonStyle.font = defaultFont;
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		boxStyle.alignment = TextAnchor.MiddleCenter;
		
		if(Input.GetKey(KeyCode.KeypadPlus))
			GUI.Label(new Rect(0,0,Screen.width / 3.75f, Screen.height / 6),"Created by Michael Walsh");
		
		if(alreadyHasName == false)
		{
			playerName = GUI.TextField(new Rect((Screen.width / 2) - (Screen.width / 6), (Screen.height / 2) - (Screen.height / 20), Screen.width / 3, Screen.height / 10),playerName,7,boxStyle);
			if(GUI.Button(new Rect((Screen.width / 2) - (Screen.width / 6), (Screen.height / 2) + (Screen.height / 20),Screen.width / 3,Screen.height / 10),"Continue",buttonStyle))
			{
				PlayerPrefs.SetString("Player Name", playerName);
				Application.LoadLevel(1);
			}
		}
	}
}
