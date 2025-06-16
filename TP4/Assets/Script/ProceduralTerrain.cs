using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class ProceduralTerrain : MonoBehaviour
{
    public int width = 80;
    public int height = 80;
    public float scale = 0.1f;
    public float amplitude = 5f;

    private Mesh mesh;
    private MeshCollider meshCollider;

    private void OnValidate()
    {
        GenerateMesh(); // Se relance à chaque changement dans l’inspecteur
    }

    private void Awake()
    {
        GenerateMesh(); // Génère dès que l’objet est chargé
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float y = Mathf.PerlinNoise(x * scale, z * scale) * amplitude;
                int index = z * width + x;
                vertices[index] = new Vector3(x, y, z);
                uvs[index] = new Vector2((float)x / (width - 1), (float)z / (height - 1));
            }
        }

        int t = 0;
        for (int x = 0; x < width - 1; x++)
        {
            for (int z = 0; z < height - 1; z++)
            {
                int i = z * width + x;
                triangles[t++] = i;
                triangles[t++] = i + width;
                triangles[t++] = i + 1;

                triangles[t++] = i + 1;
                triangles[t++] = i + width;
                triangles[t++] = i + width + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        if (meshCollider == null)
            meshCollider = GetComponent<MeshCollider>();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}
