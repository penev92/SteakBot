using Newtonsoft.Json;
using SteakBot.Core.Objects;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace SteakBot.Core.Services
{
    public class MemeService
    {
        private static readonly string MemeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];
        private static readonly string MemeCommandsOriginRelativeFilePath = ConfigurationManager.AppSettings["memeCommandsOriginRelativeFilePath"];

        public delegate void ReloadCommands();
        public event ReloadCommands ReloadCommandsEvent;

        public IList<MemeCommand> MemeCommands { get; set; }

        public MemeService()
        {
            LoadCommands();
        }

        #region Public methods

        public bool SaveCommand(MemeCommand newCmd)
        {
            if (ValidateNewCommand(newCmd))
            {
                MemeCommands.Add(newCmd);
                using (var fileWriter = new StreamWriter(MemeCommandsFileName))
                {
                    using (var jsonTextWriter = new JsonTextWriter(fileWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, MemeCommands);
                    }
                }

#if DEBUG
                // Replace original file.
                using (var fileWriter = new StreamWriter(MemeCommandsOriginRelativeFilePath))
                {
                    using (var jsonTextWriter = new JsonTextWriter(fileWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, MemeCommands);
                    }
                }
#endif
                return true;
            }

            return false;
        }

        public IList<MemeCommand> LoadCommands()
        {
            using (var fileReader = new StreamReader(MemeCommandsFileName))
            {
                using (var jsonTextReader = new JsonTextReader(fileReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    MemeCommands = jsonSerializer.Deserialize<IList<MemeCommand>>(jsonTextReader);

                    return MemeCommands;
                }
            }
        }

        #endregion

        #region Private methods

        private bool ValidateNewCommand(MemeCommand newCmd)
        {
            if (MemeCommands.Contains(newCmd))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
