namespace synchronizers
{
    public class DefaultBackupShadowDocument<T> : IBackupShadowDocument<T>
    {
        private readonly long _version;
        private readonly IShadowDocument<T> _shadow;

        public DefaultBackupShadowDocument(long version, IShadowDocument<T> shadow)
        {
            this._version = version;
            this._shadow = shadow;
        }

        public long Version()
        {
            return _version;
        }

        public IShadowDocument<T> Shadow()
        {
            return _shadow;
        }

        public override string ToString()
        {
            return string.Format("DefaultBackupShadowDocument[version={0}, shadow={1}]", _version, _shadow);
        }
    }
}