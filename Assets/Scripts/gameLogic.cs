using UnityEngine;
using System.Collections;

public class gameLogic : MonoBehaviour {	
	//Misc
	char[,] piece = new char[3,3];
	Rect[,] pieceBounds = new Rect[3,3];
	Rect[] gameOverOptionBounds = new Rect[2];
	string[] gameOverOptions = new string[2];
	Vector3[,] piecePos = new Vector3[3,3];
	public string playerTwoName;
	public bool singlePlayer;
	bool assignedNames;
	bool playersAssigned;
	bool restartingRound;
	float startTime;
	public float turnTimer;
	public float turnTimerStart;
	bool readyToQuit;
	bool restartingGame;
	int AIX = 0;
	int AIY = 0;
	
	//Prefabs
	public GameObject transparentCross;
	public GameObject transparentRing;
	public GameObject cross;
	public GameObject ring;
	
	//Materials
	public Material redToony;
	public Material redToonySmall;
	public Material redToonySelected;
	public Material blueToony;
	public Material blueToonySmall;
	public Material blueToonySelected;
	Material hostMat;
	Material clientMat;
	Material hostSmallMat;
	Material clientSmallMat;
	Material hostSelectedMat;
	Material clientSelectedMat;
	
	//Gameplay
	public AudioClip hoverSound;
	public AudioClip moveMadeSound;
	public AudioClip score;
	public AudioClip transition;
	bool hover;
	int hoverX;
	int hoverY;
	GameObject currentTransparentPiece;
	GameObject currentPiece;
	GameObject playerOnePiece;
	GameObject playerTwoPiece;
	public int scoreLimit;
	public string hostPieceInfo;
	public string clientPieceInfo;
	int hostScore = 0;
	int clientScore = 0;
	bool myTurn;
	bool cantMove;
	bool gameOver;
	char myPiece;
	
	//Single Player
	public string difficulty;
	public char AIpiece;
	
	void Start () {
		//Assign button bounds for end of game options
		gameOverOptionBounds[0] = new Rect(0, Screen.height - (Screen.height / 5),Screen.width / 3.25f, Screen.height / 5);
		gameOverOptionBounds[1] = new Rect(Screen.width - (Screen.width / 3.25f), Screen.height - (Screen.height / 5),Screen.width / 3.25f, Screen.height / 5);
		
		//Assign GameObject names for end of game options
		gameOverOptions[0] = "Rematch";
		gameOverOptions[1] = "Menu";
		
		//Assign instantiate positions for pieces
		piecePos[0,0] = new Vector3(-1.4f,1.4f,0);
		piecePos[1,0] = new Vector3(0,1.4f,0);
		piecePos[2,0] = new Vector3(1.4f,1.4f,0);
		piecePos[0,1] = new Vector3(-1.4f,0,0);
		piecePos[1,1] = new Vector3(0,0,0);
		piecePos[2,1] = new Vector3(1.4f,0,0);
		piecePos[0,2] = new Vector3(-1.4f,-1.4f,0);
		piecePos[1,2] = new Vector3(0,-1.4f,0);
		piecePos[2,2] = new Vector3(1.4f,-1.4f,0);	
	}
	
