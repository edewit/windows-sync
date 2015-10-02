namespace synchronizers
{
    public interface IClientDocument<T> : IDocument<T>
    {
        string ClientId();
    }
}