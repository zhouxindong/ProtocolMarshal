using System;
using System.Text;

namespace ProtocolMarshal
{
    public class OctetsStream : Octets
    {
        #region private members

        private int _pos;

        #endregion

        #region constructors

        public OctetsStream()
        {
        }

        public OctetsStream(int size)
            : base(size)
        {
        }

        public OctetsStream(Octets o)
            : base(o)
        {
        }

        #endregion

        #region Marshal

        public OctetsStream Marshal(byte x)
        {
            Reserve(Size + 1);
            Push_back(x);
            return this;
        }

        public OctetsStream Marshal(bool b)
        {
            Reserve(Size + 1);
            Push_back(unchecked((byte) (b ? 1 : 0)));
            return this;
        }

        public OctetsStream Marshal(short x)
        {
            Reserve(Size + 2);
            Push_back(unchecked((byte) (x >> 8)));
            Push_back(unchecked((byte) (x)));
            return this;
        }

        public OctetsStream Marshal(char x)
        {
            Reserve(Size + 2);
            Push_back(unchecked((byte) (x >> 8)));
            Push_back(unchecked((byte) (x)));
            return this;
        }

        public OctetsStream Marshal(int x)
        {
            Reserve(Size + 4);
            Push_back(unchecked((byte) (x >> 24)));
            Push_back(unchecked((byte) (x >> 16)));
            Push_back(unchecked((byte) (x >> 8)));
            Push_back(unchecked((byte) (x)));
            return this;
        }

        public OctetsStream Marshal(long x)
        {
            Reserve(Size + 8);
            Push_back(unchecked((byte) (x >> 56)));
            Push_back(unchecked((byte) (x >> 48)));
            Push_back(unchecked((byte) (x >> 40)));
            Push_back(unchecked((byte) (x >> 32)));
            Push_back(unchecked((byte) (x >> 24)));
            Push_back(unchecked((byte) (x >> 16)));
            Push_back(unchecked((byte) (x >> 8)));
            Push_back(unchecked((byte) (x)));
            return this;
        }

        public OctetsStream Marshal(float x)
        {
            byte[] b = BitConverter.GetBytes(x);
            return Marshal(BitConverter.ToInt32(b, 0));
        }

        public OctetsStream Marshal(double x)
        {
            return Marshal(BitConverter.DoubleToInt64Bits(x));
        }

        public OctetsStream Compact_uint32(int x)
        {
            if (x < 0x40) return Marshal((byte) x);
            if (x < 0x4000) return Marshal((short) (x | 0x8000));
            if (x < 0x20000000) return Marshal((int) (x | 0xc0000000));
            Marshal((byte) 0xe0);
            return Marshal(x);
        }

        public OctetsStream Compact_sint32(int x)
        {
            if (x >= 0)
            {
                if (x < 0x40) return Marshal((byte) x);
                if (x < 0x2000) return Marshal((short) (x | 0x8000));
                if (x < 0x10000000) return Marshal((int) (x | 0xc0000000));
                Marshal((byte) 0xe0);
                return Marshal(x);
            }
            if (-x > 0)
            {
                x = -x;
                if (x < 0x40) return Marshal((byte) (x | 0x40));
                if (x < 0x2000) return Marshal((short) (x | 0xa000));
                if (x < 0x10000000) return Marshal((uint) x | 0xd0000000);
                Marshal((byte) 0xf0);
                return Marshal(x);
            }
            Marshal((byte) 0xf0);
            return Marshal(x);
        }

        public OctetsStream Marshal(byte[] bytes)
        {
            Compact_uint32(bytes.Length);
            Insert(Size, bytes);
            return this;
        }

        public OctetsStream Marshal(IMarshal m)
        {
            return m.Marshal(this);
        }

        public OctetsStream Marshal(Octets o)
        {
            Compact_uint32(o.Size);
            Insert(Size, o);
            return this;
        }

        public OctetsStream Marshal(string str)
        {
            return Marshal(str, DefaultEncoding);
        }

        public OctetsStream Marshal(string str, Encoding encoding)
        {
            try
            {
                Marshal(encoding == null
                    ? DefaultEncoding.GetBytes(str)
                    : encoding.GetBytes(str));
            }
            catch
            {
                throw new MarshalException();
            }
            return this;
        }

        #endregion

        #region Unmarshal

        public byte Unmarshal_byte()
        {
            if (_pos + 1 > Size) throw new MarshalException();
            return GetByte(_pos++);
        }

        public bool Unmarshal_bool()
        {
            return Unmarshal_byte() == 1;
        }

        public short Unmarshal_short()
        {
            if (_pos + 2 > Size) throw new MarshalException();
            byte b0 = GetByte(_pos++);
            byte b1 = GetByte(_pos++);
            return (short)((b0 << 8) | (b1 & 0xff));
        }

