﻿


// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;

namespace Spreads
{
    //// TODO review & test this trick to implicitly cast non-matching cursor types to Cursor<TKey, TValue>
    //// see CouldCalculateComplexGraph test
    //// See ECMA 334 14.2.4 & 14.4.2 http://www.ecma-international.org/publications/files/ECMA-ST/Ecma-334.pdf

    public readonly partial struct Series<TKey, TValue, TCursor>
    where TCursor : ISpecializedCursor<TKey, TValue, TCursor>
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, AddOp<TValue>, TCursor, TCursor>> operator
            +(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(AddOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, AddOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            +(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, Cursor<TKey, TValue>> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(AddOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, AddOp<TValue>, Cursor<TKey, TValue>, TCursor>> operator
            +(Series<TKey, TValue> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetWrapper();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, Cursor<TKey, TValue>, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(AddOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, AddOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            +(Series<TKey, TValue, TCursor> series, Series<TKey, TValue> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetWrapper();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(AddOp<TValue>)).Source;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, SubtractOp<TValue>, TCursor, TCursor>> operator
            -(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(SubtractOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, SubtractOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            -(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, Cursor<TKey, TValue>> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(SubtractOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, SubtractOp<TValue>, Cursor<TKey, TValue>, TCursor>> operator
            -(Series<TKey, TValue> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetWrapper();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, Cursor<TKey, TValue>, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(SubtractOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, SubtractOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            -(Series<TKey, TValue, TCursor> series, Series<TKey, TValue> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetWrapper();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(SubtractOp<TValue>)).Source;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, MultiplyOp<TValue>, TCursor, TCursor>> operator
            *(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(MultiplyOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, MultiplyOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            *(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, Cursor<TKey, TValue>> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(MultiplyOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, MultiplyOp<TValue>, Cursor<TKey, TValue>, TCursor>> operator
            *(Series<TKey, TValue> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetWrapper();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, Cursor<TKey, TValue>, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(MultiplyOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, MultiplyOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            *(Series<TKey, TValue, TCursor> series, Series<TKey, TValue> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetWrapper();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(MultiplyOp<TValue>)).Source;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, DivideOp<TValue>, TCursor, TCursor>> operator
            /(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(DivideOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, DivideOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            /(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, Cursor<TKey, TValue>> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(DivideOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, DivideOp<TValue>, Cursor<TKey, TValue>, TCursor>> operator
            /(Series<TKey, TValue> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetWrapper();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, Cursor<TKey, TValue>, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(DivideOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, DivideOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            /(Series<TKey, TValue, TCursor> series, Series<TKey, TValue> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetWrapper();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(DivideOp<TValue>)).Source;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, ModuloOp<TValue>, TCursor, TCursor>> operator
            %(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(ModuloOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, ModuloOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            %(Series<TKey, TValue, TCursor> series, Series<TKey, TValue, Cursor<TKey, TValue>> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(ModuloOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, ModuloOp<TValue>, Cursor<TKey, TValue>, TCursor>> operator
            %(Series<TKey, TValue> series, Series<TKey, TValue, TCursor> other)
        {
            var c1 = series.GetWrapper();
            var c2 = other.GetEnumerator();

            var zipCursor = new Zip<TKey, TValue, TValue, Cursor<TKey, TValue>, TCursor>(c1, c2);
            return ZipOp(zipCursor, default(ModuloOp<TValue>)).Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Series<TKey, TValue, ZipOp<TKey, TValue, ModuloOp<TValue>, TCursor, Cursor<TKey, TValue>>> operator
            %(Series<TKey, TValue, TCursor> series, Series<TKey, TValue> other)
        {
            var c1 = series.GetEnumerator();
            var c2 = other.GetWrapper();

            var zipCursor = new Zip<TKey, TValue, TValue, TCursor, Cursor<TKey, TValue>>(c1, c2);
            return ZipOp(zipCursor, default(ModuloOp<TValue>)).Source;
        }


    }
}
