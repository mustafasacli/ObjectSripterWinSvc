namespace Framework.Data.Core.Interfaces
{
    public interface IQueryFormat
    {
        string[] GetFormatKeys();

        string GetFormat();
    }
}
