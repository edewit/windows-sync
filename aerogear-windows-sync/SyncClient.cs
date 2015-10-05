using System;
using synchronizers;
using static aerogear_windows_sync.Arguments;

namespace aerogear_windows_sync
{
    public class SyncClient<T, TS, TD> : ISyncClient<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _path;
        private readonly Uri _uri;
        private readonly ClientSyncEngine<T, TS, TD> _syncEngine;
        private readonly string _subprotocols;

        public SyncClient(Builder<T, TS, TD> builder)
        {
            _host = checkNotNull(builder._host, "host must not be null");
            _path = checkNotNull(builder._path, "path must not be null");
            _syncEngine = checkNotNull(builder._engine, "engine must not be null");
            _port = builder._port;
            _uri = ParseUri(builder._wss, _host, _port, _path);
            _subprotocols = builder._subprotocols;
            if (builder._listener != null)
            {
                _syncEngine.AddPatchListener(builder._listener);
            }
        }

        private static Uri ParseUri(bool wss, string host, int port, string path)
        {
            return new Uri(wss ? "wss" : "ws" + "://" + host + ':' + port + path);
        }


        public ISyncClient<T, TS, TD> Connect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public string ClientId()
        {
            throw new NotImplementedException();
        }

        public void AddDocument(IClientDocument<T> document)
        {
            throw new NotImplementedException();
        }

        public void DiffAndSend(IClientDocument<T> document)
        {
            throw new NotImplementedException();
        }

        public void AddPatchListener(IPatchListener<T> patchListener)
        {
            throw new NotImplementedException();
        }

        public void DeletePatchListener(IPatchListener<T> patchListener)
        {
            throw new NotImplementedException();
        }

        public void DeletePatchListeners()
        {
            throw new NotImplementedException();
        }

        public int CountPatchListeners()
        {
            throw new NotImplementedException();
        }
    }

    public class Builder<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        internal readonly string _host;
        internal int _port;
        internal string _path;
        internal bool _wss;
        internal string _subprotocols;
        internal ClientSyncEngine<T, TS, TD> _engine;
        internal IPatchListener<T> _listener;

        public Builder(string host)
        {
            _host = host;
        }

        public Builder<T, TS, TD> Port(int port)
        {
            _port = port;
            return this;
        }

        public Builder<T, TS, TD> Path(string path)
        {
            _path = path;
            return this;
        }

        public Builder<T, TS, TD> Wss(bool wss)
        {
            _wss = wss;
            return this;
        }

        public Builder<T, TS, TD> Subprotocols(string subprotocols)
        {
            _subprotocols = subprotocols;
            return this;
        }

        public Builder<T, TS, TD> SyncEngine(ClientSyncEngine<T, TS, TD> engine)
        {
            _engine = engine;
            return this;
        }

        public Builder<T, TS, TD> PatchListener(IPatchListener<T> listener)
        {
            _listener = listener;
            return this;
        }

        public SyncClient<T, TS, TD> Build()
        {
            return new SyncClient<T, TS, TD>(this);
        }
    }

    public class Arguments
    {

        private Arguments()
        {
        }

        public static dynamic checkNotNull(object arg, string text)
        {
            if (arg == null)
            {
                throw new NullReferenceException(text);
            }
            return arg;
        }
    }


}
