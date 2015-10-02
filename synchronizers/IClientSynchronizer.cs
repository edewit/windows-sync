using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synchronizers
{
    public interface IClientSynchronizer<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        IShadowDocument<T> PatchShadow(TS edit, IShadowDocument<T> shadowDocument);

        IClientDocument<T> PatchDocument(TS edit, IClientDocument<T> document);

        TS ServerDiff(IClientDocument<T> document, IShadowDocument<T> shadowDocument);

        TS ClientDiff(IShadowDocument<T> shadowDocument, IClientDocument<T> document);

        IPatchMessage<TS, TD> CreatePatchMessage(string documentId, string clientId, Queue<TS> edits);

        IPatchMessage<TS, TD> PatchMessageFromJson(string json);

        //  void AddContent(T content, ObjectNode objectNode, String fieldName);
    }
}
