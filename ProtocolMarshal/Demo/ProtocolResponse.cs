using System.Collections.Generic;
using System.Text;

namespace ProtocolMarshal.Demo
{
    public class ProtocolResponse : IMarshal, IProtocol
    {
        public MessageResponse MessageResponse { get; set; }
        public int LoginStage;
        public byte Sex;
        public string Name;
        public Dictionary<string, int> Param;
        public HashSet<MessageEntity> EntitySet;
        public List<MessageEntity> EntityList;
        public MessageEntity[] EntityArray;

        public ProtocolResponse()
        {
            MessageResponse = new MessageResponse();
            LoginStage = 0;
            Sex = 0;
            Name = "";
            Param = new Dictionary<string, int>();
            EntitySet = new HashSet<MessageEntity>();
            EntityList = new List<MessageEntity>();
            EntityArray = new MessageEntity[0];
        }

        public ProtocolResponse(
            int login_stage, 
            byte sex, 
            string name)
        {
            MessageResponse = new MessageResponse();
            LoginStage = login_stage;
            Sex = sex;
            Name = name;
            Param = new Dictionary<string, int>();
            EntitySet = new HashSet<MessageEntity>();
            EntityList = new List<MessageEntity>();
            EntityArray = new MessageEntity[0];
        }

        public OctetsStream Marshal(OctetsStream os)
        {
            MessageResponse.Marshal(os);
            os.Marshal(LoginStage);
            os.Marshal(Sex);
            os.Marshal(Name);

            os.Compact_uint32(Param.Count);
            foreach (KeyValuePair<string, int> e in Param)
            {
                os.Marshal(e.Key);
                os.Marshal(e.Value);
            }

            os.Compact_uint32(EntitySet.Count);
            foreach (MessageEntity v in EntitySet)
            {
                os.Marshal(v);
            }

            os.Compact_uint32(EntityList.Count);
            foreach (MessageEntity v in EntityList)
            {
                os.Marshal(v);
            }

            os.Compact_uint32(EntityArray.Length);
            foreach (MessageEntity v in EntityArray)
            {
                os.Marshal(v);
            }
            return os;
        }

        public OctetsStream Unmarshal(OctetsStream os)
        {
            MessageResponse.Unmarshal(os);
            LoginStage = os.Unmarshal_int();
            Sex = os.Unmarshal_byte();
            Name = os.Unmarshal_string();

            for (var i = os.Uncompact_uint32(); i > 0; --i)
            {
                var k = os.Unmarshal_string();
                var v = os.Unmarshal_int();
                Param.Add(k, v);
            }

            for (var i = os.Uncompact_uint32(); i > 0; --i)
            {
                var v = new MessageEntity();
                v.Unmarshal(os);
                EntitySet.Add(v);
            }

            for (int i = os.Uncompact_uint32(); i > 0; --i)
            {
                var v = new MessageEntity();
                v.Unmarshal(os);
                EntityList.Add(v);
            }

            EntityArray = new MessageEntity[os.Uncompact_uint32()];
            for (var i = EntityArray.Length; i > 0; --i)
            {
                var v = new MessageEntity();
                v.Unmarshal(os);
                EntityArray[EntityArray.Length - i] = v;
            }
            return os;
        }

        public override int GetHashCode()
        {
            int h = 0;
            h = 31 * h + MessageResponse.GetHashCode();
            h = 31 * h + LoginStage;
            h = 31 * h + Sex;
            h = 31 * h + Name.GetHashCode();
            h = 31 * h + Param.GetHashCode();
            h = 31 * h + EntitySet.GetHashCode();
            h = 31 * h + EntityList.GetHashCode();
            h = 31 * h + EntityArray.GetHashCode();
            return h;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(MessageResponse).Append(',');
            sb.Append(LoginStage).Append(',');
            sb.Append(Sex).Append(',');
            sb.Append(Name).Append(',');
            sb.Append(Param).Append(',');
            sb.Append(EntitySet).Append(',');
            sb.Append(EntityList).Append(',');
            sb.Append(EntityArray);
            sb.Append(')');
            return sb.ToString();
        }

        public string ToString(int depth)
        {
            depth++;
            StringBuilder buffer = new StringBuilder();
            buffer.Append('\n');
            for (int i = 0; i < depth - 1; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append('{');
            buffer.Append('\n');

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("MessageResponse").Append(":");
            buffer.Append(MessageResponse.ToString(depth));
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("LoginStage").Append(": ");
            buffer.Append(LoginStage);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("Sex").Append(": ");
            buffer.Append(Sex);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("Name").Append(": ");
            buffer.Append(Name);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("Param").Append(": ");
            buffer.Append(Param);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("EntitySet").Append(": ");
            buffer.Append(EntitySet);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("EntityList").Append(": ");
            buffer.Append(EntityList);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("EntityArray").Append(": ");
            buffer.Append(EntityArray);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth - 1; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append('}');
            buffer.Append(';');
            return buffer.ToString();
        }

        public byte[] ToBytes()
        {
            return new OctetsStream().Marshal(this).GetBytes();
        }

        public void ParseFrom(byte[] stream)
        {
            Unmarshal(new OctetsStream(new Octets(stream)));
        }

        public const int ProtocolType = 11100;

        public int GetProtocolType()
        {
            return ProtocolType;
        }

        public int GetMaxSize()
        {
            return 65535;
        }
    }
}