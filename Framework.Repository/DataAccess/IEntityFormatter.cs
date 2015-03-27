using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataAccess
{
    using Framework.Domain;

    public interface IEntityFormatter
    {
        bool OnLoad(Type type, IBaseEntity entity);

        void OnSave(Type type, IBaseEntity entity);
    }
}
