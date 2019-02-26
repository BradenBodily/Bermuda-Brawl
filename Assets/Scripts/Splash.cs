using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
	[SerializeField] [Range(-5.0f, 5.0f)] float m_strength = 3.0f;
	[SerializeField] Water m_water = null;

    public void SetWater(Water newWater)
    {
        m_water = newWater;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(transform.position, Vector3.down);
        m_water.Touch(ray, m_strength);
    }
}
