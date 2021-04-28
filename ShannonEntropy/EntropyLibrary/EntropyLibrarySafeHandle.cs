using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace ShannonEntropy.EntropyLibrary
{
    public class EntropyLibrarySafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public EntropyLibrarySafeHandle()
            : base(true)
        {

        }

        protected override bool ReleaseHandle()
        {
            if (!this.IsClosed)
                EntropyLibraryWrapper.ReleaseEntropyLibrary(this);
            return true;
        }
    }
}
