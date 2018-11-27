using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
using System;

public class ClipperTest : MonoBehaviour
{
    private const float CALC_SCALE = 100000;
    private const float REV_CALC_SCALE = 1 / CALC_SCALE;

    [SerializeField] PolygonCollider2D subject = new PolygonCollider2D();
    [SerializeField] PolygonCollider2D cliper = new PolygonCollider2D();

    private void Start() {
        Calculate();
    }

    void Calculate() {
        var subj = new List<List<IntPoint>>();
        for (int i = 0; i < subject.pathCount; i++) {
            var colliderPath = subject.GetPath(i);
            List<IntPoint> path = new List<IntPoint>();
            for (int j = 0; j < colliderPath.Length; j++) {
                var p = new IntPoint(colliderPath[j].x * CALC_SCALE, colliderPath[j].y * CALC_SCALE);
                path.Add(p);
            }
            subj.Add(path);
        }
        var clip = new List<List<IntPoint>>();
        for (int i = 0; i < cliper.pathCount; i++) {
            var colliderPath = cliper.GetPath(i);
            List<IntPoint> path = new List<IntPoint>(colliderPath.Length);
            for (int j = 0; j < colliderPath.Length; j++) {
                var p = new IntPoint(
                    (colliderPath[j].x + cliper.transform.position.x) * CALC_SCALE,
                    (colliderPath[j].y + cliper.transform.position.y) * CALC_SCALE);
                path.Add(p);
            }
            clip.Add(path);
        }

        var solution = new List<List<IntPoint>>();

        Clipper c = new Clipper();
        c.AddPaths(subj, PolyType.ptSubject, true);
        c.AddPaths(clip, PolyType.ptClip, true);
        c.Execute(ClipType.ctUnion, solution);


        var solutionPaths = GeneratePathsFromClipperPath(solution);
        var ob = new GameObject("Result");
        var coll = ob.AddComponent<PolygonCollider2D>();
        for (int i = 0; i < coll.pathCount; i++) {
            coll.SetPath(i, solutionPaths[i]);
        }
        ob.transform.position = subject.transform.position;

        //DrawPolygons(subj, Color.blue);
        //DrawPolygons(clip, Color.cyan);
        DrawPolygons(solution, Color.red);
    }

    private List<Vector2[]> GeneratePathsFromClipperPath(List<List<IntPoint>> cliperPaths) {
        var result = new List<Vector2[]>(cliperPaths.Count);
        foreach (var clipperPath in cliperPaths) {
            var path = new List<Vector2>(clipperPath.Count);
            for (int i = 0; i < clipperPath.Count; i++) {
                path.Add(new Vector2(clipperPath[i].X * REV_CALC_SCALE, clipperPath[i].Y * REV_CALC_SCALE));
            }
            result.Add(path.ToArray());
        }
        return result;
    }

    private void DrawPolygons(List<List<IntPoint>> solution, Color color) {
        foreach (var path in solution) {
            for (int i = 0; i < path.Count; i++) {
                Debug.DrawLine(
                    new Vector3(path[i].X / CALC_SCALE, path[i].Y / CALC_SCALE), 
                    new Vector3(path[(i + 1) % path.Count].X / CALC_SCALE, path[(i + 1) % path.Count].Y / CALC_SCALE), 
                    color, 
                    20);
            }
        }
    }
}
