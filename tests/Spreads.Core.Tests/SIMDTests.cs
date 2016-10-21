using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Spreads.Algorithms;
using System.Numerics;

namespace Spreads.Core.Tests {
    [TestFixture]
    public class SIMDTests {
        [Test]
        public void MappingTests()
        {
            var m1 = new VectorScalarMapping<int, int>((x) => x + x, (v) => Vector.Add(v, v));
            var m2 = new VectorScalarMapping<int, int>((x) => x * 10 , null);
            var m3 = m1.Combine(m2 as IScalarMapping<int,int>);
        }
    }
}