        public char Unmarshal_char()
        {
            if (_pos + 2 > Size) throw new MarshalException();
            byte b0 = GetByte(_pos++);
            byte b1 = GetByte(_pos++);
            return (char)((b0 << 8) | (b1 & 0xff));
        }

        public int Unmarshal_int()
        {
            if (_pos + 4 > Size) throw new MarshalException();
            byte b0 = GetByte(_pos++);
            byte b1 = GetByte(_pos++);
            byte b2 = GetByte(_pos++);
            byte b3 = GetByte(_pos++);
            return (int)((
                ((b0 & 0xff) << 24) |
                ((b1 & 0xff) << 16) |
                ((b2 & 0xff) << 8) |
                ((b3 & 0xff) << 0)));
        }

        public long Unmarshal_long()
        {
            if (_pos + 8 > Size) throw new MarshalException();
            byte b0 = GetByte(_pos++);
            byte b1 = GetByte(_pos++);
            byte b2 = GetByte(_pos++);
            byte b3 = GetByte(_pos++);
            byte b4 = GetByte(_pos++);
            byte b5 = GetByte(_pos++);
            byte b6 = GetByte(_pos++);
            byte b7 = GetByte(_pos++);
            return ((((long)b0 & 0xff) << 56) |
                (((long)b1 & 0xff) << 48) |
                (((long)b2 & 0xff) << 40) |
                (((long)b3 & 0xff) << 32) |
                (((long)b4 & 0xff) << 24) |
                (((long)b5 & 0xff) << 16) |
                (((long)b6 & 0xff) << 8) |
                (((long)b7 & 0xff) << 0));
        }

        public float Unmarshal_float()
        {
            int i = Unmarshal_int();
            return BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
        }

        public double Unmarshal_double()
        {
            return BitConverter.Int64BitsToDouble(Unmarshal_long());
        }

        public int Uncompact_uint32()
        {
            if (_pos == Size) throw new MarshalException();
            switch (GetByte(_pos) & 0xe0)
            {
                case 0xe0:
                    Unmarshal_byte();
                    return Unmarshal_int();
                case 0xc0:
                    return Unmarshal_int() & (int)~0xc0000000;
                case 0xa0:
                case 0x80:
                    return Unmarshal_short() & short.MaxValue;
            }
            return Unmarshal_byte();
        }

        public int Uncompact_sint32()
        {
            if (_pos == Size) throw new MarshalException();
            switch (GetByte(_pos) & 0xf0)
            {
                case 0xf0:
                    Unmarshal_byte();
                    return -Unmarshal_int();
                case 0xe0:
                    Unmarshal_byte();
                    return Unmarshal_int();
                case 0xd0:
                    return -(Unmarshal_int() & (int)~0xd0000000);
                case 0xc0:
                    return Unmarshal_int() & (int)~0xc0000000;
                case 0xb0:
                case 0xa0:
                    return -(Unmarshal_short() & ~(ushort)0xa000);
                case 0x90:
                case 0x80:
                    return Unmarshal_short() & ~(ushort)0x8000;
                case 0x70:
                case 0x60:
                case 0x50:
                case 0x40:
                    return -(Unmarshal_byte() & ~0x40);
            }
            return Unmarshal_byte();
        }

        public Octets Unmarshal_Octets()
        {
            int size = Uncompact_uint32();
            if (_pos + size > Size) throw new MarshalException();
            Octets o = new Octets(this, _pos, size);
            _pos += size;
            return o;
        }

        public byte[] Unmarshal_bytes()
        {
            int size = Uncompact_uint32();
            if (_pos + size > Size) throw new MarshalException();
            byte[] copy = new byte[size];
            Array.Copy(Buffer(), _pos, copy, 0, size);
            _pos += size;
            return copy;
        }

        public OctetsStream Unmarshal(Octets os)
        {
            int size = Uncompact_uint32();
            if (_pos + size > Size) throw new MarshalException();
            os.Replace(this, _pos, size);
            _pos += size;
            return this;
        }

        public string Unmarshal_string()
        {
            return Unmarshal_string(DefaultEncoding);
        }

        public string Unmarshal_string(Encoding encoding)
        {
            try
            {
                int size = Uncompact_uint32();
                if (_pos + size > Size) throw new MarshalException();
                int cur = _pos;
                _pos += size;
                return (encoding == null) ?
                DefaultEncoding.GetString(Buffer(), cur, size) : encoding.GetString(Buffer(), cur, size);
            }
            catch
            {
                throw new MarshalException();
            }
        }

        public OctetsStream Unmarshal(IMarshal m)
        {
            return m.Unmarshal(this);
        }
        #endregion
    }
}