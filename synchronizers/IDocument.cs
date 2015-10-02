namespace synchronizers
{
    public interface IDocument<T>
    {
        string Id();

        T Content();
    }
}