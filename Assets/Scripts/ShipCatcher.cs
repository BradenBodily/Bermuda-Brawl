using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCatcher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "P1Ship")
        {
            UIManager.Instance.SinkEndGame("Player Two");
        }
        else if (other.tag == "P2Ship")
        {
            UIManager.Instance.SinkEndGame("Player One");
        }
    }
}
