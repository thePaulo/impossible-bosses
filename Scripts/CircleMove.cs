using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CircleMove : NetworkBehaviour {
    public Transform[] spots;
    //public GameObject enemy;
    public GameObject fenix;

    GameObject[] players = new GameObject[5];
    Transform target;

    Transform[] gotos = new Transform[2];

    Transform lastSpot;
    float time;
    int times = 0;

    int entered = 0;

    float tempoprachegar = 0;
    Vector3 startPos;

    bool fenixTrigger = false;
	// Use this for initialization
	void Start () {
        target = spots[Random.Range(0,3)];
        
        RenderSettings.fog = true;
        

        lastSpot = spots[3];

        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        tempoprachegar += Time.deltaTime / 9;

        players = GameObject.FindGameObjectsWithTag("Player");//buscando jogadores no game

        transform.position = Vector3.MoveTowards(transform.position,target.position,0.3f);
        //transform.position = Vector3.MoveTowards(transform.position, target.position, tempoprachegar);
        //moveThaMotherFucker();
        //transform.position = Vector3.Lerp(startPos, target.position, tempoprachegar);

        Debug.Log("PRINTA AKI FILHO DA PUTA "+ (Vector3.Distance(transform.position, target.position)));

        if (times == 1 && entered ==0)//depois de ter entrado em um circulo e nao iterou isso ainda
        {
            fenixTrigger = true;
            entered++;
        }
        if(fenixTrigger == true)//spawne a spell
        {
            //spawnFenix();
            fenixTrigger = false;
            //NetworkServer.Spawn(Instantiate(fenix, target.position, transform.rotation));
        }

        //moveThaMotherFucker();

        if( transform.position == target.position && times < 2 )
        {
            if (times != 1)
            {
                target = spots[Random.Range(0, 3)];
            }
            if(times == 1) { 
                target = lastSpot;
            }

            times++;
        }
        if(times >= 2)//depois de passar por 2 spots
        {

            foreach (GameObject p in players)
            {
                if (Vector3.Distance(p.transform.position, transform.position) <= 25f)
                {
                    p.gameObject.SendMessage("harm", 3000);
                }
            }

            RenderSettings.fog = false;
            Destroy(gameObject,2.5f);
        }

        checkDistance();

	}
    void checkDistance()
    {
        

        if (players.Length > 0)
        {
            foreach (GameObject p in players)
            {
                if (Vector3.Distance(p.transform.position, transform.position) > 25f)
                {
                    p.gameObject.SendMessage("harm", 10);
                }
            }
        }

        //Debug.Log("DISTANCIA BETWEEN US: " + Vector3.Distance(enemy.transform.position, transform.position));
        
    }
    /*
    void spawnFenix()
    {
        //Instantiate(fenix, gotos[1].position, Quaternion.identity);

        GameObject spell = Instantiate(fenix, new Vector3(0,0,0), Quaternion.identity);
        
        spell.GetComponent<Rigidbody>().transform.LookAt(lastSpot);

        //spell.GetComponent<Rigidbody>().velocity = spell.GetComponent<Rigidbody>().transform.forward * (Vector3.Distance(spell.transform.position,lastSpot.position))/5;//-transform.forward * 20;









        //spell.GetComponent<Rigidbody>().velocity = spell.GetComponent<Rigidbody>().transform.forward * 3;//-transform.forward * 20;

        NetworkServer.Spawn(spell);
    }

    void moveThaMotherFucker()
    {
        
        //transform.position = Vector3.MoveTowards(transform.position, target.position, (Vector3.Distance(transform.position, target.position))/5);


        //transform.position = Vector3.Lerp(transform.position, target.position, 0.3f);//best so far =/
        
        
        //transform.position = Vector3.Lerp(transform.position, target.position, tempoprachegar);//best so far =/
        //.position = Vector3.MoveTowards(transform.position, target.position, 10f * Time.deltaTime);
    }*/
}
