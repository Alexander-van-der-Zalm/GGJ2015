using UnityEngine;
using System.Collections;

public class Colorpallet : MonoBehaviour {

	public Color[] neutralCol;
	public Color teamOneCol;
	public Color teamTwoCol;

    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void SetVertexColor(Color color)
    {
        int count = mesh.vertexCount;
        Color[] newColors = new Color[count];
        for(int i = 0; i < count;i++)
        {
            newColors[i] = color;
        }
        mesh.colors = newColors;
    }
}
