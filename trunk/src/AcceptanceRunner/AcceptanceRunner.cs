using System;
using System.Collections.Generic;
using System.Text;
using WatiN.Core.Interfaces;

namespace ConeFabric.FairyTales.Acceptance
{
    public class AcceptanceRunner
    {
        public AcceptanceRunner(string[] input)
        {
            foreach (string line in input)
            {
                string[] words = line.Split(new[] {" "}, StringSplitOptions.None);
                InsertCommand(words);
            }
        }

        public Command[] Commands
        {
            get { return commands.ToArray(); }
        }

        public Dictionary<string, bool> Results
        {
            get { return results; }
        }

        public string FairyTalesUrl
        {
            get { return _fairyTalesUrl; }
            set { _fairyTalesUrl = value; }
        }

        public void Run(IBrowser browser)
        {
            using (browser)
            {
                foreach (Command command in commands)
                {
                    bool result = false;
                    switch (command.Action)
                    {
                        case "open":
                            {
                                string url = string.Format("{0}{1}.aspx", FairyTalesUrl, command.Subject);

                                browser.GoTo(url);
                                result = browser.Url == url;
                            }
                            break;
                        case "check":
                            {
                                if (command.Subject == "ActiveProject")
                                {
                                    ISpan span = browser.Span("ctl00_ContentPlaceHolder1_activeProject");
                                    if (span.Exists)
                                        result = span.Text == command.Argument.Replace("\"", "");
                                }
                            }
                            break;
                        case "enter":
                            {
                                if (command.Subject == "ProjectInput")
                                {
                                    string input = command.Argument.Replace("\"", "");
                                    ITextField projectInput = browser.TextField("ctl00_ContentPlaceHolder1_projectInput");
                                    if (projectInput.Exists)
                                    {
                                        projectInput.Value = input;
                                        result = projectInput.Value == input;
                                    }
                                }
                            }
                            break;
                        case "press":
                            {
                                if (command.Subject == "AddProject")
                                {
                                    IButton button = browser.Button("ctl00_ContentPlaceHolder1_addProjectButton");
                                    if (button.Exists)
                                    {
                                        button.Click();
                                        result = true;
                                    }
                                }
                            }
                            break;
                    }
                    results.Add(command.ToString(), result);
                }
            }
            results["close"] = true;
        }

        private void InsertCommand(string[] words)
        {
            switch (words[0])
            {
                case "press":
                case "open":
                    commands.Add(new Command(words[0], words[1], string.Empty));
                    break;
                case "enter":
                case "check":
                    {
                        string argument = HandleWhiteSpaceArgument(words);
                        commands.Add(new Command(words[0], words[1], argument));
                    }
                    break;
                default:
                    throw new ArgumentException("Unidentified command {0}", words[0]);
            }
        }

        private static string HandleWhiteSpaceArgument(string[] words)
        {
            var builder = new StringBuilder();
            builder.Append(words[2]);

            if (words.Length > 2)
            {
                for (int i = 3; i < words.Length; ++i)
                    builder.AppendFormat(" {0}", words[i]);
            }
            return builder.ToString();
        }
        private readonly List<Command> commands = new List<Command>();
        private readonly Dictionary<string, bool> results = new Dictionary<string, bool>();
        private string _fairyTalesUrl = "http://localhost:666/";

    }
}