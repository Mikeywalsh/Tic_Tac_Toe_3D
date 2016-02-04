using UnityEngine;
using System.Collections;

public class gameCreation : MonoBehaviour {
	string difficulty;
	int scoreLimit;
	string hostP;
	string clientP;
	
	public GameObject transparentCross;
	public GameObject transparentRing;
	public GameObject cross;
	public GameObject ring;
	
	public Material redToony;
	public Material redToonySmall;
	public Material redToonySelected;
	public Material blueToony;
	public Material blueToonySmall;
	public Material blueToonySelected;
	
	public AudioClip hover;
	public AudioClip moveMade;
	public AudioClip score;
	public AudioClip transition;
	
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	public void createGame(bool single, string d, int s, string h, string c, string n) {
		scoreLimit = s;
		hostP = h;
		clientP = c;
		
		startGameplay(single,d,n);
	}
	
	public void startGameplay(bool singlePlayer, string difficulty, string playerTwoName) {
		
		Application.LoadLevel(2);
		
		gameLogic addedGameLogic = gameObject.AddComponent<gameLogic>();
		
		//Assign variables for use in the added gameLogic
		if(!singlePlayer)
			addedGameLogic.playerTwoName = playerTwoName;
		
		addedGameLogic.singlePlayer = singlePlayer;
		addedGameLogic.difficulty = difficulty;
		addedGameLogic.scoreLimit = scoreLimit;
		addedGameLogic.hostPieceInfo = hostP;
		addedGameLogic.clientPieceInfo = clientP;
		addedGameLogic.transparentCross = transparentCross;
		addedGameLogic.transparentRing = transparentRing;
		addedGameLogic.cross = cross;
		addedGameLogic.ring = ring;
		addedGameLogic.redToony = redToony;
		addedGameLogic.redToonySmall = redToonySmall;
		addedGameLogic.redToonySelected = redToonySelected;
		addedGameLogic.blueToony = blueToony;
		addedGameLogic.blueToonySmall = blueToonySmall;
		addedGameLogic.blueToonySelected = blueToonySelected;
		addedGameLogic.hoverSound = hover;
		addedGameLogic.moveMadeSound = moveMade;
		addedGameLogic.score = score;
		addedGameLogic.transition = transition;
	}
}
