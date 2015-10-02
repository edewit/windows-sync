using JsonDiffPatch;
using Newtonsoft.Json.Linq;
using synchronizers;

namespace aerogear_windows_sync
{
    public class JsonMergePatchDiff : IDiff
    {
        private readonly PatchDocument _jsonMergePatch;
        private readonly int _jsonObjectHashCode;

        private JsonMergePatchDiff(PatchDocument patchDocument, int jsonObjectHashCode)
        {
            _jsonMergePatch = patchDocument;
            _jsonObjectHashCode = jsonObjectHashCode;
        }

        public PatchDocument JsonMergePatch()
        {
            return _jsonMergePatch;
        }

        public static JsonMergePatchDiff FromJToken(JToken jtoken)
        {
            return new JsonMergePatchDiff(PatchDocument.Parse(jtoken.ToString()), jtoken.GetHashCode());
        }

        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }

            if (other == null || (other.GetType() != typeof(JsonMergePatchDiff)))
            {
                return false;
            }

            return _jsonObjectHashCode == ((JsonMergePatchDiff)other)._jsonObjectHashCode;
        }

        public override int GetHashCode()
        {
            return _jsonMergePatch?.GetHashCode() ?? 0;
        }
    }
}