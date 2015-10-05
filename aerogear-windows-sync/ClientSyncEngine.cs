using System;
using System.Collections.Generic;
using synchronizers;

namespace aerogear_windows_sync
{
    public class ClientSyncEngine<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        private readonly IClientSynchronizer<T, TS, TD> _clientSynchronizer;
        private readonly IClientDataStore<T, TS, TD> _clientDataStore;
        private readonly IPatchObservable<T> _patchObservable;

        public ClientSyncEngine(IClientSynchronizer<T, TS, TD> clientSynchronizer,
            IClientDataStore<T, TS, TD> clientDataStore, IPatchObservable<T> patchObservable)
        {
            _clientSynchronizer = clientSynchronizer;
            _clientDataStore = clientDataStore;
            _patchObservable = patchObservable;
        }

        public void AddDocument(IClientDocument<T> document)
        {
            SaveDocument(document);
            SaveBackupShadow(SaveShadow(new DefaultShadowDocument<T>(0, 0, document)));
        }

        public IPatchMessage<TS, TD> Diff(IClientDocument<T> document)
        {
            var documentId = document.Id();
            var clientId = document.ClientId();
            var shadow = GetShadowDocument(documentId, clientId);
            var edit = ServerDiff(document, shadow);
            SaveEdits(edit, documentId, clientId);
            var patchedShadow = DiffPatchShadow(shadow, edit);
            SaveShadow(IncrementClientVersion(patchedShadow));
            return GetPendingEdits(document.Id(), document.ClientId());
        }

        public void Patch(IPatchMessage<TS, TD> patchMessage)
        {
            var patchedShadow = PatchShadow(patchMessage);
            PatchDocument(patchedShadow);
            SaveBackupShadow(patchedShadow);
        }

        private IShadowDocument<T> PatchShadow(IPatchMessage<TS, TD> patchMessage)
        {
            var documentId = patchMessage.DocumentId();
            var clientId = patchMessage.ClientId();
            var shadow = GetShadowDocument(documentId, clientId);
            foreach (var edit in patchMessage.Edits())
            {
                if (ClientPacketDropped(edit, shadow))
                {
                    shadow = RestoreBackup(shadow, edit);
                    continue;
                }
                if (HasServerVersion(edit, shadow))
                {
                    DiscardEdit(edit, documentId, clientId, patchMessage.Edits());
                    continue;
                }
                if (AllVersionsMatch(edit, shadow) || IsSeedVersion(edit))
                {
                    var patchedShadow = _clientSynchronizer.PatchShadow(edit, shadow);
                    if (IsSeedVersion(edit))
                    {
                        shadow = SaveShadowAndRemoveEdit(WithClientVersion(patchedShadow, 0), edit);
                    }
                    else
                    {
                        shadow = SaveShadowAndRemoveEdit(IncrementServerVersion(patchedShadow), edit);
                    }
                }
            }
            return shadow;
        }

        private IDocument<T> PatchDocument(IShadowDocument<T> shadowDocument)
        {
            var clientDocument = GetClientDocumentForShadow(shadowDocument);
            var edit = ClientDiff(clientDocument, shadowDocument);
            var patched = PatchDocument(edit, clientDocument);
            SaveDocument(patched);
            SaveBackupShadow(shadowDocument);
            _patchObservable.Changed();
            _patchObservable.NotifyPatched(patched);
            return patched;
        }

        private IClientDocument<T> GetClientDocumentForShadow(IShadowDocument<T> shadow)
        {
            return _clientDataStore.GetClientDocument(shadow.Document().Id(), shadow.Document().ClientId());
        }

        private IClientDocument<T> PatchDocument(TS edit, IClientDocument<T> clientDocument)
        {
            return _clientSynchronizer.PatchDocument(edit, clientDocument);
        }

        private bool ClientPacketDropped(TS edit, IShadowDocument<T> shadow)
        {
            return edit.ClientVersion() < shadow.ClientVersion() && !IsSeedVersion(edit);
        }

        private bool IsSeedVersion(TS edit)
        {
            return edit.ClientVersion() == -1;
        }

        private IShadowDocument<T> WithClientVersion(IShadowDocument<T> shadow, long clientVersion)
        {
            return NewShadowDoc(shadow.ServerVersion(), clientVersion, shadow.Document());
        }

        private IShadowDocument<T> SaveShadowAndRemoveEdit(IShadowDocument<T> shadow, TS edit)
        {
            _clientDataStore.RemoveEdit(edit, shadow.Document().Id(), shadow.Document().ClientId());
            return SaveShadow(shadow);
        }

