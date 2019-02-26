using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
	[HeaderAttribute("Water")]
	[SerializeField] [Range(0.0f, 1.0f)] float m_dampening = 0.04f;
	[SerializeField] [Range(1.0f, 90.0f)] float m_simulationFPS = 60.0f;

	[HeaderAttribute("Mesh")]
	[SerializeField] [Range(2, 80)] int m_xMeshVertexNum = 2;
	[SerializeField] [Range(2, 80)] int m_zMeshVertexNum = 2;
	[SerializeField] [Range(1.0f, 80.0f)] float m_xMeshSize = 40.0f;
	[SerializeField] [Range(1.0f, 80.0f)] float m_zMeshSize = 40.0f;

	MeshFilter m_meshFilter = null;
	MeshCollider m_meshCollider = null;
	Mesh m_mesh = null;
	Vector3[] m_vertices;

	int frame = 0;
	float[,] m_buffer1;
	float[,] m_buffer2;

	float m_simulationTime = 0.0f;

	void Start()
	{
		m_meshFilter = GetComponent<MeshFilter>();
		m_meshCollider = GetComponent<MeshCollider>();
		m_mesh = m_meshFilter.mesh;
		MeshGenerator.Plane(m_meshFilter, m_xMeshSize, m_zMeshSize, m_xMeshVertexNum, m_zMeshVertexNum);
		m_vertices = m_mesh.vertices;

		m_buffer1 = new float[m_xMeshVertexNum, m_zMeshVertexNum];
		m_buffer2 = new float[m_xMeshVertexNum, m_zMeshVertexNum];
	}

	void Update()
	{
		m_simulationTime = m_simulationTime + Time.deltaTime;
		while (m_simulationTime > (1.0f / m_simulationFPS))
		{
			if (frame % 2 == 0) UpdateSimulation(ref m_buffer1, ref m_buffer2);
			else UpdateSimulation(ref m_buffer2, ref m_buffer1);
			frame++;
			m_simulationTime = m_simulationTime - (1.0f / m_simulationFPS);
		}

		for (int i = 0; i < m_xMeshVertexNum; i++)
		{
			for (int j = 0; j < m_zMeshVertexNum; j++)
			{
				float height = (frame % 2 == 0) ? m_buffer1[i, j] : m_buffer2[i, j];
				m_vertices[i + (j * m_xMeshVertexNum)].y = height;
			}
		}

		m_mesh.vertices = m_vertices;
		m_mesh.RecalculateNormals();
		m_mesh.RecalculateBounds();
		m_meshCollider.sharedMesh = m_mesh;
	}

	void UpdateSimulation(ref float[,] current, ref float[,] previous)
	{
        for(int x = 0; x < previous.GetLength(0); x++)
        {
            for(int y = 0; y < previous.GetLength(1); y++)
            {
                float numPoints = 0.0f;
                float sum = 0.0f;

                if(x > 0)
                {
                    sum += previous[x - 1, y];
                    numPoints++;
                    if(x > 1)
                    {
                        sum += previous[x - 2, y];
                        numPoints++;
                    }
                    if(y > 0)
                    {
                        sum += previous[x - 1, y - 1];
                        numPoints++;
                    }
                    if(y < previous.GetLength(1) - 1)
                    {
                        sum += previous[x - 1, y + 1];
                        numPoints++;
                    }
                }
                if (x < previous.GetLength(0) - 1)
                {
                    sum += previous[x + 1, y];
                    numPoints++;

                    if(x < previous.GetLength(0) - 2)
                    {
                        sum += previous[x + 2, y];
                        numPoints++;
                    }

                    if(y > 0)
                    {
                        sum += previous[x + 1, y - 1];
                        numPoints++;
                    }
                    if(y < previous.GetLength(1) - 1)
                    {
                        sum += previous[x + 1, y + 1];
                        numPoints++;
                    }
                }
                if(y > 0)
                {
                    sum += previous[x, y - 1];
                    numPoints++;

                    if(y > 1)
                    {
                        sum += previous[x, y - 2];
                        numPoints++;
                    }
                }
                if(y < previous.GetLength(1) - 1)
                {
                    sum += previous[x, y + 1];
                    numPoints++;

                    if(y < previous.GetLength(1) - 2)
                    {
                        sum += previous[x, y + 2];
                        numPoints++;
                    }
                }
                    //previous[x - 2, y] +
                    //previous[x + 2, y] +
                    //previous[x, y - 2] +
                    //previous[x, y + 2] +
                    //previous[x - 1, y] +
                    //previous[x + 1, y] +
                    //previous[x, y - 1] +
                    //previous[x, y + 1] +
                    //previous[x - 1, y - 1] +
                    //previous[x + 1, y + 1] +
                    //previous[x - 1, y + 1] +
                    //previous[x + 1, y - 1];
                sum /= numPoints / 2;
                sum -= current[x, y];
                sum *= m_dampening;
                current[x, y] = sum;
            }
        }
	}

	public void Touch(Ray ray, float strength)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			MeshCollider meshCollider = raycastHit.collider as MeshCollider;
			if (meshCollider == m_meshCollider)
			{
				int[] triangles = m_mesh.triangles;
				int index = triangles[raycastHit.triangleIndex * 3];
				int x = index % m_xMeshVertexNum;
				int z = index / m_xMeshVertexNum;
				m_buffer1[x, z] = strength;
			}
		}
	}
}
