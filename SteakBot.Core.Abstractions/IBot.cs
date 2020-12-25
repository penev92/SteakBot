using System;
using System.Threading.Tasks;

namespace SteakBot.Core.Abstractions
{
    public interface IBot : IDisposable
    {
        Task RunAsync();
    }
}
