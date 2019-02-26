using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] GameObject minLocation;
    [SerializeField] GameObject maxLocation;
    [SerializeField] float minCameraY;
    [SerializeField] float maxCameraY;
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] float responseTime;
    [SerializeField] float sizeOfShip = 5.0f;

    Vector3 center;
    float maxDistance;
    float playersDifference;
    float playerStartingDistance;
    float cameraRange;
    
	void Start ()
    {
        playerStartingDistance = (player1.transform.position - player2.transform.position).magnitude;
        maxDistance = (maxLocation.transform.position - minLocation.transform.position).magnitude;
        cameraRange = maxCameraY - minCameraY;
	}
	
	void Update () {

        center = (player2.transform.position + player1.transform.position) / 2;

        Vector3 p1Location = player1.transform.position + (player1.velocity * Time.deltaTime);
        Vector3 p2Location = player2.transform.position + (player2.velocity * Time.deltaTime);
        
        float playersDifference = (p2Location - p1Location).magnitude + (sizeOfShip * 2);
        float latePlayerDistance = playersDifference - playerStartingDistance;

        if(playersDifference <= playerStartingDistance)
        {
            center.y = minCameraY;
        }
        else if(latePlayerDistance >= (maxDistance + (sizeOfShip * 2))) // max distance
        {
            center.y = maxCameraY;
        }
        else
        {
            float cameraPercent = latePlayerDistance / (maxDistance + (sizeOfShip * 2));
            center.y = minCameraY + (cameraRange * cameraPercent);
        }

        transform.position = Vector3.Lerp(transform.position, center, responseTime * Time.deltaTime);
    }
}
