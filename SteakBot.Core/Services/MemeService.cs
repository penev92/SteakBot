using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using SteakBot.Core.Objects;

namespace SteakBot.Core.Services
{
    public class MemeService
    {
        public IList<MemeCommand> MemeCommands { get; private set; }

        public delegate void ReloadCommands();
        public event ReloadCommands OnReloadCommands;

        private readonly string _memeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];

        public MemeService()
        {
            LoadCommands();
        }

        #region Public methods

        public bool AddCommand(MemeCommand newCommand)
        {
            if (!ValidateNewCommand(newCommand))
            {
                return false;
            }

            MemeCommands.Add(newCommand);
            SaveCommands();
            OnReloadCommands?.Invoke();
            return true;
        }

        #endregion

        #region Private methods

        private bool ValidateNewCommand(MemeCommand newCmd)
        {
            return !MemeCommands.Contains(newCmd);
        }

        private void LoadCommands()
        {
            var text = File.ReadAllText(_memeCommandsFileName);
            MemeCommands = JsonConvert.DeserializeObject<List<MemeCommand>>(text);
        }

        private void SaveCommands()
        {
            using (var fileWriter = new StreamWriter(_memeCommandsFileName))
            using (var jsonTextWriter = new JsonTextWriter(fileWriter) { Formatting = Formatting.Indented, Indentation = 4 })
            {
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(jsonTextWriter, MemeCommands);
            }
        }

        #endregion
    }
}
