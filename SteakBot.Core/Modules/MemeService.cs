using Newtonsoft.Json;
using SteakBot.Core.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

namespace SteakBot.Core.Modules
{
    public class MemeService
    {
        private static readonly string MemeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];
        private static readonly string MemeCommandsOriginRelativeFilePath = ConfigurationManager.AppSettings["memeCommandsOriginRelativeFilePath"];

        private IList<MemeCommand> Commands { get; set; }

        public delegate void ReloadCommands();
        public event ReloadCommands ReloadCommandsEvent;

        #region Public methods

        public bool SaveCommand(MemeCommand newCmd)
        {
            if (ValidateNewCommand(newCmd))
            {
                Commands.Add(newCmd);
                using (var fileWriter = new StreamWriter(MemeCommandsFileName))
                {
                    using (var jsonTextWriter = new JsonTextWriter(fileWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, Commands);
                    }
                }

#if DEBUG
                // Replace original file.
                using (var fileWriter = new StreamWriter(MemeCommandsOriginRelativeFilePath))
                {
                    using (var jsonTextWriter = new JsonTextWriter(fileWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, Commands);
                    }
                }
#endif
                ReloadCommandsEvent.Invoke();

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
                    Commands = jsonSerializer.Deserialize<IList<MemeCommand>>(jsonTextReader);

                    return Commands;
                }
            }
        }

        #endregion

        #region Private methods

        private bool ValidateNewCommand(MemeCommand newCmd)
        {
            if (Commands.Contains(newCmd))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
