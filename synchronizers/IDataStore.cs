using System;
using System.Collections.Generic;

namespace synchronizers
{
    public interface IDataStore<T, TS, TD> where TS : IEdit<TD> where TD : IDiff
    {
        void SaveShadowDocument(IShadowDocument<T> shadowDocument);

        IShadowDocument<T> GetShadowDocument(string documentId, string clientId);

        void SaveBackupShadowDocument(IBackupShadowDocument<T> backupShadow);

        IBackupShadowDocument<T> GetBackupShadowDocument(string documentId, string clientId);

        void SaveEdits(TS edit, string documentId, string clientId);

        Queue<TS> GetEdits(string documentId, string clientId);

        void RemoveEdit(TS edit, string documentId, string clientId);

        void RemoveEdits(string documentId, string clientId);
    }
}