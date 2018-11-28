using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol
{
    public interface IExplodeable
    {
        void RegisterExplosion(Vector2 pos, float radius);
    } 
}
