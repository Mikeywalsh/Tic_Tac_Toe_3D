using UnityEngine;
using System.Collections;

public class vsMove : MonoBehaviour {

	float startTime;
	
	void Start () {
		startTime = Time.time;
	}
	
	void Update () {
		if((Time.time - startTime) > 1.25f)
		{
			iTween.MoveTo(gameObject,iTween.Hash("y", 2.5f, "easeType", iTween.EaseType.easeInExpo, "time", 1));
			iTween.ScaleTo(gameObject,iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInExpo, "time", 1));
		}
		if(transform.localScale == Vector3.zero)
			GameObject.Destroy(gameObject);
	}
}
