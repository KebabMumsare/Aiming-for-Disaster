using UnityEngine;
using System.Collections.Generic;

public class Semicircle : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5f)] float radius = 1f;
    [SerializeField, Range(4, 128)] int segments = 32;
    [SerializeField, Range(1f, 360f)] float arcDegrees = 180f;
    [SerializeField] PolygonCollider2D polyCollider;
    [SerializeField] MeshFilter meshFilter;

    void Awake()
    {
        Rebuild();
    }

    void OnValidate()
    {
        if (polyCollider == null || meshFilter == null)
            return;

        Rebuild();
    }

    void Rebuild()
    {
        Vector2[] points = BuildArcPoints(radius, segments, arcDegrees);
        polyCollider.pathCount = 1;
        polyCollider.SetPath(0, points);

        meshFilter.sharedMesh = BuildArcMesh(radius, segments, arcDegrees);
    }

    Vector2[] BuildArcPoints(float r, int segs, float arcDeg)
    {
        float arcRad = Mathf.Deg2Rad * Mathf.Clamp(arcDeg, 1f, 360f);
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i <= segs; ++i)
        {
            float t = i / (float)segs;
            float angle = arcRad * t;
            points.Add(new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r));
        }

        points.Add(Vector2.zero);          // close the wedge at the center
        return points.ToArray();
    }

    Mesh BuildArcMesh(float r, int segs, float arcDeg)
    {
        float arcRad = Mathf.Deg2Rad * Mathf.Clamp(arcDeg, 1f, 360f);

        Vector3[] vertices = new Vector3[segs + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[segs * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0f);

        for (int i = 0; i <= segs; ++i)
        {
            float t = i / (float)segs;
            float angle = arcRad * t;
            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;

            int idx = i + 1;
            vertices[idx] = new Vector3(x, y, 0f);
            uv[idx] = new Vector2((x / (2f * r)) + 0.5f, y / r);
        }

        for (int i = 0; i < segs; ++i)
        {
            int triIndex = i * 3;
            triangles[triIndex] = 0;
            triangles[triIndex + 1] = i + 1;
            triangles[triIndex + 2] = i + 2;
        }

        Mesh mesh = new Mesh
        {
            name = "ArcMesh",
            vertices = vertices,
            triangles = triangles,
            uv = uv
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}