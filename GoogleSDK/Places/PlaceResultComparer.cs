using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{
    public class PlaceResultComparer : IComparer<PlaceResult>
    {
        private readonly IList<string> referenceIDs;

        public PlaceResultComparer(IList<string> referenceIDs)
        {
            this.referenceIDs = referenceIDs;
        }

        public int Compare(PlaceResult x, PlaceResult y)
        {
            if (referenceIDs.Contains(x.ID))
            {
                return -1;
            }

            if (referenceIDs.Contains(y.ID))
            {
                return 1;
            }

            return 0;
        }
    }
}
