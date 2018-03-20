namespace ProtocolMarshal
{
    public interface IProtocol
    {
        byte[] ToBytes();
        void ParseFrom(byte[] stream);
        int GetMaxSize();
        int GetType();
    }
}