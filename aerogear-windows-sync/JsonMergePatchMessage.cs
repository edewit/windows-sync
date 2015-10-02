using System.Collections.Generic;
using synchronizers;

namespace aerogear_windows_sync
{
    public class JsonMergePatchMessage : IPatchMessage<JsonMergePatchEdit, JsonMergePatchDiff>
    {
        private readonly string _clientId;
        private readonly string _documentId;
        private readonly Queue<JsonMergePatchEdit> _edits;

        public JsonMergePatchMessage(string documentId, string clientId, Queue<JsonMergePatchEdit> edits)
        {
            _documentId = documentId;
            _clientId = clientId;
            _edits = edits;
        }

        public string ClientId()
        {
            return _clientId;
        }

        public string DocumentId()
        {
            return _documentId;
        }

        public Queue<JsonMergePatchEdit> Edits()
        {
            return _edits;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (JsonMergePatchMessage) && Equals((JsonMergePatchMessage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _clientId?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (_documentId?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (_edits?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        private bool Equals(JsonMergePatchMessage other)
        {
            return string.Equals(_clientId, other._clientId) && string.Equals(_documentId, other._documentId) && Equals(_edits, other._edits);
        }

        public override string ToString()
        {
            return string.Format("JsonMergePatchMessage[documentId={0}, clientId={1}, edits={2}]", _documentId, _clientId, _edits);
        }
    }
}