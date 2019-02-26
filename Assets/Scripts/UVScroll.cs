using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    [SerializeField] Vector2 m_uvRate = Vector2.zero;

    Renderer m_renderer = null;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 uv = m_renderer.materials[0].GetTextureOffset("_MainTex");
        uv = uv + m_uvRate * Time.deltaTime;
        m_renderer.materials[0].SetTextureOffset("_MainTex", uv);
    }
}
