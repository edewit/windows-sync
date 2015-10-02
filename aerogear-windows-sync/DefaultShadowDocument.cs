using System;
using Newtonsoft.Json.Linq;
using synchronizers;

namespace aerogear_windows_sync
{
    public class DefaultShadowDocument<T> : IShadowDocument<T>
    {
        private readonly long _clientVersion;
        private readonly IClientDocument<T> _document;
        private readonly long _serverVersion;

        public DefaultShadowDocument(long serverVersion, long clientVersion, IClientDocument<T> document)
        {
            _serverVersion = serverVersion;
            _clientVersion = clientVersion;
            _document = document;
        }

        public long ClientVersion()
        {
            return _clientVersion;
        }

        public IClientDocument<T> Document()
        {
            return _document;
        }

        public long ServerVersion()
        {
            return _serverVersion;
        }

        public override string ToString()
        {
            return string.Format("DefaultShadowDocument[serverVersion={0}, clientVersion={1}, document={2}]", _serverVersion, _clientVersion, _document);
        }
    }
}