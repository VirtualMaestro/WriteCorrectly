using UnityEngine;

namespace Client.Scripts.Utils
{
    public static class IntersectionUtil
    {
        public static bool IsIntersectLineToCircle(Vector2 startLine, Vector2 endLine, Vector2 circlePosition, float radius)
        {
            var cpX = circlePosition.x;
            var cpY = circlePosition.y;
            var x1 = startLine.x - cpX;
            var y1 = startLine.y - cpY;
            var x2 = endLine.x - cpX;
            var y2 = endLine.y - cpY;

            var dx = x2 - x1;
            var dy = y2 - y1;

            var a = dx * dx + dy * dy;
            var b = 2.0 * (x1 * dx + y1 * dy);
            var c = x1 * x1 + y1 * y1 - radius * radius;

            if (-b < 0) return (c < 0);
            if (-b < (2.0 * a)) return (((4.0 * a * c) - b * b) < 0);

            return ((a + b + c) < 0);
        }
    }
}