using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.Services
{
    public class MemeService
    {
        public ICollection<MemeCommand> MemeCommands { get; private set; }

        public delegate void ReloadCommands();
        public event ReloadCommands OnReloadCommands;

        private readonly string _memeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];

        private readonly string[] _imageFormats =
        {
            ".jpg",
            ".png",
            ".gif"
        };

        private readonly string[] _videoFormats =
        {
            ".mp4",
            ".webm"
        };

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

        public bool EditCommand(string oldCommandName, MemeCommand newCommand)
        {
            if (!ValidateNewCommand(newCommand) || !DeleteCommand(oldCommandName))
            {
                return false;
            }

            MemeCommands.Add(newCommand);
            SaveCommands();
            OnReloadCommands?.Invoke();

            return true;
        }

        public bool RemoveCommand(string commandName)
        {
            if (!DeleteCommand(commandName))
            {
                return false;
            }

            SaveCommands();
            OnReloadCommands?.Invoke();

            return true;
        }

        public MemeResultType GetMemeType(string value)
        {
            var valueToProcess = value.ToLower();

            // Totally original regex (https://stackoverflow.com/questions/3809401/what-is-a-good-regular-expression-to-match-a-url)
            Regex urlRegex = new Regex(@"(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&\/\/=]*)");
            if (urlRegex.IsMatch(valueToProcess))
            {
                var isImage = _imageFormats.Any(imageFormat => valueToProcess.EndsWith(imageFormat));
                var isVideo = _videoFormats.Any(videoFormat => valueToProcess.EndsWith(videoFormat));

                if (isImage && isVideo)
                {
                    return MemeResultType.Unknown;
                }

                if (isImage)
                {
                    return MemeResultType.Image;
                }
                else if (isVideo)
                {
                    return MemeResultType.Video;
                }
                else
                {
                    return MemeResultType.Text;
                }
            }
            else
            {
                return MemeResultType.Text;
            }
        }

        #endregion

        #region Private methods

        private bool ValidateNewCommand(MemeCommand newCmd)
        {
            return !MemeCommands.Contains(newCmd);
        }

        private bool DeleteCommand(string commandName)
        {
            var command = MemeCommands.FirstOrDefault(x => x.Name == commandName);
            if (command == null)
            {
                return false;
            }

            MemeCommands.Remove(command);

            return true;
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
