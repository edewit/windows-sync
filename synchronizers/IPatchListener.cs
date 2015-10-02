namespace synchronizers
{
    public interface IPatchListener<T>
    {
        void Patched(IClientDocument<T> patchedDocument);
    }
}