using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Notification
{
    using System.Collections.Concurrent;

    internal class NotificationInfo
    {
        private readonly ConcurrentDictionary<string, IWeakAction> recipentsAction =
    new ConcurrentDictionary<string, IWeakAction>(StringComparer.OrdinalIgnoreCase);

        public string UniqueID { get; internal set; }

        public string TopicName { get; internal set; }

        public ConcurrentDictionary<string, IWeakAction> Reciepents
        {
            get
            {
                return recipentsAction;
            }
        }
    }
}
