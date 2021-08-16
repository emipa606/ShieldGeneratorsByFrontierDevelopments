using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.Shields
{
    public interface IShieldField
    {
        Map Map { get; }
        int ProtectedCellCount { get; }
        float CellProtectionFactor { get; }
        Faction Faction { get; }
        IEnumerable<IShield> Emitters { get; }
        bool PresentOnMap(Map map);
        bool IsActive();
        bool Collision(Vector3 point);
        Vector3? Collision(Ray ray, float limit);
        Vector3? Collision(Vector3 start, Vector3 end);
        float Block(float damage, Vector3 position);
        float Block(ShieldDamages damages, Vector3 position);
        void FieldPreDraw();
        void FieldDraw(CellRect cameraRect);
        void FieldPostDraw();
    }
}