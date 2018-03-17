using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManagerScript : NetworkBehaviour {
    int hp=100;
    // Use this for initialization

    public GameObject ragnaros;
    /*
    public override void OnStartServer()
    {
        GameObject enemy = (GameObject)Instantiate(ragnaros, new Vector3(0, 25, -60), Quaternion.identity);
        NetworkServer.Spawn(enemy);
    }*/

    /*private void Start()
    {
        GameObject enemy = (GameObject)Instantiate(ragnaros, new Vector3(0, 25, -60), Quaternion.identity);
        NetworkServer.Spawn(enemy);
    }*/

    private void OnGUI()
    {
        //GUI.Label(new Rect(70, 80, 100, 20), "BOSS HP: "+hp+"%");
    }

}