        private IShadowDocument<T> RestoreBackup(IShadowDocument<T> shadow, TS edit)
        {
            var documentId = shadow.Document().Id();
            var clientId = shadow.Document().ClientId();
            var backup = GetBackupShadowDocument(documentId, clientId);
            if (ClientVersionMatch(edit, backup))
            {
                var patchedShadow = _clientSynchronizer.PatchShadow(edit, backup.Shadow());
                _clientDataStore.RemoveEdits(documentId, clientId);
                return SaveShadow(IncrementServerVersion(patchedShadow), edit);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Backup version [{0}] does not match edit client version [{1}]", backup.Version(), edit.ClientVersion()));
            }
        }

        private bool HasServerVersion(TS edit, IShadowDocument<T> shadow)
        {
            return edit.ServerVersion() < shadow.ServerVersion();
        }

        private void DiscardEdit(TS edit, string documentId, string clientId, Queue<TS> iterator)
        {
            _clientDataStore.RemoveEdit(edit, documentId, clientId);
            iterator.Dequeue();
        }

        private bool AllVersionsMatch(TS edit, IShadowDocument<T> shadow)
        {
            return edit.ServerVersion() == shadow.ServerVersion() && edit.ClientVersion() == shadow.ClientVersion();
        }

        private TS ClientDiff(IClientDocument<T> doc, IShadowDocument<T> shadow)
        {
            return _clientSynchronizer.ClientDiff(shadow, doc);
        }

        private IBackupShadowDocument<T> GetBackupShadowDocument(string documentId, string clientId)
        {
            return _clientDataStore.GetBackupShadowDocument(documentId, clientId);
        }

        private bool ClientVersionMatch(TS edit, IBackupShadowDocument<T> backup)
        {
            return edit.ClientVersion() == backup.Version();
        }

        private IShadowDocument<T> IncrementServerVersion(IShadowDocument<T> shadow)
        {
            long serverVersion = shadow.ServerVersion() + 1;
            return NewShadowDoc(serverVersion, shadow.ClientVersion(), shadow.Document());
        }

        private IShadowDocument<T> GetShadowDocument(string documentId, string clientId)
        {
            return _clientDataStore.GetShadowDocument(documentId, clientId);
        }

        private TS ServerDiff(IClientDocument<T> doc, IShadowDocument<T> shadow)
        {
            return _clientSynchronizer.ServerDiff(doc, shadow);
        }

        private void SaveEdits(TS edit, string documentId, string clientId)
        {
            _clientDataStore.SaveEdits(edit, documentId, clientId);
        }

        private IShadowDocument<T> DiffPatchShadow(IShadowDocument<T> shadow, TS edit)
        {
            return _clientSynchronizer.PatchShadow(edit, shadow);
        }

 
        public void AddPatchListener(IPatchListener<T> patchListener)
        {
            _patchObservable.AddPatchListener(patchListener);
        }

        public void RemovePatchListener(IPatchListener<T> patchListener)
        {
            _patchObservable.RemovePatchListener(patchListener);
        }

        public void RemovePatchListeners()
        {
            _patchObservable.RemovePatchListeners();
        }

        public int CountPatchListeners()
        {
            return _patchObservable.CountPatchListeners();
        }


        private IPatchMessage<TS, TD> GetPendingEdits(string documentId, string clientId)
        {
            return _clientSynchronizer.CreatePatchMessage(documentId, clientId, _clientDataStore.GetEdits(documentId, clientId));
        }

        private IShadowDocument<T> IncrementClientVersion(IShadowDocument<T> shadow)
        {
            var clientVersion = shadow.ClientVersion() + 1;
            return NewShadowDoc(shadow.ServerVersion(), clientVersion, shadow.Document());
        }

        private IShadowDocument<T> NewShadowDoc(long serverVersion, long clientVersion, IClientDocument<T> doc)
        {
            return new DefaultShadowDocument<T>(serverVersion, clientVersion, doc);
        }

        private IShadowDocument<T> SaveShadow(IShadowDocument<T> newShadow)
        {
            _clientDataStore.SaveShadowDocument(newShadow);
            return newShadow;
        }

        private IShadowDocument<T> SaveShadow(IShadowDocument<T> shadow, TS edit)
        {
            _clientDataStore.RemoveEdit(edit, shadow.Document().Id(), shadow.Document().ClientId());
            return SaveShadow(shadow);
        }


        private void SaveBackupShadow(IShadowDocument<T> newShadow)
        {
            _clientDataStore.SaveBackupShadowDocument(new DefaultBackupShadowDocument<T>(newShadow.ClientVersion(),
                newShadow));
        }

        private void SaveDocument(IClientDocument<T> document)
        {
            _clientDataStore.SaveClientDocument(document);
        }

    }
}
