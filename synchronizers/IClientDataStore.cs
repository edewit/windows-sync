namespace synchronizers
{
    public interface IClientDataStore<T, TS, TD> : IDataStore<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        void SaveClientDocument(IClientDocument<T> document);

        IClientDocument<T> GetClientDocument(string documentId, string clientId);
    }
}
    
