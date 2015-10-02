using Newtonsoft.Json.Linq;
using synchronizers;

namespace aerogear_windows_sync
{
    public class JsonMergePatchEdit : IEdit<JsonMergePatchDiff>
    {
        private readonly JsonMergePatchDiff _diff;

        private JsonMergePatchEdit(Builder builder)
        {
            ClientVersion = builder._clientVersion;
            ServerVersion = builder._serverVersion;
            Checksum = builder._checksum;
            _diff = builder._diff;
        }

        public long ClientVersion { get; }
        public long ServerVersion { get; }
        public string Checksum { get; }

        public JsonMergePatchDiff Diff()
        {
            return _diff;
        }

        public static Builder WithPatch(JToken patch)
        {
            return new Builder(patch);
        }

        public static Builder WithChecksum(string checksum)
        {
            return new Builder(checksum);
        }

        public class Builder
        {
            internal string _checksum;
            internal long _clientVersion;
            internal JsonMergePatchDiff _diff;
            internal long _serverVersion;

            internal Builder(JToken patch)
            {
                _diff = JsonMergePatchDiff.FromJToken(patch);
            }

            private Builder(string checksum)
            {
                _checksum = checksum;
            }

            public Builder ServerVersion(long serverVersion)
            {
                _serverVersion = serverVersion;
                return this;
            }

            public Builder ClientVersion(long clientVersion)
            {
                _clientVersion = clientVersion;
                return this;
            }

            public Builder Checksum(string checksum)
            {
                _checksum = checksum;
                return this;
            }

            public Builder Patch(JToken patch)
            {
                _diff = JsonMergePatchDiff.FromJToken(patch);
                return this;
            }

            public JsonMergePatchEdit Build()
            {
                return new JsonMergePatchEdit(this);
            }
        }
    }
}