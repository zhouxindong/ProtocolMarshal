using System.Text;

namespace ProtocolMarshal.Demo
{
    public class MessageResponse : IMarshal
    {
        public int State { get; set; }
        public string Msg { get; set; }
        public long Token { get; set; }

        public MessageResponse()
        {
            State = 0;
            Msg = "";
            Token = 0L;
        }

        public MessageResponse(int state, string msg, long token)
        {
            State = state;
            Msg = msg;
            Token = token;
        }

        public OctetsStream Marshal(OctetsStream os)
        {
            os.Marshal(State);
            os.Marshal(Msg);
            os.Marshal(Token);
            return os;
        }

        public OctetsStream Unmarshal(OctetsStream os)
        {
            State = os.Unmarshal_int();
            Msg = os.Unmarshal_string();
            Token = os.Unmarshal_long();
            return os;
        }

        public override bool Equals(object right)
        {
            if (this == right)
                return true;
            if (!(right is MessageResponse))
                return false;
            var rh = (MessageResponse)right;
            if (State != rh.State)
                return false;
            if (!Msg.Equals(rh.Msg))
                return false;
            if (Token != rh.Token)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            h = 31 * h + State;
            h = 31 * h + Msg.GetHashCode();
            h = 31 * h + (int)(Token ^ (Token >> 32));
            return h;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(State).Append(',');
            sb.Append(Msg).Append(',');
            sb.Append(Token);
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
            buffer.Append("state").Append(": ");
            buffer.Append(State);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("msg").Append(": ");
            buffer.Append(Msg);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("token").Append(": ");
            buffer.Append(Token);
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
    }
}