// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
namespace System
{

    using System;

    // Enum used to indicate all the elements of the
    // VOS it is valid to attach this element to.
    //| <include path='docs/doc[@for="AttributeTargets"]/*' />
    [Flags]
    public enum AttributeTargets
    {
        //| <include path='docs/doc[@for="AttributeTargets.Assembly"]/*' />
        Assembly      = 0x0001,
        //| <include path='docs/doc[@for="AttributeTargets.Module"]/*' />
        Module        = 0x0002,
        //| <include path='docs/doc[@for="AttributeTargets.Class"]/*' />
        Class         = 0x0004,
        //| <include path='docs/doc[@for="AttributeTargets.Struct"]/*' />
        Struct        = 0x0008,
        //| <include path='docs/doc[@for="AttributeTargets.Enum"]/*' />
        Enum          = 0x0010,
        //| <include path='docs/doc[@for="AttributeTargets.Constructor"]/*' />
        Constructor   = 0x0020,
        //| <include path='docs/doc[@for="AttributeTargets.Method"]/*' />
        Method        = 0x0040,
        //| <include path='docs/doc[@for="AttributeTargets.Property"]/*' />
        Property      = 0x0080,
        //| <include path='docs/doc[@for="AttributeTargets.Field"]/*' />
        Field         = 0x0100,
        //| <include path='docs/doc[@for="AttributeTargets.Event"]/*' />
        Event         = 0x0200,
        //| <include path='docs/doc[@for="AttributeTargets.Interface"]/*' />
        Interface     = 0x0400,
        //| <include path='docs/doc[@for="AttributeTargets.Parameter"]/*' />
        Parameter     = 0x0800,
        //| <include path='docs/doc[@for="AttributeTargets.Delegate"]/*' />
        Delegate      = 0x1000,

        //| <include path='docs/doc[@for="AttributeTargets.ReturnValue"]/*' />
        ReturnValue   = 0x2000,

        //| <include path='docs/doc[@for="AttributeTargets.All"]/*' />
        All           = Assembly | Module   | Class | Struct | Enum      | Constructor |
                        Method   | Property | Field | Event  | Interface | Parameter   | Delegate | ReturnValue,
    }
}
