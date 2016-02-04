using UnityEngine;
using System.Collections;

public class menuControl : MonoBehaviour {
	
	Rect[] menuItemBounds = new Rect[4];
	Rect[] createGameItemBounds = new Rect[21];
	Rect joinGameItemBounds;
	string[] Difficulty = new string[]{"Easy", "Medium", "Hard"};
	int selectedMode;
	int selectedDifficulty;
	int currentMenu = 0;
	bool transitioning;
	bool hovering;
	int hoverPos;
	float transitionTime;
	float transitionStart;
	Vector3 mousePos;
	public GUIStyle boxStyle;
	GUIStyle buttonStyle;
	public Font statFont;
	public Font defaultFont;
	public AudioClip menuTransition;
	public AudioClip menuItemHover;
	public AudioClip menuSoundChange;
	public AudioClip itemSelect;
	
	//Variables for creating game
	string[] pieces = new string[4]{"Blue O", "Blue X", "Red O", "Red X"};
	string playerTwoName = "Name";
	int scoreLimit = 1;
	int selectedPiece;	
	int selectedP1Piece;
	int selectedP2Piece;
	
	//Declare jagged Arrary for menu navigation
	string[][] menu = new string[][]
	{
		new string[]{"(main)Single Player Text","(main)Local Multiplayer Text","(main)Statistics Text","(main)Settings Text"},
		new string[]{"Difficulty/Name Text","Score Limit Text","Piece Info","Create Game Options"},
		new string[]{"","","","Back Text"},
		new string[]{"Sound Text","Change Name Text","","Back Text 2"}
	};
	
	void Start () {
		
		//Initialise variables for creating game
		scoreLimit = 1;
		selectedPiece = 0;
		selectedP1Piece = 0;
		selectedP2Piece = 3;
		
		//Make the user unable to use any invisible menu buttons until the actual text has arrived;
		transitioning = true;
		transitionStart = Time.time;
		transitionTime = 5;
	}
	
