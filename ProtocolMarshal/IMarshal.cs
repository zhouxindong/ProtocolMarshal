namespace ProtocolMarshal
{
    public interface IMarshal
    {
        OctetsStream Marshal(OctetsStream os);
        OctetsStream Unmarshal(OctetsStream os);
    }
}