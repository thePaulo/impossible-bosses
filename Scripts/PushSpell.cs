using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PushSpell : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody>().velocity = transform.forward * 15;
        //GetComponent<Rigidbody>().velocity = GetComponentInParent<Transform>().forward * 15;
        GetComponent<Rigidbody>().AddForce(transform.forward * 2);
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Fire");
        GameObject pla = GameObject.Find("Player 4");
        foreach(GameObject x in objetos)
        {
            x.GetComponent<Rigidbody>().velocity = pla.transform.forward * 15;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
