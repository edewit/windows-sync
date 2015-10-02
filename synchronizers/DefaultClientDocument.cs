namespace synchronizers
{
    public class DefaultClientDocument<T> : DefaultDocument<T>, IClientDocument<T>
    {
        private readonly string _clientId;

        public DefaultClientDocument(string id, string clientId, T content) : base(id, content)
        {
            _clientId = clientId;
        }

        public string ClientId()
        {
            return _clientId;
        }

        public override string ToString()
        {
            return string.Format("DefaultClientDocument[id={0}, clientId={1}, content={2}]", Id(), _clientId, Content());
        }
    }
}
