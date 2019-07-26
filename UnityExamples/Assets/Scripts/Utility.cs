using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class Utility
    {
        public static void lookAt2D(Transform self, Transform target, float offset)
        {
            Vector3 targetPos = target.position;
            targetPos.x = targetPos.x - self.position.x;
            targetPos.y = targetPos.y - self.position.y;
            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            self.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }
    }
}
