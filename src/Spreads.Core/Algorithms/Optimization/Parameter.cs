﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Spreads.Algorithms.Optimization {

    [StructLayout(LayoutKind.Sequential)]
    public struct Parameter : IEnumerable<double>, IEnumerator<double> {
        private readonly string _code;
        private readonly double _startValue;
        private readonly double _endValue;
        private readonly double _stepSize;
        private readonly int _steps;
        private readonly int _bigStepMultiple;
        private int _currentPosition;
        private int _offset;

        public Parameter(string code, double startValue, double endValue, double stepSize = 0, int bigStepMultiple = 1) {
            //
            if (endValue < startValue && stepSize > 0) { throw new ArgumentException("endValue <= startValue while step > 0"); }
            if (endValue > startValue && stepSize < 0) { throw new ArgumentException("endValue >= startValue while step < 0"); }
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (stepSize == 0) {
                Trace.TraceWarning("Step size is zero, assuming differefe between start and end");
                stepSize = endValue - startValue;
            }
            _code = code.Trim();
            _startValue = startValue;
            _endValue = endValue;
            _stepSize = stepSize;
            //Debug.Assert((_endValue - _startValue) / _stepSize > 0);
            _steps = 1 + (int)Math.Ceiling((_endValue - _startValue) / _stepSize);
            if (bigStepMultiple < 1) { throw new ArgumentOutOfRangeException(nameof(bigStepMultiple)); }
            _bigStepMultiple = bigStepMultiple;
            _currentPosition = -1;
            _offset = 0;
        }

        public string Code => _code;
        public double StartValue => _startValue;
        public double EndValue => _endValue;
        public double StepSize => _stepSize;

        public int Steps {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _steps; }
        }

        public int BigStepMultiple => _bigStepMultiple;

        public int CurrentPosition {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _currentPosition; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                if (value < 0 || value >= _steps) throw new ArgumentOutOfRangeException(nameof(value));
                _currentPosition = value;
            }
        }

        public int GridPosition {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _offset + (_currentPosition == -1 ? 0 : _currentPosition); }
        }

        public double this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (index < 0) throw new ArgumentException("Parameter index is negative");
                if (index >= _steps) throw new ArgumentException("Parameter index is greater than number of steps");
                return index == (_steps - 1) ? _endValue : _startValue + index * _stepSize;
            }
        }

        /// <summary>
        /// Epsilon number of step around the position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public Parameter GetRegion(int position, int epsilon) {
            var offset = Math.Max(position - epsilon, 0);
            var start = this[offset];
            var end = this[Math.Min(position + epsilon, _steps - 1)];
            var newParameter = new Parameter(_code, start, end, _stepSize, _bigStepMultiple) {
                _offset = this._offset + offset
            };
            return newParameter;
        }

        public Parameter WithBigStep() {
            var newParameter = new Parameter(_code, _startValue, _endValue, _stepSize * _bigStepMultiple, 1) {
                _offset = this._offset
            };
            return newParameter;
        }

        public void Dispose() {
        }

        public double Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Debug.Assert(_currentPosition >= 0, "Wrong access to Current of not started Parameter");
                Debug.Assert(_currentPosition < _steps, "Wrong _current position");
                return _currentPosition == (_steps - 1) ? _endValue : _startValue + _currentPosition * _stepSize;
            }
        }

        object IEnumerator.Current => this.Current;

        public Parameter GetEnumerator() {
            var copy = this;
            copy._currentPosition = -1;
            return copy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() {
            var nextPosition = _currentPosition + 1;
            Debug.Assert(nextPosition <= _steps);
            if (nextPosition == _steps) { return false; }
            _currentPosition = nextPosition;
            return true;
        }

        public bool BigMoveNext() {
            Debug.Assert(_currentPosition + 1 <= _steps);
            if (_currentPosition + 1 == _steps) { return false; }
            if (_currentPosition == -1) {
                _currentPosition++;
                return true;
            }
            var nextPosition = _currentPosition + _bigStepMultiple;
            if (nextPosition >= _steps) {
                _currentPosition = _steps - 1;
                return true;
            }
            _currentPosition = nextPosition;
            return true;
        }

        public void Reset() {
            _currentPosition = -1;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator<double> IEnumerable<double>.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class Parameters : DynamicObject {
        private readonly Parameter[] _parameters;

        public Parameters(Parameter[] parameters) {
            if (parameters.Select(x => x.Code).Distinct(StringComparer.OrdinalIgnoreCase).Count() != parameters.Length) {
                throw new ArgumentException("Parameter codes are not unique");
            }
            _parameters = parameters;
        }

        public Parameter[] Array => _parameters;

        //public Parameter[] Array => _parameters;
        public int Count => _parameters.Length;

        public double this[string code] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                foreach (var par in _parameters) {
                    if (par.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) {
                        return par.Current;
                    }
                }
                var trimmed = code.Trim();
                if (trimmed != code) {
                    var result = this[trimmed];
                    Trace.TraceWarning($"Parameter {code} has leading or trailing spaces, check for typos in accessor");
                    return result;
                } else {
                    throw new KeyNotFoundException($"Unknown parameter: {code}");
                }
            }
        }

        public Parameter this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (index < 0 || index >= _parameters.Length) {
                    throw new IndexOutOfRangeException();
                }
                return _parameters[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                if (index < 0 || index >= _parameters.Length) {
                    throw new IndexOutOfRangeException();
                }
                _parameters[index] = value;
            }
        }

        public int TotalInterations => _parameters.Select(x => x.Steps).Aggregate(1, (i, st) => checked(i * st));

        public Parameters GetRegion(int epsilon) {
            var clone = this.Clone();
            for (int i = 0; i < _parameters.Length; i++) {
                clone._parameters[i] = _parameters[i].GetRegion(_parameters[i].CurrentPosition, epsilon);
            }
            return clone;
        }

        public Parameters Clone() {
            return new Parameters(_parameters.ToArray());
        }

        public Parameters Reset() {
            foreach (var p in _parameters) {
                p.Reset();
            }
            return this;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            var name = binder.Name;
            foreach (var par in _parameters) {
                if (par.Code.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    result = par.Current;
                    return true;
                }
            }
            result = null;
            return false;
        }
    }

    public static class ParameterExtensions {

        // useful to as a key to memoize target function result at a point, instead of int[]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LinearAddress(this Parameters parameters) {
            return parameters.Array.LinearAddress();
        }

        // useful to as a key to memoize target function result at a point, instead of int[]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LinearAddress(this Parameter[] parameters) {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var address = -1L;
            if (parameters.Length == 0) return address;
            address = parameters[0].CurrentPosition;
            // previous * current dim + current addr
            // TODO test + review
            for (int i = 1; i < parameters.Length; i++) {
                address = address * parameters[i].Steps + parameters[i].CurrentPosition;
            }
            return address;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parameters SetPositionsFromLinearAddress(this Parameters parameters, long linearAddress) {
            var newParameters = SetPositionsFromLinearAddress(parameters.Array, linearAddress);
            return new Parameters(newParameters);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parameter[] SetPositionsFromLinearAddress(this Parameter[] parameters, long linearAddress) {
            var newParameters = parameters.ToArray();
            for (int i = parameters.Length - 1; i >= 1; i--) {
                var steps = parameters[i].Steps;
                var tmp = linearAddress / steps;
                var iPos = checked((int)(linearAddress - tmp * steps));
                newParameters[i].CurrentPosition = iPos;
                linearAddress = tmp;
            }
            newParameters[0].CurrentPosition = checked((int)linearAddress);
            return newParameters;
        }
    }
}