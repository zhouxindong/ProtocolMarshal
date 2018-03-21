using System.Text;

namespace ProtocolMarshal.Demo
{
    public class MessageEntity : IMarshal
    {
        public int State { get; set; }
        public int Id { get; set; }
        public bool GeneralCheck { get; set; }
        public string Precondition { get; set; }

        public MessageEntity()
        {
            State = 0;
            Id = 0;
            GeneralCheck = true;
            Precondition = "";
        }

        public MessageEntity(int state, int id, bool general_check, string precondition)
        {
            State = state;
            Id = id;
            GeneralCheck = general_check;
            Precondition = precondition;
        }

        public OctetsStream Marshal(OctetsStream os)
        {
            os.Marshal(State);
            os.Marshal(Id);
            os.Marshal(GeneralCheck);
            os.Marshal(Precondition);
            return os;
        }

        public OctetsStream Unmarshal(OctetsStream os)
        {
            State = os.Unmarshal_int();
            Id = os.Unmarshal_int();
            GeneralCheck = os.Unmarshal_bool();
            Precondition = os.Unmarshal_string();
            return os;
        }

        public override bool Equals(object right)
        {
            if (this == right)
                return true;
            if (!(right is MessageEntity))
                return false;
            var right_con  = (MessageEntity)right;
            if (State != right_con.State)
                return false;
            if (Id != right_con.Id)
                return false;
            if (GeneralCheck != right_con.GeneralCheck)
                return false;
            if (!Precondition.Equals(right_con.Precondition))
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            h = 31 * h + State;
            h = 31 * h + Id;
            h = 31 * h + (GeneralCheck ? 1231 : 1237);
            h = 31 * h + Precondition.GetHashCode();
            return h;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(State).Append(',');
            sb.Append(Id).Append(',');
            sb.Append(GeneralCheck).Append(',');
            sb.Append(Precondition);
            sb.Append(')');
            return sb.ToString();
        }

        public string ToString(int depth)
        {
            depth++;
            var buffer = new StringBuilder();
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
            buffer.Append("State").Append(": ");
            buffer.Append(State);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("Id").Append(": ");
            buffer.Append(Id);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("GeneralCheck").Append(": ");
            buffer.Append(GeneralCheck);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("Precondition").Append(": ");
            buffer.Append(Precondition);
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