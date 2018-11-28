using System.Collections.Generic;
using UnityEngine;

namespace ClipperLib.Tools
{
    public static class ClippingTool
    {
        private const float CALC_SCALE = 100000;
        private const float REV_CALC_SCALE = 1 / CALC_SCALE;




        public static void DrawPolygons(List<Vector2[]> paths, Color color, Vector3 offset, float time) {
            var clippingPaths = ConvertToCliperLibPath(paths);
            foreach (var path in clippingPaths) {
                for (int i = 0; i < path.Count; i++) {
                    Debug.DrawLine(
                        new Vector3(path[i].X * REV_CALC_SCALE, path[i].Y * REV_CALC_SCALE) + offset,
                        new Vector3(path[(i + 1) % path.Count].X * REV_CALC_SCALE, path[(i + 1) % path.Count].Y * REV_CALC_SCALE) + offset,
                        color,
                        time);
                }
            }
        }

        static List<List<IntPoint>> ConvertToCliperLibPath(List<Vector2[]> paths) {
            var result = new List<List<IntPoint>>();
            foreach (var path in paths) {
                var clippingPath = new List<IntPoint>();
                for (int i = 0; i < path.Length; i++) {
                    clippingPath.Add(new IntPoint(path[i].x * CALC_SCALE, path[i].y * CALC_SCALE));
                }
                result.Add(clippingPath);
            }
            return result;
        }

        private static List<Vector2[]> ConvertClipperPathToPath(List<List<IntPoint>> paths) {
            var result = new List<Vector2[]>();
            foreach (var clipperPath in paths) {
                var path = new Vector2[clipperPath.Count];
                for (int i = 0; i < path.Length; i++) {
                    path[i] = new Vector2(clipperPath[i].X * REV_CALC_SCALE, clipperPath[i].Y * REV_CALC_SCALE);
                }
                result.Add(path);
            }
            return result;
        }

        internal static List<Vector2[]> Subtract(List<Vector2[]> subject, List<Vector2[]> clip) {
            var solution = new List<List<IntPoint>>();

            Clipper c = new Clipper();
            c.AddPaths(ConvertToCliperLibPath(subject), PolyType.ptSubject, true);
            c.AddPaths(ConvertToCliperLibPath(clip), PolyType.ptClip, true);
            c.Execute(ClipType.ctDifference, solution);

            return ConvertClipperPathToPath(solution);
        }
    } 
}
