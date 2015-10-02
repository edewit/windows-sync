namespace synchronizers
{
    public class DefaultShadowDocument<T> : IShadowDocument<T>
    {
        private readonly long _serverVersion;
        private readonly long _clientVersion;
        private readonly IClientDocument<T> _document;

        public DefaultShadowDocument(long serverVersion, long clientVersion, IClientDocument<T> document)
        {
            _serverVersion = serverVersion;
            _clientVersion = clientVersion;
            _document = document;
        }

        public long ServerVersion()
        {
            return _serverVersion;
        }

        public long ClientVersion()
        {
            return _clientVersion;
        }

        public IClientDocument<T> Document()
        {
            return _document;
        }

        public override string ToString()
        {
            return string.Format("DefaultShadowDocument[serverVersion={0}, clientVersion={1}, document={2}]", _serverVersion, _clientVersion, _document);
        }
    }
}