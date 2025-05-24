using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PatternUtil
{ 
    public enum LaserPatternType
    {
        Single,
        Cross,
        Diagonal,
        Radial
    }
    public static Vector3[] GetLaserDirections(LaserPatternType pattern, Transform enemyTransform, Transform playerTransform = null, int radialCount = 8)
    {
        switch (pattern)
        {
            case LaserPatternType.Single:
                if (playerTransform != null)
                    return new Vector3[] { (playerTransform.position - enemyTransform.position).normalized };
                return new Vector3[] { enemyTransform.right };

            case LaserPatternType.Cross:
                return new Vector3[]
                {
                    Vector3.right, Vector3.left,
                    Vector3.up, Vector3.down
                };

            case LaserPatternType.Diagonal:
                return new Vector3[]
                {
                    (Vector3.up + Vector3.right).normalized,
                    (Vector3.up + Vector3.left).normalized,
                    (Vector3.down + Vector3.right).normalized,
                    (Vector3.down + Vector3.left).normalized
                };

            case LaserPatternType.Radial:
                List<Vector3> dirs = new List<Vector3>();
                for (int i = 0; i < radialCount; i++)
                {
                    float angle = (360f / radialCount) * i;
                    float rad = angle * Mathf.Deg2Rad;
                    dirs.Add(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)));
                }
                return dirs.ToArray();
            default:
                return new Vector3[] { Vector3.right };
        }
    }
}
