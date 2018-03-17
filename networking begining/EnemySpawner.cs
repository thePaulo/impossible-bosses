using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    

    public override void OnStartServer()
    {

        //var rotation = Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));

        var enemy = (GameObject)Instantiate(enemyPrefab, new Vector3(0, 25, -60), Quaternion.identity);
        NetworkServer.Spawn(enemy);

    }
}