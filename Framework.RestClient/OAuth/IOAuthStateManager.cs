using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Rest.OAuth
{
    public interface IOAuthStateManager
    {
        void SaveState(string key, OAuthState state);

        OAuthState GetState(string key);
    }
}
