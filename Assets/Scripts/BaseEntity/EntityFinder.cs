using System;
using UnityEngine;

namespace Finders
{
    public class EntityFinder
    {
        private readonly LayerMask _targetMask;
        private readonly LayerMask _ignoreMask;

        public EntityFinder(float lookRadius, LayerMask targetMask, LayerMask ignoreMask)
        {
            _targetMask = targetMask;
            _ignoreMask = ignoreMask;
        }
        
        public Transform FindObjectInLookRadius(Vector3 startPosition, float lookRadius)
        {
            Vector3 entityPos = startPosition;
            var targets = Physics2D.OverlapCircleAll(
                startPosition, lookRadius, _targetMask);

            Array.Sort(targets, (collider2D1, collider2D2) =>
            {
                float dist1 = (collider2D1.transform.position - entityPos).sqrMagnitude;
                float dist2 = (collider2D2.transform.position - entityPos).sqrMagnitude;
                if (dist1 < dist2)
                    return -1;
                if (dist1 > dist2)
                    return 1;

                return 0;
            });

            foreach (var target in targets)
            {
                RaycastHit2D hit = Physics2D.Raycast(entityPos,
                    target.transform.position - entityPos,
                    lookRadius,  ~_ignoreMask);

                if (hit.collider != null && _targetMask == (_targetMask | (1 << hit.collider.gameObject.layer)))
                    return hit.collider.transform;
            }

            return null;    
        }
    }
}