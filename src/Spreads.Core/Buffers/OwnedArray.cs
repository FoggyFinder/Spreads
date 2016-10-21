using System;
using System.Runtime.CompilerServices;

namespace Spreads.Buffers {
    // TODO look closer to Memory<T> and other types from CoreFxLab, maybe we do not need our ones

    public struct OwnedArray<T> : IDisposable {
        private Counter Counter { get; }
        public T[] Array { get; }

        public OwnedArray(int minLength, bool requireExactSize = true) :
            this(Impl.ArrayPool<T>.Rent(minLength, requireExactSize)) {
        }

        public OwnedArray(T[] array) {
            Array = array;
            Counter = new Counter(1);
        }

        public int RefCount => Counter.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnedArray<T> Rent() {
            //lock (Counter) {
                Counter.Increment();
                return this;
            //}
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnedArray<T> Clear() {
            //lock (Counter) {
                if (Counter.Value == 1)
                {
                    return this;
                };
                return new OwnedArray<T>(Array.Length);
            //}
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Return() {
            //lock (Counter) {
                var remaining = Counter.Decrement();
                if (remaining == 0) {
                    Impl.ArrayPool<T>.Return(Array);
                }
                return remaining;
            //}
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() {
            Return();
        }

        public static implicit operator T[] (OwnedArray<T> ownedArray) {
            return ownedArray.Array;
        }

        public static implicit operator Slices.Span<T>(OwnedArray<T> ownedArray) {
            return new Slices.Span<T>(ownedArray.Array);
        }
    }
}