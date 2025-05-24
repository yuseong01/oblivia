using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsUtil
{
    public static Vector2 ClampToBounds(Vector2 position, Vector2 min, Vector2 max, float margin = 0.3f)
    {
        float x = Mathf.Clamp(position.x, min.x + margin, max.x - margin);
        float y = Mathf.Clamp(position.y, min.y + margin, max.y - margin);
        return new Vector2(x, y);
    }

    public static Vector3 ClampToBounds(Vector3 position, Vector2 min, Vector2 max, float margin = 0.3f)
    {
        float x = Mathf.Clamp(position.x, min.x + margin, max.x - margin);
        float y = Mathf.Clamp(position.y, min.y + margin, max.y - margin);
        return new Vector3(x, y, position.z);
    }

    public static void UpdateBoundsFromRoom(IEnemy enemy, ref Vector2 minBounds, ref Vector2 maxBounds)
    {
        var room = enemy.GetCurrentRoom();
        if (room != null)
        {
            minBounds = room.GetMinBounds();
            maxBounds = room.GetMaxBounds();
        }
    }

    public static bool IsInsideBounds(Vector2 pos, Vector2 minBounds, Vector2 maxBounds)
    {
        return pos.x >= minBounds.x && pos.x <= maxBounds.x &&
               pos.y >= minBounds.y && pos.y <= maxBounds.y;
    }

    public static Vector3 GetRayToRoomEdge(Vector3 origin, Vector3 direction, Vector2 min, Vector2 max, float extend = 0.5f)
    {
        // 끝점 계산을 위한 큰 값
        float distance = 100f;

        Vector3 testEnd = origin + direction.normalized * distance;

        // Clamp to Room Bounds
        Vector3 clamped = ClampToBounds(testEnd, min, max, extend);

        return clamped;
    }

}
