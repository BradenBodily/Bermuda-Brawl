using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : Singleton<GoldManager> {

    [SerializeField] List<GameObject> m_goldThings;

	public void ResetGold()
    {
        foreach(GameObject gameObject in m_goldThings)
        {
            gameObject.SetActive(true);
        }
    }
}
