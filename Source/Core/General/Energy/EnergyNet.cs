using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace FrontierDevelopments.General.Energy
{
    public class EnergyNet : IEnergyNet, IExposable
    {
        private readonly HashSet<IEnergyNode> nodes = new HashSet<IEnergyNode>();
        private float _draw;

        //
        //
        //

        private int _id;
        private bool _netPowered;
        private IEnergyNet _parent;

        public EnergyNet()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                _id = NextId;
            }
        }

        private static int NextId => Verse.Find.UniqueIDsManager.GetNextThingID();

        public IEnergyNet Parent => _parent;

        public IEnumerable<IEnergyNode> Nodes => nodes;

        public void Connect(IEnergyNode node)
        {
            if (node == this)
            {
                return;
            }

            nodes.Add(node);
            Changed();
        }

        public void Disconnect(IEnergyNode node)
        {
            if (node == this)
            {
                return;
            }

            nodes.Remove(node);
            Changed();
        }

        public void ConnectTo(IEnergyNet net)
        {
            _parent?.Disconnect(this);
            _parent = net;
            _parent?.Connect(this);
        }

        public void Disconnect()
        {
            _parent?.Disconnect();
            _parent = null;
        }

        public float AmountAvailable => CanProvide(provider => provider.AmountAvailable);

        public float RateAvailable => CanProvide(provider => provider.RateAvailable);

        public float TotalAvailable => CanProvide(provider => provider.TotalAvailable);

        public float MaxRate => CanProvide(provider => provider.MaxRate);

        public float Provide(float amount)
        {
            return HandleEnergy(
                amount,
                nodes
                    .OfType<IEnergyProvider>()
                    .OrderBy(provider => provider.AmountAvailable),
                (node, remaining) => node.Provide(remaining));
        }

        public float Consume(float amount)
        {
            var result = HandleEnergy(
                amount,
                nodes
                    .OfType<IEnergyProvider>()
                    .OrderByDescending(provider => provider.AmountAvailable),
                (node, remaining) => node.Consume(remaining));
            if (result < amount)
            {
                _draw += amount - result;
            }

            return result;
        }

        public float Request(float amount)
        {
            var result = HandleEnergy(
                amount,
                nodes
                    .OfType<IEnergyProvider>()
                    .OrderByDescending(provider => provider.AmountAvailable),
                (node, remaining) => node.Request(remaining));
            if (result < amount)
            {
                _draw += amount - result;
            }

            return result;
        }

        public void Update()
        {
            CalculateConsumption(consumer => Consume(consumer.Rate));
        }

        public void Changed()
        {
            if (_parent != null)
            {
                _parent.Changed();
            }
            else
            {
                CalculateConsumption(consumer => Request(consumer.Rate));
            }
        }

        public float Rate => _draw;

        public void HasPower(bool isPowered)
        {
            var last = _netPowered;
            _netPowered = isPowered;
            if (_netPowered != last)
            {
                Update();
            }
        }

        public string GetUniqueLoadID()
        {
            return "EnergyNet" + _id;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref _id, "id");
            Scribe_References.Look(ref _parent, "parent");
            Scribe_Values.Look(ref _netPowered, "netPowered");
            Scribe_Values.Look(ref _draw, "draw");

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                ConnectTo(_parent);
            }
        }

        public static IEnergyNet Find(Thing parent)
        {
            switch (parent)
            {
                case IEnergyNet parentSource:
                    return parentSource;
                case ThingWithComps thingWithComps:
                    return FindComp(thingWithComps.AllComps);
                default:
                    return null;
            }
        }

        public static IEnergyNet FindComp(IEnumerable<ThingComp> comps)
        {
            try
            {
                return comps.OfType<IEnergyNet>().First();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private float CanProvide(Func<IEnergyProvider, float> callback)
        {
            return nodes.OfType<IEnergyProvider>().Aggregate(0f, (sum, node) => sum + callback(node))
                   + (_netPowered && _parent != null ? callback(_parent) : 0f);
        }

        private float HandleEnergy(
            float amount,
            IEnumerable<IEnergyProvider> providers,
            Func<IEnergyProvider, float, float> callback)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException("Can't provide a negative amount");
            }

            var remaining = amount;

            foreach (var node in providers)
            {
                if (remaining <= 0)
                {
                    break;
                }

                var handled = callback(node, remaining);

                remaining -= handled;
            }

            return amount - remaining;
        }

        private void CalculateConsumption(Action<IEnergyConsumer> consumerAction)
        {
            _draw = 0;
            nodes.OfType<IEnergyProvider>().Do(provider => provider.Update());
            nodes.OfType<IEnergyConsumer>().Do(consumer =>
            {
                if (RateAvailable - consumer.Rate < 0)
                {
                    consumer.HasPower(false);
                }
                else
                {
                    consumer.HasPower(true);
                    consumerAction.Invoke(consumer);
                }
            });
        }
    }
}