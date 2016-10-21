using System;
using System.Numerics;

namespace Spreads.Algorithms {

    internal interface IScalarMapping<V, V1> {
        Func<V, V1> ScalarFunc { get; }
        // TODO replace to Action<,> with provided destination segment
        Func<ArraySegment<V>, ArraySegment<V1>> ArrayFunc { get; }
        IScalarMapping<V, V2> Combine<V2>(IScalarMapping<V1, V2> next);
    }

    internal class VectorScalarMapping<V, V1> : IScalarMapping<V, V1> where V : struct where V1 : struct {
        public Func<Func<Vector<V>, Vector<V1>>, Func<ArraySegment<V>, ArraySegment<V1>>> VectorToArrayFunc { get; set; }

        public VectorScalarMapping(Func<V, V1> sf, Func<Vector<V>, Vector<V1>> vf) {
            ScalarFunc = sf;
            VectorFunc = vf;
        }

        public Func<V, V1> ScalarFunc { get; }
        public Func<Vector<V>, Vector<V1>> VectorFunc { get; }

        public Func<ArraySegment<V>, ArraySegment<V1>> ArrayFunc => VectorToArrayFunc(VectorFunc);

        public IScalarMapping<V, V2> Combine<V2>(IScalarMapping<V1, V2> next) {
            // workaround for type constraints
            dynamic cont = next;
            return DoCombine(cont);
        }

        private IScalarMapping<V, V2> DoCombine<V2>(VectorScalarMapping<V1, V2> next) where V2 : struct {
            var sf = CoreUtils.CombineMaps(ScalarFunc, next.ScalarFunc);
            var vf = CoreUtils.CombineMaps(VectorFunc, next.VectorFunc);
            //Console.WriteLine("Dispatched to SIMD");
            return new VectorScalarMapping<V, V2>(sf, vf);
        }

        private IScalarMapping<V, V2> DoCombine<V2>(IScalarMapping<V1, V2> next) {
            throw new NotImplementedException();
        }
    }
}