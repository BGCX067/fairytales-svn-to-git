using System;
using System.Collections.Generic;
using System.Text;

namespace ConeFabric.FairyTales.Acceptance
{
    public class Command
    {
        public Command(string action, string subject, string argument)
        {
            this.action = action;
            this.subject = subject;
            this.argument = argument;
        }
        public string Action { get { return action; } }
        public string Subject { get { return subject; } }
        public string Argument { get { return argument; } }
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", action, subject, argument).Trim();
        }
        private string action, subject, argument;
    }
}
