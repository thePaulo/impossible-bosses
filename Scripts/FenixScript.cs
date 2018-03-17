using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenixScript : MonoBehaviour {
    float timer=0;
    GameObject[] players = new GameObject[5];
    public float distance;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            Debug.Log(Vector3.Distance(p.transform.position, transform.position));
            
        }


        if (timer > 6f)
        {
            foreach (GameObject p in players)
            {
                Debug.Log(Vector3.Distance(p.transform.position, transform.position));
                if (Vector3.Distance(p.transform.position, transform.position) <= distance)
                {
                    //Destroy(p);
                    p.SendMessage("harm", 3000);
                    //Destroy(gameObject,5.5f);
                    
                }
            }
            Destroy(gameObject);
        }
    }
}
