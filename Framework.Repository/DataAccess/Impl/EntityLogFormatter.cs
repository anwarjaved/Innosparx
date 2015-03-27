using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataAccess.Impl
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Security;

    [SecurityCritical]
    internal class EntityLogFormatter : DatabaseLogFormatter
    {
        public EntityLogFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        {
        }

        [SecurityCritical]
        public override void LogCommand<TResult>(
        DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            Write(string.Format(
                "Context '{0}' is executing command '{1}'{2}",
                Context.GetType().Name,
                command.CommandText.Replace(Environment.NewLine, ""),
                Environment.NewLine));
        }

        [SecurityCritical]
        public override void LogResult<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            base.LogResult(command, interceptionContext);
        }
    }
}
