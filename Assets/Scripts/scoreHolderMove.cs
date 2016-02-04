using UnityEngine;
using System.Collections;

public class scoreHolderMove : MonoBehaviour {
	
	public int multiplier;
	float startTime;
	bool moved;
	
	void Start () {	
		startTime = Time.time;
	}
	
	void Update () {
		
		if((Time.time - startTime) > 1.25f && moved == false)
		{
			iTween.MoveTo(gameObject,iTween.Hash("x", 4.24f * multiplier,"y", -1.85f, "easeType", iTween.EaseType.easeInExpo, "time", 2.5f));
			iTween.MoveTo(GameObject.Find("Game Info Text"),iTween.Hash("y", 2.75f,"easeType", iTween.EaseType.easeInExpo, "time", 2.5f));
			moved = true;
		}
	}
}
