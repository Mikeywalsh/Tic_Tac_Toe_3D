using UnityEngine;
using System.Collections;

public class logoMove : MonoBehaviour {
	
	float startTime;
	bool moved;
	bool played;
	public float delay;
	public AudioClip enterScreen;
	
	void Start () {
		moved = false;
		played = false;
		startTime = Time.time;
	}
	
	void Update () {
		
		if((Time.time - (startTime + delay)) > 0f && moved == false)
		{
			iTween.MoveTo(gameObject,iTween.Hash("z", 4.5f, "easeType", iTween.EaseType.easeInExpo, "time", 1));
			moved = true;
		}
		
		if((Time.time -(startTime + delay)) >= 1f && played == false)
		{
			if(PlayerPrefs.GetInt("sound") == 0)
				GetComponent<AudioSource>().PlayOneShot(enterScreen);
			played = true;
		}
	}
}
