using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,0.1f); // cant rotate negative
		if (transform.eulerAngles.z>=70f){
			transform.eulerAngles=Vector3.zero;
			
		}
	}
}
