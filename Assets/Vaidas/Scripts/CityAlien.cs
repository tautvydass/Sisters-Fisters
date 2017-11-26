using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityAlien : MonoBehaviour {
	//public float height;
	private float TimerTillAttack;
	//private float Time;
	private float CustomDelta;
	private bool Attacking;
	private bool CoGo;
	private ParticleSystem PS;

	// Use this for initialization
	void Start () {
		TimerTillAttack=Random.Range(2f,8f);
		PS=GetComponent<ParticleSystem>();
		PS.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		//print(CustomDelta);
		if (!Attacking){
			CustomDelta+=Time.deltaTime;
		} else {
			if (!CoGo){
				StartCoroutine(AttackLazer());
			}
		}
		Movement();
		if(TimerTillAttack<=0){
			recount();
		}
	}
	
	void Movement(){
		transform.position=new Vector3(Mathf.Sin(CustomDelta*0.3f)*10,transform.position.y,transform.position.z);
		TimerTillAttack-=Time.deltaTime;
	}
	
	void recount(){
		TimerTillAttack=Random.Range(3f,10f);
		Attacking=true;
	}
	
	IEnumerator AttackLazer(){
		PS.Play();
		CoGo=true;
		yield return new WaitForSeconds(3f);
		PS.Stop();
		Attacking=false;
		CoGo=false;
	}
	
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player" && Attacking){
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 12);
		}
    }
	

	
	
}
