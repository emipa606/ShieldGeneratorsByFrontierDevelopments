using System.Collections.Generic;
using System.Linq;
using FrontierDevelopments.Shields.Linear;
using HarmonyLib;
using Verse;

namespace FrontierDevelopments.Shields
{
    public class ShieldMapManager : IExposable, IShieldManager
    {
        private readonly HashSet<IShieldField> _fields = new HashSet<IShieldField>();
        private readonly HashSet<IShield> _shields = new HashSet<IShield>();
        private HashSet<Map> _associated = new HashSet<Map>();
        private HashSet<LinearShieldLink> _linearLinks = new HashSet<LinearShieldLink>();

        public ShieldMapManager()
        {
        }

        public ShieldMapManager(Map map)
        {
            AssociateWithMap(map);
        }

        public IEnumerable<IShield> Shields => _shields;

        public void ExposeData()
        {
            Scribe_Collections.Look(ref _associated, "associatedMaps", LookMode.Reference);
            Scribe_Collections.Look(ref _linearLinks, "linearLinks", LookMode.Deep);

            if (Scribe.mode != LoadSaveMode.PostLoadInit)
            {
                return;
            }

            if (_linearLinks == null)
            {
                _linearLinks = new HashSet<LinearShieldLink>();
            }

            if (_associated == null)
            {
                _associated = new HashSet<Map>();
            }

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                AssociatedMaps.Do(map => ShieldManager.Register(map, this));
            }
        }

        public IEnumerable<Map> AssociatedMaps => _associated;

        public IEnumerable<IShieldField> Fields
        {
            get
            {
                foreach (var field in _fields)
                {
                    yield return field;
                }

                foreach (var link in _linearLinks)
                {
                    yield return link;
                }
            }
        }

        public void AssociateWithMap(Map map)
        {
            _associated.Add(map);
        }

        public HashSet<IShield> AllEmitters
        {
            get
            {
                IEnumerable<IShield> Union()
                {
                    foreach (var shield in _shields)
                    {
                        yield return shield;
                    }

                    foreach (var shield in _fields.SelectMany(field => field.Emitters))
                    {
                        yield return shield;
                    }
                }

                return Union().ToHashSet();
            }
        }

        public void Add(IShieldField field)
        {
            switch (field)
            {
                case LinearShieldLink link:
                    _linearLinks.Add(link);
                    break;
                default:
                    _fields.Add(field);
                    break;
            }
        }

        public void Del(IShieldField field)
        {
            switch (field)
            {
                case LinearShieldLink link:
                    _linearLinks.Remove(link);
                    break;
                default:
                    _fields.Remove(field);
                    break;
            }
        }

        public void Add(IEnumerable<IShieldField> fields)
        {
            fields.Do(Add);
        }

        public void Del(IEnumerable<IShieldField> fields)
        {
            fields.Do(Del);
        }

        public void Add(IShield shield)
        {
            _shields.Add(shield);
        }

        public void Del(IShield shield)
        {
            _shields.Remove(shield);
        }

        public IEnumerable<Map> PresentOnMaps(IShield shield)
        {
            return _associated.Where(shield.PresentOnMap);
        }

        public IEnumerable<Map> PresentOnMaps(IShieldField field)
        {
            return _associated.Where(field.PresentOnMap);
        }

        public void DrawShields(CellRect cameraRect, Map map)
        {
            var fields = Fields.Where(field => field.PresentOnMap(map)).ToList();

            foreach (var field in fields)
            {
                field.FieldPreDraw();
            }

            foreach (var field in fields)
            {
                field.FieldDraw(cameraRect);
            }

            foreach (var field in fields)
            {
                field.FieldPostDraw();
            }
        }

        public FieldQuery Query(Map map)
        {
            return new FieldQuery(Fields, map);
        }

        public void Tick()
        {
            _linearLinks.Do(link => link.Tick());
        }
    }
}