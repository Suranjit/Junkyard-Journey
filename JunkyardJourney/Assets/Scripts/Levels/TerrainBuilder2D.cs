using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilder2D : MonoBehaviour
{
    [Header("Visual")]
    public Material terrainLineMaterial;
    public float lineWidth = 0.25f;
    public Color terrainColor = new Color(0.45f, 0.3f, 0.15f, 1f);

    public void BuildTerrain(List<TerrainDefinition> terrainDefinitions, Transform terrainRoot)
    {
        if (terrainDefinitions == null)
        {
            Debug.LogWarning("TerrainBuilder2D: No terrain definitions provided.");
            return;
        }

        foreach (TerrainDefinition terrainDefinition in terrainDefinitions)
        {
            BuildSingleTerrain(terrainDefinition, terrainRoot);
        }
    }

    private void BuildSingleTerrain(TerrainDefinition terrainDefinition, Transform terrainRoot)
    {
        if (terrainDefinition == null || terrainDefinition.points == null || terrainDefinition.points.Count < 2)
        {
            Debug.LogWarning("TerrainBuilder2D: Invalid terrain definition skipped.");
            return;
        }

        GameObject terrainObject = new GameObject($"Terrain_{terrainDefinition.id}");
        terrainObject.transform.SetParent(terrainRoot, false);

        Vector2[] points2D = new Vector2[terrainDefinition.points.Count];
        Vector3[] points3D = new Vector3[terrainDefinition.points.Count];

        for (int i = 0; i < terrainDefinition.points.Count; i++)
        {
            Vector2 point = terrainDefinition.points[i].ToVector2();
            points2D[i] = point;
            points3D[i] = new Vector3(point.x, point.y, 0f);
        }

        LineRenderer lineRenderer = terrainObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = points3D.Length;
        lineRenderer.SetPositions(points3D);
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.sortingOrder = 1;

        if (terrainLineMaterial != null)
            lineRenderer.material = terrainLineMaterial;
        else
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startColor = terrainColor;
        lineRenderer.endColor = terrainColor;

        EdgeCollider2D edgeCollider = terrainObject.AddComponent<EdgeCollider2D>();
        edgeCollider.points = points2D;

        Debug.Log($"TerrainBuilder2D: Built terrain '{terrainDefinition.id}' with {points2D.Length} points.");
    }
}
