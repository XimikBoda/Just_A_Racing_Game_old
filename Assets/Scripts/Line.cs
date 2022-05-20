using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(gameObject.GetComponent<MeshFilter>().mesh.);
        Vector3[] vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;
        int[,] v_to_t = new int[vertices.Length, 3];

       // for (int i = 0; i < vertices.Length; ++i)
        //    Debug.Log(vertices[i]);

        //   gameObject.GetComponent<MeshFilter>().mesh.ver

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