	void Update () {		
		//Calculate mouse position
		mousePos = new Vector3(Input.mousePosition.x,Screen.height - Input.mousePosition.y,Input.mousePosition.z);
		
		//Determine when a menu transition has completed so the user can again use the menu
		if(transitioning)
			if(Time.time - transitionStart > transitionTime)
				transitioning = false;
		
		//Assign invisible bounds for menu items
		menuItemBounds[0] = new Rect((3*(Screen.width / 8)) - (Screen.width / 50), Screen.height / 2.2f,Screen.width / 3.2f, Screen.height / 9);
		menuItemBounds[1] = new Rect(2.5f*(Screen.width / 8), (3*(Screen.height / 5)),Screen.width / 2.5f, Screen.height / 9);
		menuItemBounds[2] = new Rect((3.5f*(Screen.width / 8)) - (Screen.width / 50), (Screen.height / 2) + (2.5f * (Screen.height / 10.5f)),Screen.width / 5.5f, Screen.height / 9);
		menuItemBounds[3] = new Rect(3*(Screen.width / 8), (Screen.height / 2) + (4 * (Screen.height / 10.75f)),Screen.width / 4, Screen.height / 9);
		
		//Assign invisible bounds for creating game items
		createGameItemBounds[0] = new Rect(Screen.width / 2.4f, 1.575f * (Screen.height / 3),Screen.width / 8, Screen.height / 5.5f);
		createGameItemBounds[1] = new Rect((Screen.width / 2.35f) + (Screen.width / 8), 1.575f * (Screen.height / 3),Screen.width / 10, Screen.height / 5.5f);
		createGameItemBounds[2] = new Rect((Screen.width / 2.4f) + (Screen.width / 4.15f), 1.575f * (Screen.height / 3),Screen.width / 10, Screen.height / 5.5f);
		createGameItemBounds[3] = new Rect((Screen.width / 2.55f) + (3 * (Screen.width / 8)), 1.575f * (Screen.height / 3),Screen.width / 10, Screen.height / 5.5f);
		createGameItemBounds[4] = new Rect(Screen.width / 3, Screen.height - (Screen.height / 6.5f), Screen.width / 7,Screen.height / 10);
		createGameItemBounds[5] = new Rect(Screen.width / 2, Screen.height - (Screen.height / 6.5f), Screen.width / 5,Screen.height / 10);
		createGameItemBounds[6] = new Rect(Screen.width / 1.9f, Screen.height / 3.1f, Screen.width / 12.5f, Screen.height / 8.3f);
		createGameItemBounds[7] = new Rect((Screen.width / 1.9f) + (Screen.width / 12.5f), Screen.height / 3.1f, Screen.width / 14, Screen.height / 8.3f);
		createGameItemBounds[8] = new Rect((Screen.width / 1.9f) - (Screen.width / 14), Screen.height / 3.1f, Screen.width / 14, Screen.height / 8.3f);
		createGameItemBounds[9] = new Rect(Screen.width / 2.25f, Screen.height / 10, Screen.width / 4.8f, Screen.height / 7.5f);
		createGameItemBounds[10] = new Rect((Screen.width / 2.25f) + (Screen.width / 4.8f), Screen.height / 10, Screen.width / 14, Screen.height / 7.5f);
		createGameItemBounds[11] = new Rect((Screen.width / 2.25f) - (Screen.width / 14), Screen.height / 10, Screen.width / 14, Screen.height / 7.5f);
		createGameItemBounds[12] = new Rect(Screen.width / 1.73f, Screen.height / 11.5f, Screen.width / 3f, Screen.height / 7f);
		createGameItemBounds[13] = new Rect(Screen.width / 1.92f, 1.48f * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[14] = new Rect((Screen.width / 1.92f) + (Screen.width / 11), 1.48f * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[15] = new Rect((Screen.width / 1.92f) + (2*(Screen.width / 11)), 1.48f * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[16] = new Rect((Screen.width / 1.92f) + (3*(Screen.width / 11)), 1.48f * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[17] = new Rect(Screen.width / 1.92f, 2 * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[18] = new Rect((Screen.width / 1.92f) + (Screen.width / 11), 2 * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[19] = new Rect((Screen.width / 1.92f) + (2*(Screen.width / 11)), 2 * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		createGameItemBounds[20] = new Rect((Screen.width / 1.92f) + (3*(Screen.width / 11)), 2 * (Screen.height / 3),Screen.width / 12, Screen.height / 6.5f);
		
		//Assign invisible bounds for joining game item
		joinGameItemBounds = new Rect(Screen.width / 2.35f, Screen.height - (Screen.height / 6.5f), Screen.width / 7,Screen.height / 10);
		
		//Determine if mouse is within any invisible button area when not creating or joining a game
		for(int count = 0; count < 4; count++)
		{
			if(menu[currentMenu][count] != "" && currentMenu == 0 || menu[currentMenu][count] != ""  && currentMenu == 3)
			{
				if (menuItemBounds[count].Contains(mousePos) && !transitioning)
				{
					if(hovering == false)
					{
						if(PlayerPrefs.GetInt("sound") == 0)
							GetComponent<AudioSource>().PlayOneShot(menuItemHover,20);
						hoverPos = count;
						hovering = true;
					}
					GameObject.Find(menu[currentMenu][count]).GetComponent<Renderer>().material.color = Color.red;
				}
				else
				{
					if(count == hoverPos)
						hovering = false;
					GameObject.Find(menu[currentMenu][count]).GetComponent<Renderer>().material.color = Color.white;
				}
			}
		}
		
		//Determine if mouse is within any invisible button area when creating a game
		for(int count = 4; count < 6 && currentMenu == 1; count++)
		{
			if (createGameItemBounds[count].Contains(mousePos) && !transitioning)
			{					
				if(hovering == false)
				{
					if(PlayerPrefs.GetInt("sound") == 0)
						GetComponent<AudioSource>().PlayOneShot(menuItemHover,20);
					hoverPos = count;
					hovering = true;
				}
				GameObject.Find("Create Option " + (count - 3).ToString()).GetComponent<Renderer>().material.color = Color.red;
			}
			else
			{
				if(count == hoverPos)
					hovering = false;
				GameObject.Find("Create Option " + (count - 3).ToString()).GetComponent<Renderer>().material.color = Color.white;
			}
		}
		
		//Determine if mouse is within any invisible button area when viewing stats
		if(currentMenu == 2)
		{
			if(joinGameItemBounds.Contains(mousePos) && !transitioning)
			{
				if(hovering == false)
				{	
					if(PlayerPrefs.GetInt("sound") == 0)
						GetComponent<AudioSource>().PlayOneShot(menuItemHover,20);
					hovering = true;
				}
				GameObject.Find(menu[currentMenu][3]).GetComponent<Renderer>().material.color = Color.red;
			}
			else
			{
				hovering = false;
				GameObject.Find(menu[currentMenu][3]).GetComponent<Renderer>().material.color = Color.white;
			}
		}
		
		if(currentMenu == 3)
		{
			if(PlayerPrefs.GetInt("sound") == 0)
				GameObject.Find("Sound Text").GetComponent<TextMesh>().text = "Mute Sound: No";
			else if(PlayerPrefs.GetInt("sound") == 1)
				GameObject.Find("Sound Text").GetComponent<TextMesh>().text = "Mute Sound: Yes";
		}
		
		//Determine if mouse is within piece options when creating a game
		if(selectedMode == 0 && currentMenu != 0)
		{
			for(int count = 13; count < 21; count++)
			{
				if(count < 17)
				{
					if (selectedP1Piece == (count - 13) && !transitioning)
						GameObject.Find("Item " + (count - 8).ToString()).transform.RotateAround(GameObject.Find("Item " + (count - 8).ToString()).transform.position,new Vector3(0,1,0),1);
					else if(count == 13 || count == 15)
						GameObject.Find("Item " + (count - 8).ToString()).transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
					else if(count == 14 || count == 16)
						GameObject.Find("Item " + (count - 8).ToString()).transform.rotation = Quaternion.Euler(new Vector3(45,90,90));
								
					if (createGameItemBounds[count].Contains(mousePos) && !transitioning)
					{
						if(count == 13 || count == 14)
							GameObject.Find("Item " + (count - 8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySelected;
						else if(count == 15|| count == 16)
							GameObject.Find("Item " + (count - 8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySelected;
					}
					else if(count == 13 && selectedP1Piece != (count - 13) || count == 14 && selectedP1Piece != (count - 13))
						GameObject.Find("Item " + (count - 8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySmall;
					else if(count == 15 && selectedP1Piece != (count - 13) || count == 16 && selectedP1Piece != (count - 13))
						GameObject.Find("Item " + (count -8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySmall;
				}
				else if(count >= 17 && count < 21)
				{
					if (selectedP2Piece == (count - 17) && !transitioning)
						GameObject.Find("Item " + (count - 8).ToString()).transform.RotateAround(GameObject.Find("Item " + (count - 8).ToString()).transform.position,new Vector3(0,1,0),1);
					else if(count == 17 || count == 19)
						GameObject.Find("Item " + (count - 8).ToString()).transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
					else if(count == 18 || count == 20)
						GameObject.Find("Item " + (count - 8).ToString()).transform.rotation = Quaternion.Euler(new Vector3(45,90,90));
					
					if(count == 17 && selectedP2Piece != (count - 17) || count == 18 && selectedP2Piece != (count - 17))
						GameObject.Find("Item " + (count - 8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySmall;
					else if(count == 19 && selectedP2Piece != (count - 17) || count == 20 && selectedP2Piece != (count - 17))
						GameObject.Find("Item " + (count -8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySmall;
				}
			}
		}
		else if(selectedMode == 1 && currentMenu != 0)
		{
			for(int count = 0; count < 4 && currentMenu == 1; count++)
			{
				if (selectedPiece == count && !transitioning)
					GameObject.Find("Item " + (count + 1).ToString()).transform.RotateAround(GameObject.Find("Item " + (count + 1).ToString()).transform.position,new Vector3(0,1,0),1);
				else if(count == 0 || count == 2)
					GameObject.Find("Item " + (count + 1).ToString()).transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
				else if(count == 1 || count == 3)
					GameObject.Find("Item " + (count + 1).ToString()).transform.rotation = Quaternion.Euler(new Vector3(45,90,90));

				if (createGameItemBounds[count].Contains(mousePos) && !transitioning)
				{
					if(count == 0 || count == 1)
						GameObject.Find("Item " + (count + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySelected;
					else if(count == 2|| count == 3)
						GameObject.Find("Item " + (count + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySelected;
				}
				else if(count == 0 && selectedPiece != count || count == 1 && selectedPiece != count)
					GameObject.Find("Item " + (count + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySmall;
				else if(count == 2 && selectedPiece != count || count == 3 && selectedPiece != count)
					GameObject.Find("Item " + (count + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySmall;
			}
		}
	}
	
	void OnGUI() {			
		//Menu navigation
		if(!transitioning)
		{
			if(currentMenu == 0)
			{				
				//if(GUI.Button(new Rect(0,Screen.height - (Screen.height / 7),Screen.width / 6, Screen.height / 7),"Quit Game"))
					//Application.Quit();
					
				if(GUI.Button(menuItemBounds[0],"", GUIStyle.none))
				{
					if(currentMenu == 0)
					{
						selectedMode = 1;
						scoreLimit = 1;
						iTween.MoveTo(GameObject.Find("Logo"),iTween.Hash("y", 7.5f, "easeType", iTween.EaseType.easeInExpo, "time", 1));
						GameObject.Find("Create Game Text").GetComponent<TextMesh>().text = "Create Single Player Game";
						GameObject.Find("Difficulty/Name Text").GetComponent<TextMesh>().text = "Difficulty";
						menu[1][2] = "Piece Info 1";
						buttonClick(new bool[]{true, false, false, false}, 1);
					}
				}

				if(GUI.Button(menuItemBounds[1],"",GUIStyle.none))
				{
					if(currentMenu == 0)
					{
						selectedMode = 0;
						scoreLimit = 1;
						iTween.MoveTo(GameObject.Find("Logo"),iTween.Hash("y", 7.5f, "easeType", iTween.EaseType.easeInExpo, "time", 1));
						GameObject.Find("Create Game Text").GetComponent<TextMesh>().text = "Create Local Multiplayer Game";
						GameObject.Find("Difficulty/Name Text").GetComponent<TextMesh>().text = "Player Two Name";
						menu[1][2] = "Piece Info 2";
						buttonClick(new bool[]{false, true, false, false}, 1);
					}
				}
		
				if(GUI.Button(menuItemBounds[2],"",GUIStyle.none))
				{
					if(currentMenu == 0)
					{
						iTween.MoveTo(GameObject.Find("Logo"),iTween.Hash("y", 7.5f, "easeType", iTween.EaseType.easeInExpo, "time", 1));
						buttonClick(new bool[]{false, false, true, false}, 2);
					}
				}
		
				if (GUI.Button(menuItemBounds[3],"", GUIStyle.none))
				{
					if (currentMenu == 0)
						buttonClick(new bool[]{false, false, false, true}, 3);
				}
			}
			else if(currentMenu == 1)
			{
				//Change the font size for displaying score limit, difficulty, and player two's name
				boxStyle = GUI.skin.box;
				boxStyle.font = defaultFont;
				boxStyle.fontSize = 15;
				buttonStyle = GUI.skin.button;
				buttonStyle.font = defaultFont;
				buttonStyle.fontSize = 15;
				buttonStyle.alignment = TextAnchor.MiddleCenter;
				boxStyle.alignment = TextAnchor.MiddleCenter;
				
				//Display scoreLimit
				GUI.Box(createGameItemBounds[6],scoreLimit.ToString("00"),boxStyle);
				
				//Buttons for adjusting scoreLimit variable
				if(scoreLimit < 99)
				{
					if(GUI.Button(createGameItemBounds[7],">",buttonStyle))
					{
						if(PlayerPrefs.GetInt("sound") == 0)
							GetComponent<AudioSource>().PlayOneShot(itemSelect);
						scoreLimit++;	
					}
				}
				if(scoreLimit > 1)
				{
					if(GUI.Button(createGameItemBounds[8],"<",buttonStyle))
					{
						if(PlayerPrefs.GetInt("sound") == 0)
							GetComponent<AudioSource>().PlayOneShot(itemSelect);
						scoreLimit--;
					}
				}
				
				//Displays options for selecting difficulty if creating a single player match
				if(selectedMode == 1)
				{					
					//Display Difficulty
					GUI.Box(createGameItemBounds[9],Difficulty[selectedDifficulty],boxStyle);
				
					//Buttons for adjusting difficulty
					if(selectedDifficulty < 2)
					{
						if(GUI.Button(createGameItemBounds[10],">",buttonStyle))
						{
							if(PlayerPrefs.GetInt("sound") == 0)
								GetComponent<AudioSource>().PlayOneShot(itemSelect);
							selectedDifficulty++;
						}
					}
					if(selectedDifficulty > 0)
					{
						if(GUI.Button(createGameItemBounds[11],"<",buttonStyle))
						{
							if(PlayerPrefs.GetInt("sound") == 0)
								GetComponent<AudioSource>().PlayOneShot(itemSelect);
							selectedDifficulty--;
						}
					}
				}
				
				//Displays option for inputting player twos name if creating a local multiplayer match				
				if(selectedMode == 0)
				{
					playerTwoName = GUI.TextArea(createGameItemBounds[12],playerTwoName,7,boxStyle);
				}
				
				//Back to Menu
				if(GUI.Button(createGameItemBounds[4],"",GUIStyle.none))
				{
					iTween.MoveTo(GameObject.Find("Logo"),iTween.Hash("y", 0, "easeType", iTween.EaseType.easeInExpo, "time", 1));
					buttonClick(new bool[]{false, true, false, false}, 0); 
				}
				
				//Create Game
				if(GUI.Button(createGameItemBounds[5],"",GUIStyle.none))
				{
					string hostPiece = "";
					string clientPiece = "";
	
					if(selectedMode == 0)
					{
						hostPiece = pieces[selectedP1Piece];
						clientPiece = pieces[selectedP2Piece];
					}
					else if(selectedMode == 1)
					{
						hostPiece = pieces[selectedPiece];
						clientPiece = pieces[3 - selectedPiece];
					}
					
					if(selectedMode == 0)
						GameObject.Find("Game Handler").GetComponent<gameCreation>().createGame(false,Difficulty[selectedDifficulty],scoreLimit,hostPiece,clientPiece,playerTwoName);
					else if(selectedMode == 1)
						GameObject.Find("Game Handler").GetComponent<gameCreation>().createGame(true,Difficulty[selectedDifficulty],scoreLimit,hostPiece,clientPiece,"");
					}
				
				//Asthetics for choosing a piece whilst creating game
				if(selectedMode == 0)
				{
					for(int count = 13; count < 17; count++)
					{
						if(GUI.Button(createGameItemBounds[count],"",GUIStyle.none) && (count - 13) != selectedP1Piece)
						{
							if(PlayerPrefs.GetInt("sound") == 0)
								GetComponent<AudioSource>().PlayOneShot(itemSelect);
							for(int x = 13; x < 17; x++)
							{
								if(x == 13 || x == 14)
								{
									GameObject.Find("Item " + (count - 8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySmall;
									GameObject.Find("Item " + (count - 4).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySmall;
								}
								else if(x == 15 || x == 16)
								{
									GameObject.Find("Item " + (count - 8).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySmall;
									GameObject.Find("Item " + (count - 4).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySmall;	
								}
							}							
							if(count == 13 || count == 14)
							{
								selectedP1Piece = count - 13;
								selectedP2Piece = 3 - (count - 13);
								GameObject.Find("Item " + (5 + selectedP1Piece).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySelected;
								GameObject.Find("Item " + (9 + selectedP2Piece).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySelected;
							}
							else if(count == 15 || count == 16)
							{
								selectedP1Piece = count - 13;
								selectedP2Piece = 3 - (count - 13);
								GameObject.Find("Item " + (5 + selectedP1Piece).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySelected;
								GameObject.Find("Item " + (9 + selectedP2Piece).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySelected;
							}
						}
					}	
				}
				else if(selectedMode == 1)
				{
					for(int count = 0; count < 4; count++)
					{
						if(GUI.Button(createGameItemBounds[count],"",GUIStyle.none) && count != selectedPiece)
						{
							if(PlayerPrefs.GetInt("sound") == 0)
								GetComponent<AudioSource>().PlayOneShot(itemSelect);
							for(int x = 0; x < 4; x++)
							{
								if(x == 0 || x == 1)
									GameObject.Find("Item " + (x + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySmall;
								else if(x == 2 || x == 3)
									GameObject.Find("Item " + (x + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySmall;					
							}
						
							if(count == 0 || count == 1)
							{
								selectedPiece = count;
								GameObject.Find("Item " + (count + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().blueToonySelected;
							}
							else if(count == 2 || count == 3)
							{
								selectedPiece = count;
								GameObject.Find("Item " + (count + 1).ToString()).GetComponent<Renderer>().material = GameObject.Find("Game Handler").GetComponent<gameCreation>().redToonySelected;
							}
						}
					}
				}
			}
			else if(currentMenu == 2)
			{
				boxStyle = GUI.skin.box;
				boxStyle.font = statFont;
				boxStyle.fontSize = 15;
				boxStyle.alignment = TextAnchor.MiddleLeft;
				string statsFormat = "{0,-7}{1,-9}{2,-7}";
				
				float easyRat = 0;
				float mediumRat = 0;
				float hardRat = 0;
				float p1Rat = 0;
				float p2Rat = 0;
				
				if(PlayerPrefs.GetInt("eL") != 0)
					easyRat = Mathf.Round(100*((float)PlayerPrefs.GetInt("eW") / (float)PlayerPrefs.GetInt("eL"))) / 100;
				if(PlayerPrefs.GetInt("mL") != 0)
					mediumRat = Mathf.Round(100*((float)PlayerPrefs.GetInt("mW") / (float)PlayerPrefs.GetInt("mL"))) / 100;
				if(PlayerPrefs.GetInt("hL") != 0)
					hardRat = Mathf.Round(100*((float)PlayerPrefs.GetInt("hW") / (float)PlayerPrefs.GetInt("hL"))) / 100;
				if(PlayerPrefs.GetInt("P1L") != 0)
					p1Rat = Mathf.Round(100*((float)PlayerPrefs.GetInt("P1W") / (float)PlayerPrefs.GetInt("P1L"))) / 100;
				if(PlayerPrefs.GetInt("P2L") != 0)
					p2Rat = Mathf.Round(100*((float)PlayerPrefs.GetInt("P2W") / (float)PlayerPrefs.GetInt("P2L"))) / 100;
				
				GUI.Box(new Rect(Screen.width / 12, 3.5f*(Screen.height / 10),Screen.width / 10, Screen.height / 8),"Won",boxStyle);
				GUI.Box(new Rect(Screen.width / 12, 5*(Screen.height / 10),Screen.width / 10, Screen.height / 8),"Lost",boxStyle);
				GUI.Box(new Rect(Screen.width / 12, 6.5f*(Screen.height / 10),Screen.width / 10, Screen.height / 8),"Ratio",boxStyle);
				
				GUI.Box(new Rect(Screen.width / 5, 0.5f*(Screen.height / 10), Screen.width - (Screen.width / 1.6f), Screen.height / 8),"Single Player",boxStyle);
				GUI.Box(new Rect(Screen.width / 5, 2*(Screen.height / 10), Screen.width - (Screen.width / 1.6f), Screen.height / 8),string.Format(statsFormat,"Easy","Medium","Hard"),boxStyle);
				GUI.Box(new Rect(Screen.width / 5, 3.5f*(Screen.height / 10), Screen.width - (Screen.width / 1.6f), Screen.height / 8),string.Format(statsFormat,PlayerPrefs.GetInt("eW"),PlayerPrefs.GetInt("mW"),PlayerPrefs.GetInt("hW")),boxStyle);
				GUI.Box(new Rect(Screen.width / 5, 5*(Screen.height / 10), Screen.width - (Screen.width / 1.6f), Screen.height / 8),string.Format(statsFormat,PlayerPrefs.GetInt("eL"),PlayerPrefs.GetInt("mL"),PlayerPrefs.GetInt("hL")),boxStyle);
				GUI.Box(new Rect(Screen.width / 5, 6.5f*(Screen.height / 10), Screen.width - (Screen.width / 1.6f), Screen.height / 8),string.Format(statsFormat,easyRat.ToString(),mediumRat.ToString(),hardRat.ToString()),boxStyle);
				
				GUI.Box(new Rect(7.5f *(Screen.width / 12), 0.5f*(Screen.height / 10), 3.75f*(Screen.width / 12), Screen.height / 8),"Multiplayer",boxStyle);
				GUI.Box(new Rect(7.5f *(Screen.width / 12), 2*(Screen.height / 10), 3.75f*(Screen.width / 12), Screen.height / 8),string.Format("{0,-10}{1,-10}","Player 1","Player 2"),boxStyle);
				GUI.Box(new Rect(7.5f *(Screen.width / 12), 3.5f*(Screen.height / 10), 3.75f*(Screen.width / 12), Screen.height / 8),string.Format("{0,-10}{1,-10}",PlayerPrefs.GetInt("P1W"),PlayerPrefs.GetInt("P2W")),boxStyle);
				GUI.Box(new Rect(7.5f *(Screen.width / 12), 5*(Screen.height / 10), 3.75f*(Screen.width / 12), Screen.height / 8),string.Format("{0,-10}{1,-10}",PlayerPrefs.GetInt("P1L"),PlayerPrefs.GetInt("P2L")),boxStyle);
				GUI.Box(new Rect(7.5f *(Screen.width / 12), 6.5f*(Screen.height / 10), 3.75f*(Screen.width / 12), Screen.height / 8),string.Format("{0,-10}{1,-10}",p1Rat.ToString(),p2Rat.ToString()),boxStyle);
				
				//Back to Menu
				if(GUI.Button(joinGameItemBounds,"",GUIStyle.none))
				{
					iTween.MoveTo(GameObject.Find("Logo"),iTween.Hash("y", 0, "easeType", iTween.EaseType.easeInExpo, "time", 1));
					buttonClick(new bool[]{false, false, true, false}, 0); 
				}
			}
			else if(currentMenu == 3)
			{
				if(GUI.Button(menuItemBounds[0],"",GUIStyle.none))
				{
					if(PlayerPrefs.GetInt("sound") == 0)
						PlayerPrefs.SetInt("sound",1);
					else if(PlayerPrefs.GetInt("sound") == 1)
					{
						PlayerPrefs.SetInt("sound",0);
						GetComponent<AudioSource>().PlayOneShot(itemSelect);
					}
				}
				if(GUI.Button(menuItemBounds[1],"",GUIStyle.none))
				{
					PlayerPrefs.SetString("Player Name","");
					Application.LoadLevel(0);
				}
				
				if(GUI.Button(menuItemBounds[3],"",GUIStyle.none))
					buttonClick(new bool[]{false, false, false, true}, 0); 
			}
		}
	}
	
	//Procedure called whenever the user navigates to another menu
	void buttonClick(bool[] clicked, int nextMenu)
	{		
		transitioning = true;
		hovering = false;
		transitionStart = Time.time;
		transitionTime = 1.1f;
		
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(menuTransition);
		
		for(int count = 0; count < 4; count++)
			if(menu[currentMenu][count] != "")
				GameObject.Find(menu[currentMenu][count]).GetComponent<menuTextMove>().leaveScreen(clicked[count]);
				
		if(currentMenu == 1)
			iTween.MoveTo(GameObject.Find("Create Game Text"),iTween.Hash("y", 10,"easeType", iTween.EaseType.easeInExpo, "time", 1));
		
		currentMenu = nextMenu;
				
		if(currentMenu == 1)
			iTween.MoveTo(GameObject.Find("Create Game Text"),iTween.Hash("y", 6.4f,"easeType", iTween.EaseType.easeInExpo, "time", 1));	
		
		for(int count = 0; count < 4; count++)
			if(menu[currentMenu][count] != "")
				GameObject.Find(menu[currentMenu][count]).GetComponent<menuTextMove>().enterScreen();
	}
}
