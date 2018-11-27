using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnPress : MonoBehaviour
{
    [SerializeField] new Camera camera = null;
    [SerializeField] DestructibleGround ground = null;
    [Space]
    [SerializeField] float minRadius = 3f;
    [SerializeField] float maxRadius = 4f;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var pos = camera.ScreenToWorldPoint(Input.mousePosition);
            ground.RegisterExplosion(pos, Random.Range(minRadius, maxRadius));
        }
    }
}
