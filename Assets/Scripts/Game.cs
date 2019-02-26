using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : Singleton<Game> {
    
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] GameObject player1Spawn;
    [SerializeField] GameObject player2Spawn;
    [SerializeField] GameObject island;
    [SerializeField] GameObject islandMin;
    [SerializeField] GameObject islandMax;
    [SerializeField] GameObject islandSpawnMin;
    [SerializeField] GameObject islandSpawnMax;    

    public void PutOutFires()
    {
        player1.PutOutFires();
        player2.PutOutFires();
    }  
    
    public void ResetGame()
    {
        player1.hitPoints = 400;
        player2.hitPoints = 400;
        player1.goldCount = 0;
        player2.goldCount = 0;
        player1.transform.position = player1Spawn.transform.position;
        player1.transform.rotation = player1Spawn.transform.rotation;
        player2.transform.position = player2Spawn.transform.position;
        player2.transform.rotation = player2Spawn.transform.rotation;
        UIManager.Instance.SetPlayerOneHP((int)player1.hitPoints);
        UIManager.Instance.SetPlayerTwoHP((int)player2.hitPoints);
        UIManager.Instance.SetPlayerOneGold((int)0);
        UIManager.Instance.SetPlayerTwoGold((int)0);
        PutOutFires();
    }    

    public Player GetPlayer1()
    {
        return player1;
    }

    public Player GetPlayer2()
    {
        return player2;
    }

    public void SpawnIsland()
    {
        GameObject currentIsland = GameObject.FindGameObjectWithTag("Island");

        if (currentIsland)
        {
            currentIsland.transform.position = GetIslandPosition();
        }
        else
        {
            GameObject newIsland = Instantiate(island);
            newIsland.transform.position = GetIslandPosition();
        }
    }

    private Vector3 GetIslandPosition()
    {
        Vector3 islandPosition = Vector3.zero;

        do
        {
            islandPosition.x = Random.Range(islandMin.transform.position.x, islandMax.transform.position.x);
            islandPosition.y = Random.Range(islandMin.transform.position.y, islandMax.transform.position.y);
            islandPosition.z = Random.Range(islandMin.transform.position.z, islandMax.transform.position.z);
        }
        while ((islandPosition.x >= islandSpawnMin.transform.position.x && islandPosition.x <= islandSpawnMax.transform.position.x) &&
                (islandPosition.z >= islandSpawnMin.transform.position.z && islandPosition.z <= islandSpawnMax.transform.position.z));

        return islandPosition;
    }
}
