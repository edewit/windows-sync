using System.Collections.Generic;

namespace synchronizers
{
    public interface IPatchMessage<T, TD> where T : IEdit<TD> where TD : IDiff
    {
        string ClientId();

        string DocumentId();

        Queue<T> Edits();
    }
}