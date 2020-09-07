// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
namespace System
{

    using System;
    using System.Reflection;

    //  This value type is used for making classlib type safe.
    //
    //  SECURITY : m_ptr cannot be set to anything other than null by untrusted
    //  code.
    //
    //  This corresponds to EE FieldDesc.
    //| <include path='docs/doc[@for="RuntimeFieldHandle"]/*' />
    public struct RuntimeFieldHandle
    {
        private IntPtr m_ptr;

        //| <include path='docs/doc[@for="RuntimeFieldHandle.Value"]/*' />
        public IntPtr Value {
            get {
                return m_ptr;
            }
        }
    }
}
