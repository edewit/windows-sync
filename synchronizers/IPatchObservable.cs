namespace synchronizers
{
    public interface IPatchObservable<T>
    {
        void AddPatchListener(IPatchListener<T> listener);

        void RemovePatchListener(IPatchListener<T> listener);

        void RemovePatchListeners();

        void NotifyPatched(IClientDocument<T> patchedDocument);

        void Changed();

        int CountPatchListeners();
    }
}