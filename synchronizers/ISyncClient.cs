using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synchronizers
{
    public interface ISyncClient<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        ISyncClient<T, TS, TD> Connect();

        bool IsConnected();

        void Disconnect();

        string ClientId();

        void AddDocument(IClientDocument<T> document);

        void DiffAndSend(IClientDocument<T> document);

        void AddPatchListener(IPatchListener<T> patchListener);

        void DeletePatchListener(IPatchListener<T> patchListener);

        void DeletePatchListeners();

        int CountPatchListeners();
    }
}
