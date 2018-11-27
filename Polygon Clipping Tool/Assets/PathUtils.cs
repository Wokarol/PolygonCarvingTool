using System;
using System.Collections.Generic;
using UnityEngine;

internal static class PathUtils
{
    internal static List<Vector2[]> PathFromCircle(Vector2 offset, float radius, int pointsCount) {
        var paths = new List<Vector2[]>(1);
        var path = new Vector2[pointsCount];
        paths.Add(path);

        float interval = Mathf.PI * 2 / pointsCount;
        for (int i = 0; i < pointsCount; i++) {
            float angle = interval * (i + 1);
            path[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (radius * 0.5f) + offset;
        }

        return paths;
    }

    internal static List<Vector2[]> PolygonToPath(PolygonCollider2D polygonCollider) {
        var result = new List<Vector2[]>(polygonCollider.pathCount);
        for (int i = 0; i < polygonCollider.pathCount; i++) {
            Vector2[] path = polygonCollider.GetPath(i);
            result.Add(path);
        }
        return result;
    }

    internal static void PathToPolygon(PolygonCollider2D polygonCollider, List<Vector2[]> path) {
        polygonCollider.pathCount = path.Count;
        for (int i = 0; i < path.Count; i++) {
            polygonCollider.SetPath(i, path[i]);
        }
    }
}