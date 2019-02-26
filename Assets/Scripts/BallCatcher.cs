using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatcher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall")
        {
            Destroy(other.gameObject);
        }
    }
}
