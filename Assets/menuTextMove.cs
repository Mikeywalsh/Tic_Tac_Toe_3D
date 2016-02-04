using UnityEngine;
using System.Collections;

public class menuTextMove : MonoBehaviour {
	
	public float x;
	public float y;
	public bool exitsVertically;
	float delay;
	float startTime;
	bool started;
	public AudioClip menuTransition;
	
	void Start () {	
		startTime = Time.time;
		
		if (gameObject.tag == "menuItem0")
			delay = 0.1f;
		else if (gameObject.tag == "menuItem1")
			delay = 0.5f;
		else if (gameObject.tag == "menuItem2")
			delay = 0.9f;
		else if (gameObject.tag == "menuItem3")
			delay = 1.3f; 
	}
	
	void Update () {
		if((Time.time - startTime) - delay > 2.5f && started == false && gameObject.name.Contains("(main)"))
		{
			enterScreen();
			if(PlayerPrefs.GetInt("sound") == 0)
				GetComponent<AudioSource>().PlayOneShot(menuTransition);
			started = true;
		}
	}
	
	public void enterScreen()
	{
		startTime = Time.time;
		iTween.MoveTo(gameObject,iTween.Hash("x", x, "y", y, "time", 1, "easeType", iTween.EaseType.easeInExpo));
	}
	
	public void leaveScreen(bool clicked)
	{
		int direction;
		
		if (clicked == true)
			direction = -1;
		else
			direction = 1;
		
		if(exitsVertically == true)			
			iTween.MoveTo(gameObject,iTween.Hash("y", -7, "time", 0.7f, "easeType", iTween.EaseType.easeInExpo));
		else
			iTween.MoveTo(gameObject,iTween.Hash("x", 25 * direction, "time", 1, "easeType", iTween.EaseType.easeInExpo));
	}
}
