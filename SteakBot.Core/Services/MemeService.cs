using System.Collections.Generic;
using System.Configuration;
using System.IO;
using SteakBot.Core.Objects;
using Newtonsoft.Json;

namespace SteakBot.Core.Services
{
    public class MemeService
    {
        private static readonly string MemeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];

        public delegate void ReloadCommands();
        public event ReloadCommands ReloadCommandsEvent;

        public IList<MemeCommand> MemeCommands { get; private set; }

        public MemeService()
        {
            LoadCommands();
        }

        #region Public methods

        public bool SaveCommand(MemeCommand newCmd)
        {
            if (!ValidateNewCommand(newCmd))
            {
                return false;
            }

            MemeCommands.Add(newCmd);
            using (var fileWriter = new StreamWriter(MemeCommandsFileName))
            using (var jsonTextWriter = new JsonTextWriter(fileWriter))
            {
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(jsonTextWriter, MemeCommands);
            }

            ReloadCommandsEvent?.Invoke();

            return true;
        }

        #endregion

        #region Private methods

        private void LoadCommands()
        {
            using (var fileReader = new StreamReader(MemeCommandsFileName))
            using (var jsonTextReader = new JsonTextReader(fileReader))
            {
                var jsonSerializer = new JsonSerializer();
                MemeCommands = jsonSerializer.Deserialize<IList<MemeCommand>>(jsonTextReader);
            }
        }

        private bool ValidateNewCommand(MemeCommand newCmd)
        {
            return !MemeCommands.Contains(newCmd);
        }

        #endregion
    }
}
