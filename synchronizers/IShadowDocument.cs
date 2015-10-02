namespace synchronizers
{
    public interface IShadowDocument<T>
    {
        long ServerVersion();

        long ClientVersion();

        IClientDocument<T> Document();

    }
}