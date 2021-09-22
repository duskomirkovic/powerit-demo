using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo.Primes
{
    class BitRange : IEnumerable<int>
    {
        private int _toValue;
        public int FromValue { get; }
        public int ToValue
        { 
            get => _toValue; 
            set => this.Enlarge(value); 
        }

        private List<UInt64> Bits { get; }
        private int BitsCount => this.ToValue - this.FromValue + 1;
        private int VectorLength => (this.BitsCount + 63) / 64;

        public BitRange(int fromValue, int toValue)
        {
            this.Bits = new List<ulong>(this.VectorLength);

            this.FromValue = fromValue;
            this.ToValue = toValue;
        }

        public bool this[int value]
        {
            get
            {
                if (value < this.FromValue || value > this.ToValue)
                    throw new IndexOutOfRangeException();
                int entry = (value - this.FromValue) / 64;
                int offset = (value - this.FromValue) % 64;
                return (this.Bits[entry] & (1ul << offset)) != 0;
            }
        }

        private void Enlarge(int toValue)
        {
            if (toValue < this.FromValue)
                throw new ArgumentException();

            this._toValue = toValue;
            while (this.Bits.Count < this.VectorLength)
                this.Bits.Add(0);
        }

        public void SetMultipliesOf(int value, int fromValue)
        {
            fromValue = Math.Max(this.FromValue, fromValue);
            int strike = (fromValue - 1) / value * value;
            while (strike <= this.ToValue - value)
            {
                strike += value;
                int entry = (strike - this.FromValue) / 64;
                int offset = (strike - this.FromValue) % 64;
                this.Bits[entry] |= 1ul << offset;
            }
        }

        public void SetMultipliesOf(IEnumerable<int> values) =>
            this.SetMultipliesOf(values, this.FromValue);

        public void SetMultipliesOf(IEnumerable<int> values, int fromValue)
        {
            foreach (int value in values)
                this.SetMultipliesOf(value, fromValue);
        }

        public BitRange InPlaceOr(BitRange other)
        {
            if (other.FromValue != this.FromValue || other.ToValue != this.ToValue)
                throw new ArgumentException();

            for (int i = 0; i < this.VectorLength; i++)
                this.Bits[i] |= other.Bits[i];

            return this;
        }

        public IEnumerable<int> GetZeros(int fromValue)
        {
            int value = Math.Max(fromValue, this.FromValue) - 1;
            int entry = (value - this.FromValue + 1) / 64;
            int offset = (value - this.FromValue + 1) % 64;
            UInt64 mask = 1ul << offset;

            while (value < this.ToValue)
            {
                value += 1;
                if ((this.Bits[entry] & mask) == 0)
                    yield return value;
                mask <<= 1;
                if (mask == 0)
                {
                    entry += 1;
                    mask = 1;
                }
            }
        }

        public IEnumerable<int> GetZeros() =>
            this.GetZeros(this.FromValue);

        public IEnumerator<int> GetEnumerator() =>
            this.GetZeros().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }
}
