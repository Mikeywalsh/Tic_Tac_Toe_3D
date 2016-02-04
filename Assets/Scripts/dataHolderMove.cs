using UnityEngine;
using System.Collections;

public class dataHolderMove : MonoBehaviour {
	
	float startTime;
	public int multiplier;
	bool moved;
	bool played1;
	bool played2;
	public AudioClip transition;
	public AudioClip smallTransition;
	
	void Start () {		
		startTime = Time.time;
	}
	
	void Update () {
		if((Time.time - startTime) > 1.25f && moved == false)
		{
			if(PlayerPrefs.GetInt("sound") == 0)
				GetComponent<AudioSource>().PlayOneShot(transition);
			iTween.MoveTo(gameObject,iTween.Hash("x", 4.77f * multiplier, "y", 3.42f, "z", 1.146f, "easeType", iTween.EaseType.easeInExpo, "time", 1));
			iTween.ScaleTo(gameObject,iTween.Hash("x", 1, "y", 1, "z", 1, "easeType", iTween.EaseType.easeInExpo, "time", 1));
			moved = true;
		}
		
		if((Time.time - startTime) > 2.25f && played1 == false)
		{
			if(PlayerPrefs.GetInt("sound") == 0)
				GetComponent<AudioSource>().PlayOneShot(smallTransition);
			played1 = true;
		}
		
		if((Time.time - startTime) > 3.25f && played2 == false)
		{
			if(PlayerPrefs.GetInt("sound") == 0)
				GetComponent<AudioSource>().PlayOneShot(smallTransition);
			played2 = true;
		}
	}
}
