using JsonDiffPatch;
using Newtonsoft.Json.Linq;
using synchronizers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aerogear_windows_sync
{
    public class JsonMergePatchClientSynchronizer : IClientSynchronizer<JToken, JsonMergePatchEdit, JsonMergePatchDiff>
    {
        public JsonMergePatchEdit ClientDiff(IShadowDocument<JToken> shadowDocument, IClientDocument<JToken> document)
        {
            var shadow = shadowDocument.Document().Content();
            return JsonMergePatchEdit.WithPatch(document.Content()).Checksum(shadow.GetHashCode().ToString()).Build();
        }
        public JsonMergePatchEdit ServerDiff(IClientDocument<JToken> document, IShadowDocument<JToken> shadowDocument)
        {
            var shadow = shadowDocument.Document().Content();
            return
                JsonMergePatchEdit.WithPatch(shadow)
                    .ServerVersion(shadowDocument.ServerVersion())
                    .ClientVersion(shadowDocument.ClientVersion())
                    .Checksum(shadow.GetHashCode().ToString()).Build();
        }

        public IShadowDocument<JToken> PatchShadow(JsonMergePatchEdit edit, IShadowDocument<JToken> shadowDocument)
        {
            var content = PatchContent(edit, shadowDocument.Document());
            return new DefaultShadowDocument<JToken>(shadowDocument.ServerVersion(), shadowDocument.ClientVersion(),
                new DefaultClientDocument<JToken>(shadowDocument.Document().Id(), shadowDocument.Document().ClientId(), content));
        }
        public IClientDocument<JToken> PatchDocument(JsonMergePatchEdit edit, IClientDocument<JToken> document)
        {
            var content = PatchContent(edit, document);
            return new DefaultClientDocument<JToken>(document.Id(), document.ClientId(), content);
        }

        public IPatchMessage<JsonMergePatchEdit, JsonMergePatchDiff> CreatePatchMessage(string documentId, string clientId, Queue<JsonMergePatchEdit> edits)
        {
            return new JsonMergePatchMessage(documentId, clientId, edits);
        }

        public IPatchMessage<JsonMergePatchEdit, JsonMergePatchDiff> PatchMessageFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IPatchMessage<JsonMergePatchEdit, JsonMergePatchDiff>>(json);
        }

        private static JToken PatchContent(JsonMergePatchEdit edit, IDocument<JToken> document)
        {
            var content = document.Content();
            new JsonPatcher().Patch(ref content, edit.Diff().JsonMergePatch());
            return content;
        }
    }
}
