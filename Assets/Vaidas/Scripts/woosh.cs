using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woosh : MonoBehaviour {
	public float height;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=new Vector3(transform.position.x,Mathf.Sin(Time.time*0.3f)*height,transform.position.z);
		transform.Rotate(0,0,0.1f);
	}
}
