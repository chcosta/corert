// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Threading
{
    public sealed partial class Mutex
    {
        private static void VerifyNameForCreate(string name)
        {
            if (name != null)
            {
                throw new PlatformNotSupportedException(SR.PlatformNotSupported_NamedSynchronizationPrimitives);
            }
        }

        private void CreateMutexCore(bool initiallyOwned, string name, out bool createdNew)
        {
            Debug.Assert(name == null);

            SafeWaitHandle = WaitSubsystem.NewMutex(initiallyOwned);
            createdNew = true;
        }

        private static OpenExistingResult OpenExistingWorker(string name, out Mutex result)
        {
            throw new PlatformNotSupportedException(SR.PlatformNotSupported_NamedSynchronizationPrimitives);
        }

        public void ReleaseMutex()
        {
            // The field value is modifiable via the public <see cref="WaitHandle.SafeWaitHandle"/> property, save it locally
            // to ensure that one instance is used in all places in this method
            SafeWaitHandle waitHandle = _waitHandle;
            if (waitHandle == null)
            {
                ThrowInvalidHandleException();
            }

            waitHandle.DangerousAddRef();
            try
            {
                WaitSubsystem.ReleaseMutex(waitHandle.DangerousGetHandle());
            }
            finally
            {
                waitHandle.DangerousRelease();
            }
        }
    }
}
