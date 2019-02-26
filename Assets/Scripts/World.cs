using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] Transform m_min = null;
    [SerializeField] Transform m_max = null;

    public Vector3 WrapPosition(Vector3 position)
    {
        Vector3 newPosition = position;

        if (position.x > m_max.position.x) newPosition.x = m_min.position.x + (position.x - m_max.position.x);
        else if (position.x < m_min.position.x) newPosition.x = m_max.position.x + (m_min.position.x - position.x);

        if (position.z > m_max.position.z) newPosition.z = m_min.position.z + (position.z - m_max.position.z);
        else if (position.z < m_min.position.z) newPosition.z = m_max.position.z + (m_min.position.z - position.z);

        return newPosition;
    }
}