	void Update () {		
		//Assign player data
		if(Application.isLoadingLevel == false && assignedNames == false && Application.loadedLevel == 2)
		{
			startTime = Time.time;			
			playerAssign();
			
			GameObject.Find("Game Info Text").GetComponent<TextMesh>().text = "First To " + scoreLimit.ToString();

			GameObject.Find("Player One Name Text").GetComponent<TextMesh>().text = PlayerPrefs.GetString("Player Name");
				
			if(singlePlayer == true)
				GameObject.Find("Player Two Name Text").GetComponent<TextMesh>().text = difficulty + " Bot";
			else
				GameObject.Find("Player Two Name Text").GetComponent<TextMesh>().text = playerTwoName;

			if(hostPieceInfo[hostPieceInfo.Length - 1] == 'X')
				playerOnePiece = Instantiate(cross,new Vector3(-3.51f,0.3f,-0.3f),Quaternion.Euler(new Vector3(45,90,90))) as GameObject;
			else if(hostPieceInfo[hostPieceInfo.Length - 1] == 'O')
				playerOnePiece = Instantiate(ring,new Vector3(-3.52f,0.28f,-0.25f),Quaternion.Euler(new Vector3(45,90,90))) as GameObject;
				
			playerOnePiece.GetComponent<Renderer>().material = hostSmallMat;
			playerOnePiece.transform.parent = GameObject.Find("Player One Data Holder").transform;
			GameObject.Find("Player One Data Holder").GetComponent<Renderer>().material = hostSmallMat;
			GameObject.Find("Player One Score Holder").GetComponent<Renderer>().material = hostSmallMat;
				
			if(clientPieceInfo[clientPieceInfo.Length - 1] == 'X')
				playerTwoPiece = Instantiate(cross,new Vector3(3.51f,0.3f,-0.3f),Quaternion.Euler(new Vector3(45,90,90))) as GameObject;
			else if(clientPieceInfo[clientPieceInfo.Length - 1] == 'O')
				playerTwoPiece = Instantiate(ring,new Vector3(3.52f,0.28f,-0.25f),Quaternion.Euler(new Vector3(45,90,90))) as GameObject;
				
			playerTwoPiece.GetComponent<Renderer>().material = clientSmallMat;
			playerTwoPiece.transform.parent = GameObject.Find("Player Two Data Holder").transform;
			GameObject.Find("Player Two Data Holder").GetComponent<Renderer>().material = clientSmallMat;
			GameObject.Find("Player Two Score Holder").GetComponent<Renderer>().material = clientSmallMat;
						
			playerOnePiece.transform.localScale = new Vector3(0.13f,0.13f,0.13f);
			playerTwoPiece.transform.localScale = new Vector3(0.13f,0.13f,0.13f);			
			assignedNames = true;
		}	
		
		//Stop first player from making a move before board has arrived
		if(Time.time - startTime > 3 && playersAssigned == false && assignedNames == true) {				
			turnTimerStart = Time.time;
			highlightChange();
			playersAssigned = true;
		}
		
		//Assign invisible button bounds for pieces
		pieceBounds[0,0] = new Rect(((Screen.width / 2) - (Screen.width / 18)) - (Screen.width / 5.2f), (Screen.height / 2) - (Screen.height / 3.35f), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[1,0] = new Rect((Screen.width / 2) - (Screen.width / 13), (Screen.height / 2) - (Screen.height / 3.35f), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[2,0] = new Rect((Screen.width / 2) + (Screen.width / 10), (Screen.height / 2) - (Screen.height / 3.35f), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[0,1] = new Rect(((Screen.width / 2) - (Screen.width / 18)) - (Screen.width / 5.2f), (Screen.height / 2) - (Screen.height / 28), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[1,1] = new Rect((Screen.width / 2) - (Screen.width / 13), (Screen.height / 2) - (Screen.height / 28), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[2,1] = new Rect((Screen.width / 2) + (Screen.width / 10), (Screen.height / 2) - (Screen.height / 28), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[0,2] = new Rect(((Screen.width / 2) - (Screen.width / 18)) - (Screen.width / 5.2f), (Screen.height / 2) + (Screen.height / 4.25f), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[1,2] = new Rect((Screen.width / 2) - (Screen.width / 13), (Screen.height / 2) + (Screen.height / 4.25f), Screen.width / 6.5f, Screen.height / 4.5f);
		pieceBounds[2,2] = new Rect((Screen.width / 2) + (Screen.width / 10), (Screen.height / 2) + (Screen.height / 4.25f), Screen.width / 6.5f, Screen.height / 4.5f);	
		
		//Single player AI
		if(singlePlayer == true && !myTurn && playersAssigned == true && cantMove == false)
			if(Time.time - turnTimerStart > 1)
				AIMove();
		
		//Transparent Piece spawn/despawn handling
		Destroy(currentTransparentPiece);
		for(int x = 0; x < 3; x++)
		{
			for(int y = 0; y < 3; y++)
			{
				if(pieceBounds[x,y].Contains(new Vector3(Input.mousePosition.x,Screen.height - Input.mousePosition.y, Input.mousePosition.z)) && piece[x,y] == '\0' && cantMove == false && restartingRound == false && playersAssigned == true)
				{
					if(myTurn)
					{
						if(hover == false)
						{
							if(PlayerPrefs.GetInt("sound") == 0)
								GetComponent<AudioSource>().PlayOneShot(hoverSound);
							hoverX = x;
							hoverY = y;
							hover = true;
						}
						if(hoverX != x || hoverY != y)
							hover = false;
						
						if(myPiece == 'X')
							currentTransparentPiece = Instantiate(transparentCross,piecePos[x,y],Quaternion.Euler(45,90,90)) as GameObject;
						else if(myPiece == 'O')
							currentTransparentPiece = Instantiate(transparentRing,piecePos[x,y],Quaternion.Euler(0,90,90)) as GameObject;
					}
					else if(singlePlayer == false && !myTurn)
					{

						if(hover == false)
						{
							if(PlayerPrefs.GetInt("sound") == 0)
							{
								GetComponent<AudioSource>().PlayOneShot(hoverSound);
								hoverX = x;
								hoverY = y;
								hover = true;
							}
						}
						if(hoverX != x || hoverY != y)
							hover = false;
						
						if(AIpiece == 'X')
							currentTransparentPiece = Instantiate(transparentCross,piecePos[x,y],Quaternion.Euler(45,90,90)) as GameObject;
						else if(AIpiece == 'O')
							currentTransparentPiece = Instantiate(transparentRing,piecePos[x,y],Quaternion.Euler(0,90,90)) as GameObject;
					}
				}
			}
		}
		
		//Highlight end of game options when moused over
		for(int x = 0; x < 2 && gameOver == true; x++)
		{
			if(gameOverOptionBounds[x].Contains(new Vector3(Input.mousePosition.x,Screen.height - Input.mousePosition.y, Input.mousePosition.z)))
			{
				GameObject.Find(gameOverOptions[x] + " Text 1").GetComponent<Renderer>().material.color = Color.red;
				GameObject.Find(gameOverOptions[x] + " Text 2").GetComponent<Renderer>().material.color = Color.red;
			}
			else
			{
				GameObject.Find(gameOverOptions[x] + " Text 1").GetComponent<Renderer>().material.color = Color.white;
				GameObject.Find(gameOverOptions[x] + " Text 2").GetComponent<Renderer>().material.color = Color.white;
			}
		}

	}
	
	void OnGUI() {	
		//Make move determined by which button is clicked
		for(int x = 0; x < 3; x++)
		{
			for(int y = 0; y < 3; y++)
			{
				if(GUI.Button(pieceBounds[x,y],"",GUIStyle.none) && piece[x,y] == '\0' && playersAssigned == true)
				{
					if(myTurn && cantMove == false)
						makeMove(myPiece.ToString(),x,y);
					else if(singlePlayer == false && !myTurn && cantMove == false)
						makeMove(AIpiece.ToString(),x,y);
				}
			}
		}
		
		//End of game options
		if(gameOver)
		{
			//Rematch
			if(GUI.Button(gameOverOptionBounds[0],"",GUIStyle.none))
			{
				hostScore = 0;
				clientScore = 0;
				
				if(PlayerPrefs.GetInt("sound") == 0)
					GetComponent<AudioSource>().PlayOneShot(moveMadeSound);
				
				iTween.MoveTo(GameObject.Find("Player One Data Holder"),iTween.Hash("x", -4.77f,"y", 3.42f,"z",1.146f,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
				iTween.MoveTo(GameObject.Find("Player One Score Holder"),iTween.Hash("x", -4.24f,"y", -1.85f,"z",0,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
				iTween.MoveTo(GameObject.Find("Player Two Data Holder"),iTween.Hash("x", 4.77f,"y", 3.42f,"z",1.146f,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
				iTween.MoveTo(GameObject.Find("Player Two Score Holder"),iTween.Hash("x", 4.24f,"y", -1.85f,"z",0,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
				iTween.MoveTo(GameObject.Find("Game Info Text"),iTween.Hash("y", 2.75f,"easeType", iTween.EaseType.easeInExpo, "time", 1));
				iTween.MoveTo(GameObject.Find("Player One Win Text"),iTween.Hash("y", 5,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
				iTween.MoveTo(GameObject.Find("Player Two Win Text"),iTween.Hash("y", 5,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
				iTween.MoveTo(GameObject.Find("Rematch"),iTween.Hash("x", -7.5f,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
				iTween.MoveTo(GameObject.Find("Menu"),iTween.Hash("x", 7,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
				GameObject.Find("Player One Score Text").GetComponent<TextMesh>().text = "0";
				GameObject.Find("Player Two Score Text").GetComponent<TextMesh>().text = "0";				
				
				GameObject.Find("Player One Score Holder").GetComponent<Renderer>().material = hostSmallMat;
				GameObject.Find("Player Two Score Holder").GetComponent<Renderer>().material = clientSmallMat;
				
				restartingGame = true;
				StartCoroutine(newRound());
				gameOver = false;
			}
			
			//Leave Game
			if(GUI.Button(gameOverOptionBounds[1],"",GUIStyle.none))
			{
				Application.LoadLevel(1);
				Destroy(gameObject);
			}
		}
	}
	
	void makeMove(string pieceType, int x, int y) {		
		//Move Making Handling
		if(pieceType == "X")
			currentPiece = Instantiate(cross,piecePos[x,y] + new Vector3(0,0,4.5f),Quaternion.Euler(45,90,90)) as GameObject;
		else if(pieceType == "O")
			currentPiece = Instantiate(ring,piecePos[x,y] + new Vector3(0,0,4.5f),Quaternion.Euler(0,90,90)) as GameObject;
		
		if(pieceType[0] == hostPieceInfo[hostPieceInfo.Length - 1])
			currentPiece.gameObject.GetComponent<Renderer>().material = hostMat;
		else if(pieceType[0] == clientPieceInfo[clientPieceInfo.Length - 1])
			currentPiece.gameObject.GetComponent<Renderer>().material = clientMat;
			
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(moveMadeSound);
		currentPiece.gameObject.name = "Piece " + x.ToString() + y.ToString();
		currentPiece.gameObject.tag = "piece";
		currentPiece.transform.parent = GameObject.Find("Game Board").transform;
		iTween.MoveTo(currentPiece,iTween.Hash("z", 0, "easeType", iTween.EaseType.easeInExpo, "time", 0.4f));
		piece[x,y] = pieceType[0];
		
		moveMade();
		turnTimerStart = Time.time;
		GameObject.Find("Player One Turn Timer").GetComponent<TextMesh>().text = "";
		GameObject.Find("Player Two Turn Timer").GetComponent<TextMesh>().text = "";
	}
		
	void win(string winString, string p) {
		//Highlight the winning combination
		for(int x = 0; x < 3; x++)
		{
			if(PlayerPrefs.GetInt("sound") == 0 && p != "Z")
				GetComponent<AudioSource>().PlayOneShot(score);
			if(p[0] == hostPieceInfo[hostPieceInfo.Length - 1])
				GameObject.Find("Piece " + winString[0 + x] + winString[3 + x]).GetComponent<Renderer>().material = hostSelectedMat;
			else if(p[0] == clientPieceInfo[clientPieceInfo.Length - 1])
				GameObject.Find("Piece " + winString[0 + x] + winString[3 + x]).GetComponent<Renderer>().material = clientSelectedMat;
		}
		
		//Update winners score for both players
		if(p[0] == hostPieceInfo[hostPieceInfo.Length - 1])
			hostScore += 1;
		else if(p[0] == clientPieceInfo[clientPieceInfo.Length - 1])
			clientScore += 1;
		
		//Display updated scores
		GameObject.Find("Player One Score Text").GetComponent<TextMesh>().text = hostScore.ToString();
		GameObject.Find("Player Two Score Text").GetComponent<TextMesh>().text = clientScore.ToString();
		
		string w = isWinner();
		
		//Begin a new round if game win criteria has not been met
		if(w != "")
			StartCoroutine(endGame(w));
		else
			StartCoroutine(newRound());
	}
	
	string isWinner()
	{
		string winner = "";
		
		if(hostScore == scoreLimit)
			winner = "host";
		else if(clientScore == scoreLimit)
			winner = "client";
		
		return winner;		
	}
	
	IEnumerator endGame(string winner) {
		cantMove = true;
		restartingRound = true;
		
		yield return new WaitForSeconds(2);
		
		highlightReset();
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(transition);
		iTween.MoveTo(GameObject.Find("Game Board"),iTween.Hash("y", -5,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		
		yield return new WaitForSeconds(2);
		
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(transition);
		iTween.MoveTo(GameObject.Find("Player One Data Holder"),iTween.Hash("x", -1.25f,"y", 1,"z",-1.5f,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		iTween.MoveTo(GameObject.Find("Player One Score Holder"),iTween.Hash("x", -1.25f,"y", 0.25f,"z",-1.5f,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		iTween.MoveTo(GameObject.Find("Player Two Data Holder"),iTween.Hash("x", 1.25f,"y", 1,"z",-1.5f,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		iTween.MoveTo(GameObject.Find("Player Two Score Holder"),iTween.Hash("x", 1.25f,"y", 0.25f,"z",-1.5f,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		iTween.MoveTo(GameObject.Find("Game Info Text"),iTween.Hash("y", 4.5f,"easeType", iTween.EaseType.easeInExpo, "time", 1));
		
		yield return new WaitForSeconds(2);
		
		if(winner == "host")
		{
			if(singlePlayer == true)
			{
				if(difficulty == "Easy")
					PlayerPrefs.SetInt("eW",PlayerPrefs.GetInt("eW") + 1);
				else if(difficulty == "Medium")
					PlayerPrefs.SetInt("mW",PlayerPrefs.GetInt("mW") + 1);
				else if(difficulty == "Hard")
					PlayerPrefs.SetInt("hW",PlayerPrefs.GetInt("hW") + 1);
			}
			else
			{
				PlayerPrefs.SetInt("P1W",PlayerPrefs.GetInt("P1W") + 1);
				PlayerPrefs.SetInt("P2L",PlayerPrefs.GetInt("P2L") + 1);
			}
			
			GameObject.Find("Player One Win Text").GetComponent<TextMesh>().text = "Winner";
			GameObject.Find("Player Two Win Text").GetComponent<TextMesh>().text = "Loser";
			GameObject.Find("Player One Data Holder").GetComponent<Renderer>().material = hostSelectedMat;
			GameObject.Find("Player One Score Holder").GetComponent<Renderer>().material = hostSelectedMat;
			iTween.MoveTo(GameObject.Find("Player One Win Text"),iTween.Hash("y", 2,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
			iTween.MoveTo(GameObject.Find("Player Two Win Text"),iTween.Hash("y", 2,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
		}
		if(winner == "client")
		{
			if(singlePlayer == true)
			{
				if(difficulty == "Easy")
					PlayerPrefs.SetInt("eL",PlayerPrefs.GetInt("eL") + 1);
				else if(difficulty == "Medium")
					PlayerPrefs.SetInt("mL",PlayerPrefs.GetInt("mL") + 1);
				else if(difficulty == "Hard")
					PlayerPrefs.SetInt("hL",PlayerPrefs.GetInt("hL") + 1);
			}
			else
			{
				PlayerPrefs.SetInt("P1L",PlayerPrefs.GetInt("P1L") + 1);
				PlayerPrefs.SetInt("P2W",PlayerPrefs.GetInt("P2W") + 1);
			}
			
			GameObject.Find("Player One Win Text").GetComponent<TextMesh>().text = "Loser";
			GameObject.Find("Player Two Win Text").GetComponent<TextMesh>().text = "Winner";
			GameObject.Find("Player Two Data Holder").GetComponent<Renderer>().material = clientSelectedMat;
			GameObject.Find("Player Two Score Holder").GetComponent<Renderer>().material = clientSelectedMat;
			iTween.MoveTo(GameObject.Find("Player One Win Text"),iTween.Hash("y", 2,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
			iTween.MoveTo(GameObject.Find("Player Two Win Text"),iTween.Hash("y", 2,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
		}
		
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(moveMadeSound);
		
		yield return new WaitForSeconds(0.75f);
		
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(moveMadeSound);
		
		iTween.MoveTo(GameObject.Find("Rematch"),iTween.Hash("x", -3.75f,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
		iTween.MoveTo(GameObject.Find("Menu"),iTween.Hash("x", 3.75f,"easeType", iTween.EaseType.easeInOutExpo, "time", 1));
		
		gameOver = true;
	}
	
	IEnumerator newRound() {
		GameObject[] pieces;
		
		cantMove = true;
		restartingRound = true;
		highlightReset();
		yield return new WaitForSeconds(1);
		
		if(PlayerPrefs.GetInt("sound") == 0 && restartingGame == false)
			GetComponent<AudioSource>().PlayOneShot(transition);
		
		restartingGame = false;
		iTween.MoveTo(GameObject.Find("Game Board"),iTween.Hash("y", -5,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		
		yield return new WaitForSeconds(2);
		
		for(int x = 0; x < 3; x++)
			for(int y = 0; y < 3; y++)
				piece[x,y] = '\0';
		
		pieces = GameObject.FindGameObjectsWithTag("piece");
		for(int count = 0; count < pieces.Length; count++)
			Destroy(pieces[count]);
		
		if(PlayerPrefs.GetInt("sound") == 0)
			GetComponent<AudioSource>().PlayOneShot(transition);
		iTween.MoveTo(GameObject.Find("Game Board"),iTween.Hash("y", 0,"easeType", iTween.EaseType.easeInOutExpo, "time", 2));
		
		yield return new WaitForSeconds(2);	
		turnTimerStart = Time.time;
		highlightChange();
		cantMove = false;
		restartingRound = false;
	}
	
	void moveMade() {
		string s = "";
		bool draw = drawCheck();
		
		if(!myTurn)
			s = winCheck(AIpiece);
		else
			s = winCheck(myPiece);
		
		if (s != "")
		{
			if(!myTurn)
				win(s,AIpiece.ToString());
			else
				win(s,myPiece.ToString());
		}
		else if(draw)
		{
			win("","Z");
		}
						
		myTurn = !myTurn;
		if(s == "" && !draw)
			highlightChange();
	}
	
	void highlightChange() {
		//Assign materials to the player data holders
		if (myTurn)
		{
			GameObject.Find("Player One Data Holder").GetComponent<Renderer>().material = hostSelectedMat;
			GameObject.Find("Player Two Data Holder").GetComponent<Renderer>().material = clientSmallMat;
		}
		else
		{
			GameObject.Find("Player One Data Holder").GetComponent<Renderer>().material = hostSmallMat;
			GameObject.Find("Player Two Data Holder").GetComponent<Renderer>().material = clientSelectedMat;
		}
	}
	
	void highlightReset() {
		//Reset highlights
		GameObject.Find("Player One Data Holder").GetComponent<Renderer>().material = hostSmallMat;
		GameObject.Find("Player Two Data Holder").GetComponent<Renderer>().material = clientSmallMat;
	}
	
	void timerRanOut() {
		bool found = false;
		
		for(int x = 0; x < 3; x++)
			for(int y = 0; y < 3; y++)
				if(piece[x,y] == '\0' && found == false)
				{
					makeMove(myPiece.ToString(),x,y);
					found = true;
				}				
	}
	
	void AIMove() {
		bool found = false;
		
	
		if(difficulty != "Easy") {
		//AI Y
		for(int y = 0; y < 3 && found == false; y++)
		{
			if(piece[0,y] == AIpiece && piece[1,y] == AIpiece && piece[2,y] == '\0')
			{
				makeMove(AIpiece.ToString(),2,y);
				found = true;
			}
			if(piece[0,y] == '\0' && piece[1,y] == AIpiece && piece[2,y] == AIpiece)
			{
				makeMove(AIpiece.ToString(),0,y);
				found = true;
			}
			if(piece[0,y] == AIpiece && piece[1,y] == '\0' && piece[2,y] == AIpiece)
			{
				makeMove(AIpiece.ToString(),1,y);
				found = true;
			}	
		}
		//AI X
		for(int x = 0; x < 3 && found == false; x++)
		{
			if(piece[x,0] == AIpiece && piece[x,1] == AIpiece && piece[x,2] == '\0')
			{
				makeMove(AIpiece.ToString(),x,2);
				found = true;
			}
			if(piece[x,0] == '\0' && piece[x,1] == AIpiece && piece[x,2] == AIpiece)
			{
				makeMove(AIpiece.ToString(),x,0);
				found = true;
			}
			if(piece[x,0] == AIpiece && piece[x,1] == '\0' && piece[x,2] == AIpiece)
			{
				makeMove(AIpiece.ToString(),x,1);
				found = true;
			}	
		}
		//AI Diag
		if(piece[0,0] == AIpiece && piece[1,1] == AIpiece && piece[2,2] == '\0' && found == false)
		{
			makeMove(AIpiece.ToString(),2,2);
			found = true;
		}	
		if(piece[0,0] == '\0' && piece[1,1] == AIpiece && piece[2,2] == AIpiece && found == false)
		{
			makeMove(AIpiece.ToString(),0,0);
			found = true;
		}	
		if(piece[2,0] == AIpiece && piece[1,1] == AIpiece && piece[0,2] == '\0' && found == false)
		{
			makeMove(AIpiece.ToString(),0,2);
			found = true;
		}	
		if(piece[2,0] == '\0' && piece[1,1] == AIpiece && piece[0,2] == AIpiece && found == false)
		{
			makeMove(AIpiece.ToString(),2,0);
			found = true;
		}			
		
		//Human Y
		for(int y = 0; y < 3 && found == false; y++)
		{
			if(piece[0,y] == myPiece && piece[1,y] == myPiece && piece[2,y] == '\0')
			{
				makeMove(AIpiece.ToString(),2,y);
				found = true;
			}
			if(piece[0,y] == '\0' && piece[1,y] == myPiece && piece[2,y] == myPiece)
			{
				makeMove(AIpiece.ToString(),0,y);
				found = true;
			}	
			if(piece[0,y] == myPiece && piece[1,y] == '\0' && piece[2,y] == myPiece)
			{
				makeMove(AIpiece.ToString(),1,y);
				found = true;
			}		
		}
		//Human X
		for(int x = 0; x < 3 && found == false; x++)
		{
			if(piece[x,0] == myPiece && piece[x,1] == myPiece && piece[x,2] == '\0')
			{
				makeMove(AIpiece.ToString(),x,2);
				found = true;
			}
			if(piece[x,0] == '\0' && piece[x,1] == myPiece && piece[x,2] == myPiece)
			{
				makeMove(AIpiece.ToString(),x,0);
				found = true;
			}
			if(piece[x,0] == myPiece && piece[x,1] == '\0' && piece[x,2] == myPiece)
			{
				makeMove(AIpiece.ToString(),x,1);
				found = true;
			}
		}
		//Human Diag
		if(piece[0,0] == myPiece && piece[1,1] == myPiece && piece[2,2] == '\0' && found == false)
		{
			makeMove(AIpiece.ToString(),2,2);
			found = true;
		}	
		if(piece[0,0] == '\0' && piece[1,1] == myPiece && piece[2,2] == myPiece && found == false)
		{
			makeMove(AIpiece.ToString(),0,0);
			found = true;
		}	
		if(piece[2,0] == myPiece && piece[1,1] == myPiece && piece[0,2] == '\0' && found == false)
		{
			makeMove(AIpiece.ToString(),0,2);
			found = true;
		}	
		if(piece[2,0] == '\0' && piece[1,1] == myPiece && piece[0,2] == myPiece && found == false)
		{
			makeMove(AIpiece.ToString(),2,0);
			found = true;
		}		
		}
		
		if(difficulty == "Hard")
		{
			if(piece[0,0] == myPiece && piece[1,2] == myPiece && piece[1,1] == '\0' && found == false|| piece[2,0] == myPiece && piece[1,2] == myPiece && piece[1,1] == '\0' && found == false||piece[2,0] == myPiece && piece[0,1] == myPiece && piece[1,1] == '\0' && found == false||piece[2,2] == myPiece && piece[0,1] == myPiece && piece[1,1] == '\0' && found == false||piece[2,2] == myPiece && piece[1,0] == myPiece && piece[1,1] == '\0' && found == false||piece[0,2] == myPiece && piece[1,0] == myPiece && piece[1,1] == '\0' && found == false||piece[0,0] == myPiece && piece[2,1] == myPiece && piece[1,1] == '\0' && found == false||piece[0,2] == myPiece && piece[2,1] == myPiece && piece[1,1] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),1,1);
				found = true;
			}
			if(piece[1,0] == myPiece && piece[0,1] == myPiece && piece[0,0] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),0,0);
				found = true;
			}
			if(piece[1,0] == myPiece && piece[2,1] == myPiece && piece[2,0] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),2,0);
				found = true;
			}
			if(piece[1,2] == myPiece && piece[2,1] == myPiece && piece[2,2] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),2,2);
				found = true;
			}
			if(piece[0,1] == myPiece && piece[1,2] == myPiece && piece[0,2] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),0,2);
				found = true;
			}
			
			if(piece[1,1] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),1,1);
				found = true;
			}
		}
		
		while(found == false)
		{
			AIX = Random.Range(0,3);
			AIY = Random.Range(0,3);
					
			if(piece[AIX,AIY] == '\0' && found == false)
			{
				makeMove(AIpiece.ToString(),AIX,AIY);
				found = true;
			}
		}
	}
	
	void playerAssign() {
		//Assign players pieces and turns
		myPiece = hostPieceInfo[hostPieceInfo.Length - 1];
		AIpiece = clientPieceInfo[clientPieceInfo.Length - 1];
		myTurn = true;
		
		if(hostPieceInfo[0] == 'R')
		{
			hostMat = redToony;
			hostSmallMat = redToonySmall;
			hostSelectedMat = redToonySelected;
		}
		else if(hostPieceInfo[0]== 'B')
		{
			hostMat = blueToony;
			hostSmallMat = blueToonySmall;
			hostSelectedMat = blueToonySelected;
		}
			
		if(clientPieceInfo[0] == 'R')
		{
			clientMat = redToony;
			clientSmallMat = redToonySmall;
			clientSelectedMat = redToonySelected;
		}
		else if(clientPieceInfo[0]== 'B')
		{
			clientMat = blueToony;
			clientSmallMat = blueToonySmall;
			clientSelectedMat = blueToonySelected;
		}
	}
	
	string winCheck(char p) {
		//Check if last move has won the game
		string winString = "";
		
		for(int x = 0; x < 3; x++)
			if(piece[x,0] == p && piece[x,1] == p && piece[x,2] == p)
				winString = (x.ToString() + x.ToString() + x.ToString() + "012");
		
		for(int y = 0; y < 3; y++)
			if(piece[0,y] == p && piece[1,y] == p && piece[2,y] == p)
				winString = "012" + y.ToString() + y.ToString() + y.ToString();
		
		if(piece[0,0] == p && piece[1,1] == p && piece[2,2] == p)
			winString = "012012";
		
		if(piece[2,0] == p && piece[1,1] == p && piece[0,2] == p)
			winString = "210012";
		
		return winString;
	}
	
	bool drawCheck() {
		bool draw = true;
		
		for(int x = 0; x < 3 && draw == true; x++)
		{
			for(int y = 0; y < 3; y++)
			{
				if(piece[x,y] == '\0')
					draw = false;
			}
		}
				
		return draw;		
	}
}