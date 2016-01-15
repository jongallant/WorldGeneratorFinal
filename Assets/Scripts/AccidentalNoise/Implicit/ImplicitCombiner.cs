using System;
using System.Collections.Generic;
using System.Linq;

namespace AccidentalNoise
{
    public sealed class ImplicitCombiner : ImplicitModuleBase
    {
        private readonly HashSet<ImplicitModuleBase> sources = new HashSet<ImplicitModuleBase>();

        public ImplicitCombiner(CombinerType type)
        {
            this.CombinerType = type;
        }

        public CombinerType CombinerType { get; set; }

        public void AddSource(ImplicitModuleBase module)
        {
            this.sources.Add(module);
        }

        public void RemoveSource(ImplicitModuleBase module)
        {
            this.sources.Remove(module);
        }

        public void ClearSources()
        {
            this.sources.Clear();
        }

        public override Double Get(Double x, Double y)
        {
            switch (this.CombinerType)
            {
                case CombinerType.ADD:
                    return this.AddGet(x, y);
                case CombinerType.MULTIPLY:
                    return this.MultiplyGet(x, y);
                case CombinerType.MAX:
                    return this.MaxGet(x, y);
                case CombinerType.MIN:
                    return this.MinGet(x, y);
                case CombinerType.AVERAGE:
                    return this.AverageGet(x, y);
                default:
                    return 0.0;
            }
        }

        public override Double Get(Double x, Double y, Double z)
        {
            switch (this.CombinerType)
            {
                case CombinerType.ADD:
                    return this.AddGet(x, y, z);
                case CombinerType.MULTIPLY:
                    return this.MultiplyGet(x, y, z);
                case CombinerType.MAX:
                    return this.MaxGet(x, y, z);
                case CombinerType.MIN:
                    return this.MinGet(x, y, z);
                case CombinerType.AVERAGE:
                    return this.AverageGet(x, y, z);
                default:
                    return 0.0;
            }
        }

        public override Double Get(Double x, Double y, Double z, Double w)
        {
            switch (this.CombinerType)
            {
                case CombinerType.ADD:
                    return this.AddGet(x, y, z, w);
                case CombinerType.MULTIPLY:
                    return this.MultiplyGet(x, y, z, w);
                case CombinerType.MAX:
                    return this.MaxGet(x, y, z, w);
                case CombinerType.MIN:
                    return this.MinGet(x, y, z, w);
                case CombinerType.AVERAGE:
                    return this.AverageGet(x, y, z, w);
                default:
                    return 0.0;
            }
        }

        public override Double Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            switch (this.CombinerType)
            {
                case CombinerType.ADD:
                    return this.AddGet(x, y, z, w, u, v);
                case CombinerType.MULTIPLY:
                    return this.MultiplyGet(x, y, z, w, u, v);
                case CombinerType.MAX:
                    return this.MaxGet(x, y, z, w, u, v);
                case CombinerType.MIN:
                    return this.MinGet(x, y, z, w, u, v);
                case CombinerType.AVERAGE:
                    return this.AverageGet(x, y, z, w, u, v);
                default:
                    return 0.0;
            }
        }


        private Double AddGet(Double x, Double y)
        {
            return this.sources.Sum(source => source.Get(x, y));
        }

        private Double AddGet(Double x, Double y, Double z)
        {
            return this.sources.Sum(source => source.Get(x, y, z));
        }

        private Double AddGet(Double x, Double y, Double z, Double w)
        {
            return this.sources.Sum(source => source.Get(x, y, z, w));
        }

        private Double AddGet(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            return this.sources.Sum(source => source.Get(x, y, z, w, u, v));
        }


        private Double MultiplyGet(Double x, Double y)
        {
            return this.sources.Aggregate(1.00, (current, source) => current * source.Get(x, y));
        }

        private Double MultiplyGet(Double x, Double y, Double z)
        {
            return this.sources.Aggregate(1.00, (current, source) => current * source.Get(x, y, z));
        }

        private Double MultiplyGet(Double x, Double y, Double z, Double w)
        {
            return this.sources.Aggregate(1.00, (current, source) => current * source.Get(x, y,z,w));
        }

        private Double MultiplyGet(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            return this.sources.Aggregate(1.00, (current, source) => current * source.Get(x, y, z, w, u, v));
        }


        private Double MinGet(Double x, Double y)
        {
            return this.sources.Min(source => source.Get(x, y));
        }

        private Double MinGet(Double x, Double y, Double z)
        {
            return this.sources.Min(source => source.Get(x, y, z));
        }

        private Double MinGet(Double x, Double y, Double z, Double w)
        {
            return this.sources.Min(source => source.Get(x, y, z, w));
        }

        private Double MinGet(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            return this.sources.Min(source => source.Get(x, y, z, w, u, v));
        }


        private Double MaxGet(Double x, Double y)
        {
            return this.sources.Max(source => source.Get(x, y));
        }

        private Double MaxGet(Double x, Double y, Double z)
        {
            return this.sources.Max(source => source.Get(x, y, z));
        }

        private Double MaxGet(Double x, Double y, Double z, Double w)
        {
            return this.sources.Max(source => source.Get(x, y, z, w));
        }

        private Double MaxGet(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            return this.sources.Max(source => source.Get(x, y, z, w, u, v));
        }


        private Double AverageGet(Double x, Double y)
        {
            return this.sources.Average(source => source.Get(x, y));
        }

        private Double AverageGet(Double x, Double y, Double z)
        {
            return this.sources.Average(source => source.Get(x, y, z));
        }

        private Double AverageGet(Double x, Double y, Double z, Double w)
        {
            return this.sources.Average(source => source.Get(x, y, z, w));
        }

        private Double AverageGet(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            return this.sources.Average(source => source.Get(x, y, z, w, u, v));
        }
    }
}