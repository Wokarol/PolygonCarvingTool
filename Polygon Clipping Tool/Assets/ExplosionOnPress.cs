using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol
{
    public class ExplosionOnPress : MonoBehaviour
    {
        [SerializeField] new Camera camera = null;
        [Space]
        [SerializeField] float minRadius = 3f;
        [SerializeField] float maxRadius = 4f;

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                var pos = camera.ScreenToWorldPoint(Input.mousePosition);
                float radius = Random.Range(minRadius, maxRadius);
                var targets = Physics2D.OverlapCircleAll(pos, radius);
                foreach (var target in targets) {
                    var destroyable = target.GetComponent<IExplodeable>();
                    destroyable?.RegisterExplosion(pos, radius);
                }
            }
        }

        private void OnDrawGizmos() {
            if (camera) {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(camera.ScreenToWorldPoint(Input.mousePosition), minRadius);
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(camera.ScreenToWorldPoint(Input.mousePosition), maxRadius);

            }
        }
    }
}
