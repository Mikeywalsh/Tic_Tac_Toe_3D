using UnityEngine;
using System.Collections;

public class boardMove : MonoBehaviour {

	float startTime;
	bool moved;
	
	void Start () {
		startTime = Time.time;
	}
	
	void Update () {
		
		if((Time.time - startTime) > 1.25f && moved == false)
		{
			iTween.MoveTo(gameObject,iTween.Hash("y", 0, "easeType", iTween.EaseType.easeInExpo, "time", 1.5f));
			moved = true;
		}
	}
}
