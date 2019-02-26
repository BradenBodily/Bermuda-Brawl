using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 900.0f)] float m_rate = 1.0f;

    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(m_rate * Time.deltaTime, 0.0f, m_rate * Time.deltaTime * 1.3f);
    }
}
