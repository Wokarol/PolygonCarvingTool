//#define LOGS

using ClipperLib.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Wokarol.Utils;


namespace Wokarol.Ground
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class DestructibleGround : MonoBehaviour, IExplodeable
    {
        [SerializeField] GameObject maskPrefab = null;
        [SerializeField] PolygonCollider2D polygonCollider = null;
        private void Awake() {
            polygonCollider = GetComponent<PolygonCollider2D>();
        }

        public void RegisterExplosion(Vector2 pos, float radius) {
            CalculateExplosion(pos, radius);
        }

        private async void CalculateExplosion(Vector2 pos, float radius) {
#if LOGS
            int startFrame = Time.frameCount;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            List<Vector2[]> oldPath = PathUtils.PolygonToPath(polygonCollider);
            List<Vector2[]> carvePath = new List<Vector2[]>() {
                PathUtils.PathFromCircle(pos, radius, 30)};
            List<Vector2[]> newPath = null;
            await Task.Run(() => {
                newPath = ClippingTool.Subtract(oldPath, carvePath);
            });
            PathUtils.PathToPolygon(polygonCollider, newPath);

            // Creating mask
            Instantiate(maskPrefab, pos, Quaternion.identity).transform.localScale = Vector3.one * (radius * 2f);

            ClippingTool.DrawPolygons(newPath, Color.red, Vector3.zero, 0.5f);
            ClippingTool.DrawPolygons(carvePath, Color.green, Vector3.zero, 0.25f);
#if LOGS
            Debug.Log($"Time taken = {Time.frameCount - startFrame} frames and {stopwatch.ElapsedMilliseconds} ms ");
            stopwatch.Stop();
#endif
        }
    }
}
