
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        readonly private byte[] arrayOfBytes;
        private int hashCode;

        public int Length { get { return arrayOfBytes.Length; } }

        public ReadonlyBytes(params byte[] _arrayOfBytes)
        {
            if (_arrayOfBytes == null) throw new ArgumentNullException();
            this.arrayOfBytes = _arrayOfBytes;
            hashCode = CalculateHash();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0; i < arrayOfBytes.Length; i++)
            {
                yield return arrayOfBytes[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > arrayOfBytes.Length - 1) throw new IndexOutOfRangeException();
                return arrayOfBytes[index];
            }
        }

        public override bool Equals(object obj)
        {
            var someBytesToCompare = obj as ReadonlyBytes;
            if (someBytesToCompare == null || arrayOfBytes.Length != someBytesToCompare.Length || obj.GetType().IsSubclassOf(typeof(ReadonlyBytes))) return false;
            for (int i = 0; i < arrayOfBytes.Length; i++)
                if (someBytesToCompare[i] != arrayOfBytes[i]) return false;
            return true;
        }
        public int CalculateHash()
        {
            var primeFNV = 1676;
            var hash = 13;
            unchecked
            {
                for (var i = 0; i < arrayOfBytes.Length; i++)
                {
                    hash *= primeFNV;
                    hash ^= arrayOfBytes[i].GetHashCode();
                }
            }
            return hash;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }


        public override string ToString()
        {
            var stringToWrite = new StringBuilder();
            for (var i = 0; i < arrayOfBytes.Length;i++)
            {
                if (i == arrayOfBytes.Length - 1) stringToWrite.Append(arrayOfBytes[i].ToString());
                else stringToWrite.Append(arrayOfBytes[i].ToString() + ", ");
            }
            return "[" + stringToWrite.ToString() + "]";
        }
    }
}