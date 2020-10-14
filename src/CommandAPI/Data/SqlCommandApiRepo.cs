using System;
using System.Collections.Generic;
using CommandAPI.Models;
using System.Linq;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext __context;
        public SqlCommandAPIRepo(CommandContext context)
        {
            __context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            __context.CommandItems.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            __context.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return __context.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return __context.CommandItems.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return (__context.SaveChanges() >= 0);
        }

        public void UpdateCommand(Command cmd)
        {
            // we don't need to do anything here
        }
    }
}