namespace SteakBot.Core.Abstractions.Providers
{
    public interface IAudioFilePathProvider
    {
        string GetPath(string audioFileId);
    }
}