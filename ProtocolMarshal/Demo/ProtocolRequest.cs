using System.Text;

namespace ProtocolMarshal.Demo
{
    public class ProtocolRequest : IMarshal, IProtocol
    {
        public MessageRequest MessageRequest { get; set; }

        public ProtocolRequest()
        {
            MessageRequest = new MessageRequest();
        }

        public ProtocolRequest(MessageRequest req)
        {
            MessageRequest = req;
        }

        public OctetsStream Marshal(OctetsStream os)
        {
            MessageRequest.Marshal(os);
            return os;
        }

        public OctetsStream Unmarshal(OctetsStream os)
        {
            MessageRequest.Unmarshal(os);
            return os;
        }

        public override int GetHashCode()
        {
            int h = 0;
            h = 31 * h + MessageRequest.GetHashCode();
            return h;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(MessageRequest);
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
            buffer.Append("MessageRequest").Append(":");
            buffer.Append(MessageRequest.ToString(depth));
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

        public const int ProtocolType = 1100;

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