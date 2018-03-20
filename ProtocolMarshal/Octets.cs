using System;
using System.Text;

namespace ProtocolMarshal
{
    public class Octets : IComparable<object>, ICloneable
    {
        #region private members

        private static readonly int DefaultSize = 128;
        public static Encoding DefaultEncoding = Encoding.UTF8; //Encoding.Unicode;

        private byte[] _buffer = null;

        #endregion

        #region constructors

        public Octets()
        {
            Reserve(DefaultSize);
        }

        public Octets(int size)
        {
            Reserve(size);
        }

        public Octets(Octets rhs)
        {
            Replace(rhs);
        }

        public Octets(byte[] rhs)
        {
            Replace(rhs);
        }

        private Octets(byte[] bytes, int length)
        {
            _buffer = bytes;
            Size = length;
        }

        public Octets(byte[] rhs, int pos, int size)
        {
            Replace(rhs, pos, size);
        }

        public Octets(Octets rhs, int pos, int size)
        {
            Replace(rhs, pos, size);
        }

        #endregion

        #region public properties

        public int Size { get; set; }

        public byte[] Buffer()
        {
            return _buffer;
        }

        #endregion

        #region public methods

        public byte[] GetBytes()
        {
            byte[] tmp = new byte[Size];
            Array.Copy(_buffer, 0, tmp, 0, Size);
            return tmp;
        }

        #endregion

        #region private methods

        private byte[] Roundup(int size)
        {
            int capacity = 16;
            while (size > capacity) capacity <<= 1;
            return new byte[capacity];
        }

        public void Reserve(int size)
        {
            if (_buffer == null)
            {
                _buffer = Roundup(size);
            }
            else if (size > _buffer.Length)
            {
                byte[] tmp = Roundup(size);
                Array.Copy(_buffer, 0, tmp, 0, Size);
                _buffer = tmp;
            }
        }

        public Octets Replace(byte[] data, int pos, int size)
        {
            Reserve(size);
            Array.Copy(data, pos, _buffer, 0, size);
            Size = size;
            return this;
        }

        public Octets Replace(Octets data, int pos, int size)
        {
            return Replace(data._buffer, pos, size);
        }

        public Octets Replace(byte[] data)
        {
            return Replace(data, 0, data.Length);
        }

        public Octets Replace(Octets data)
        {
            return Replace(data._buffer, 0, data.Size);
        }

        #endregion

        #region protected methods

        protected Octets Swap(Octets rhs)
        {
            int size = Size;
            Size = rhs.Size;
            rhs.Size = size;
            byte[] tmp = rhs._buffer;
            rhs._buffer = _buffer;
            _buffer = tmp;
            return this;
        }

        protected Octets Push_back(byte data)
        {
            _buffer[Size++] = data;
            return this;
        }

        protected Octets Erase(int from, int to)
        {
            Array.Copy(_buffer, to, _buffer, from, Size - to);
            Size -= to - from;
            return this;
        }

        protected Octets Insert(int from, byte[] data, int pos, int size)
        {
            Reserve(Size + size);
            Array.Copy(_buffer, from, _buffer, from + size, Size - from);
            Array.Copy(data, pos, _buffer, from, size);
            Size += size;
            return this;
        }

        protected Octets Insert(int from, byte[] data)
        {
            return Insert(from, data, 0, data.Length);
        }

        protected Octets Insert(int from, Octets data)
        {
            return Insert(from, data._buffer, 0, data.Size);
        }

        public byte GetByte(int pos)
        {
            return _buffer[pos];
        }

        #endregion

        #region public static methods

        public static Octets Wrap(byte[] bytes, int length)
        {
            return new Octets(bytes, length);
        }

        public static Octets Wrap(byte[] bytes)
        {
            return Wrap(bytes, bytes.Length);
        }

        public static Octets Wrap(string str, string encoding)
        {
            return Wrap(Encoding.GetEncoding(encoding).GetBytes(str));
        }

        #endregion

        #region interface IComparable, ICloneable

        public object Clone()
        {
            return new Octets(this);
        }

        public int CompareTo(object other)
        {
            return CompareTo((Octets) other);
        }

        public int CompareTo(Octets rhs)
        {
            int c = Size - rhs.Size;
            if (c != 0) return c;

            byte[] v1 = _buffer;
            byte[] v2 = rhs._buffer;
            for (int i = 0; i < Size; i++)
            {
                int v = v1[i] - v2[i];
                if (v != 0)
                    return v;
            }
            return 0;
        }

        #endregion

        #region override object methods

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            return CompareTo(o) == 0;
        }

        public override int GetHashCode()
        {
            if (_buffer == null)
                return 0;

            int result = 1;
            for (int i = 0; i < Size; i++)
                result = 31*result + _buffer[i];

            return result;
        }

        public override string ToString()
        {
            return "octets.size=" + Size;
        }

        #endregion
    }
}