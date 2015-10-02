namespace synchronizers
{
    public interface IBackupShadowDocument<T>
    {
        long Version();

        IShadowDocument<T> Shadow();
    }
}