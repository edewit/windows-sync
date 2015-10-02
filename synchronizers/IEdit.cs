namespace synchronizers
{
    public interface IEdit<out T> where T : IDiff
    {
        long ClientVersion();

        long ServerVersion();

        string Checksum();

        T Diff();
    }
}