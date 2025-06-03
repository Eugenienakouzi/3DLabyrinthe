using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ProceduralTerrain : MonoBehaviour
{
    private const int width = 50;
    private const int height = 50;

    public float scale = 0.1f;
    private float amplitude = 5f;

    private float deformationStrength = 1f;

    public RayCast raycast;

    private const float radius = 1f;

    private Mesh mesh;
    private MeshCollider meshCollider;

    public Vector3[] vertices;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];

        // Génération des vertices avec Perlin Noise
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float y = Mathf.PerlinNoise(x * scale, z * scale) * amplitude;
                vertices[z * width + x] = new Vector3(x, y, z);
            }
        }

        // Génération des triangles
        int index = 0;
        for (int x = 0; x < width - 1; x++)
        {
            for (int z = 0; z < height - 1; z++)
            {
                int topLeft = z * width + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + width;
                int bottomRight = bottomLeft + 1;

                triangles[index++] = topLeft;
                triangles[index++] = bottomLeft;
                triangles[index++] = topRight;

                triangles[index++] = topRight;
                triangles[index++] = bottomLeft;
                triangles[index++] = bottomRight;
            }
        }

        // Ajout des UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float u = (float)x / (width - 1);
                float v = (float)z / (height - 1);
                uvs[z * width + x] = new Vector2(u, v);
            }
        }

        // Assignation au mesh
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    void Update()
    {
        DeformJob deformJob = new DeformJob
        {
            vertices = new NativeArray<Vector3>(vertices, Allocator.TempJob),
            hitPoint = raycast.hitPoint,
            radius = radius,
            strength = deformationStrength
        };

        JobHandle jobHandle = deformJob.Schedule(vertices.Length, 64);
        jobHandle.Complete();

        vertices = deformJob.vertices.ToArray();
        mesh.vertices = vertices;
        mesh.RecalculateNormals();

        // Mise à jour du mesh
        GetComponent<MeshFilter>().mesh = null;
        GetComponent<MeshFilter>().mesh = mesh;

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }

    [BurstCompile]
    public struct DeformJob : IJobParallelFor
    {
        public NativeArray<Vector3> vertices;
        public Vector3 hitPoint;
        public float radius;
        public float strength;

        public void Execute(int index)
        {
            Vector3 vertex = vertices[index];
            float distance = Vector3.Distance(vertex, hitPoint);
            if (distance < radius)
            {
                vertex.y += strength;
                vertices[index] = vertex;
            }
        }
    }
}
