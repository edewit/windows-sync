namespace synchronizers
{
    public class DefaultDocument<T> : IDocument<T>
    {
        private readonly string _id;
        private readonly T _content;

        public DefaultDocument(string id, T content)
        {
            _id = id;
            _content = content;
        }

        public string Id()
        {
            return _id;
        }

        public T Content()
        {
            return _content;
        }

        public override string ToString()
        {
            return string.Format("DefaultDocument[id={0}, content={1}]", _id, _content);
        }
    }
}