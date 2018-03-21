using System.Text;

namespace ProtocolMarshal.Demo
{
    public class MessageRequest : IMarshal
    {
        public long Aid { get; set; }
        public long Pid { get; set; }
        public long SysTime { get; set; }

        public MessageRequest()
        {
            Aid = 0L;
            Pid = 0L;
            SysTime = 0L;
        }

        public MessageRequest(long aid, long pid, long sys_time)
        {
            Aid = aid;
            Pid = pid;
            SysTime = sys_time;
        }


        public OctetsStream Marshal(OctetsStream os)
        {
            os.Marshal(Aid);
            os.Marshal(Pid);
            os.Marshal(SysTime);
            return os;
        }

        public OctetsStream Unmarshal(OctetsStream os)
        {
            Aid = os.Unmarshal_long();
            Pid = os.Unmarshal_long();
            SysTime = os.Unmarshal_long();
            return os;
        }

        public override bool Equals(object right)
        {
            if (this == right)
                return true;
            if (!(right is MessageRequest))
                return false;
            var rh = (MessageRequest)right;
            if (Aid != rh.Aid)
                return false;
            if (Pid != rh.Pid)
                return false;
            if (SysTime != rh.SysTime)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            h = 31 * h + (int)(Aid ^ (Aid >> 32));
            h = 31 * h + (int)(Pid ^ (Pid >> 32));
            h = 31 * h + (int)(SysTime ^ (SysTime >> 32));
            return h;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(Aid).Append(',');
            sb.Append(Pid).Append(',');
            sb.Append(SysTime);
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
            buffer.Append("aid").Append(": ");
            buffer.Append(Aid);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("pid").Append(": ");
            buffer.Append(Pid);
            buffer.Append(';');
            buffer.Append("\n");

            for (int i = 0; i < depth; i++)
            {
                buffer.Append('\t');
            }
            buffer.Append("sysTime").Append(": ");
            buffer.Append(SysTime);
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