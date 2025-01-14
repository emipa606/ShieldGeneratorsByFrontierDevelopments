using System;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.General
{
    public class CollisionUtility
    {
        public static class Circle
        {
            public static Vector3? LineSegment(Vector3 circleOrigin, float radius, Vector3 origin, Vector3 destination)
            {
                var d = destination - origin;
                var f = origin - circleOrigin;

                var a = Vector3.Dot(d, d);
                var b = Vector3.Dot(2 * f, d);
                var c = Vector3.Dot(f, f) - (radius * radius);

                var discriminant = (b * b) - (4 * a * c);

                if (discriminant < 0)
                {
                    return null;
                }

                // ray didn't totally miss sphere,
                // so there is a solution to
                // the equation.
                discriminant = (float) Math.Sqrt(discriminant);

                // either solution may be on or off the ray so need to test both
                // t1 is always the smaller value, because BOTH discriminant and
                // a are nonnegative.
                var t1 = (-b - discriminant) / (2 * a);
                var t2 = (-b + discriminant) / (2 * a);

                // 3x HIT cases:
                //          -o->             --|-->  |            |  --|->
                // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

                // 3x MISS cases:
                //       ->  o                     o ->              | -> |
                // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

                Vector3 Hit(float t)
                {
                    return new Vector3(origin.x + (t * d.x), origin.y + (t * d.y), origin.z + (t * d.z));
                }

                // This is if the origin is inside or past the circle
                // Possible:
                //  Hit: exit
                //  Miss: completely inside
                //  Miss: past
                if (t1 < -0.00001f)
                {
                    // Hit: Exit
                    if (t2 is <= 0 and <= 1f)
                    {
                        return Hit(t2);
                    }

                    // Miss: completely inside
                    // Miss: Past
                    return null;
                }
                // Starting outside
                // Possible:
                //  Hit: Enter
                //  Hit: Through
                //  Miss: fall short

                if (!(1f < t2))
                {
                    return Hit(t1);
                }

                if (1.00001f < t1)
                {
                    // Miss: fall short
                    return null;
                }

                // Hit: enter
                Hit(t1);

                // Hit: through
                // t2 is also a valid collision point here also
                return Hit(t1);
            }

            public static bool CellRect(Vector3 circleOrigin, float radius, CellRect rect)
            {
                if (rect.minX <= circleOrigin.x
                    && circleOrigin.x <= rect.maxX
                    && rect.minZ <= circleOrigin.z
                    && circleOrigin.z <= rect.maxZ)
                {
                    return true;
                }

                var a = new Vector3(rect.minX + 0.5f, 0, rect.minZ + 0.5f);
                var b = new Vector3(rect.minX + 0.5f, 0, rect.maxZ + 0.5f);
                var c = new Vector3(rect.maxX + 0.5f, 0, rect.maxZ + 0.5f);
                var d = new Vector3(rect.maxX + 0.5f, 0, rect.minZ + 0.5f);

                return LineSegment(circleOrigin, radius, a, b) != null
                       || LineSegment(circleOrigin, radius, b, c) != null
                       || LineSegment(circleOrigin, radius, c, d) != null
                       || LineSegment(circleOrigin, radius, d, a) != null;
            }

            public static bool Point(Vector3 circleOrigin, float radius, Vector3 point)
            {
                return Vector3.Distance(circleOrigin, point) < radius;
            }
        }

        public static class LineSegment
        {
            // Implementation from here:
            // https://jsfiddle.net/ferrybig/eokwL9mp/
            public static Vector3? Other(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
            {
                float? ComputeH(Vector3 p, Vector3 q, Vector3 r, Vector3 s)
                {
                    var e = q - p;
                    var vector3 = s - r;
                    var g = new Vector3(-e.z, 0, e.x);

                    var intersection = (vector3.x * g.x) + (vector3.z * g.z);
                    if (intersection == 0)
                    {
                        return null;
                    }

                    return (((p.x - r.x) * g.x) + ((p.z - r.z) * g.z)) / intersection;
                }

                var h1 = ComputeH(a, b, c, d);
                var h2 = ComputeH(c, d, a, b);

                // parallel
                if (h1 == null || h2 == null)
                {
                    return null;
                }

                // intersection
                if (!(h1 >= 0) || !(h1 <= 1) || !(h2 >= 0) || !(h2 <= 1))
                {
                    return null;
                }

                var f = new Vector3(d.x - c.x, 0, d.z - c.z);
                return new Vector3(c.x + (f.x * h1.Value), 0, c.z + (f.z * h1.Value));
            }
        }
    }
}