// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
//============================================================
//
// Enum:  NumberStyles.cool
//
// Purpose: Contains valid formats for Numbers recognized by
// the Number class' parsing code.
//
//===========================================================  
namespace System.Globalization
{

    using System;
    //| <include path='docs/doc[@for="NumberStyles"]/*' />
    [Flags]
    public enum NumberStyles {
        // Bit flag indicating that leading whitespace is allowed. Character values
        // 0x0009, 0x000A, 0x000B, 0x000C, 0x000D, and 0x0020 are considered to be
        // whitespace.

        //| <include path='docs/doc[@for="NumberStyles.None"]/*' />
        None                  = 0x00000000,
        //| <include path='docs/doc[@for="NumberStyles.AllowLeadingWhite"]/*' />
        AllowLeadingWhite     = 0x00000001,
        //| <include path='docs/doc[@for="NumberStyles.AllowTrailingWhite"]/*' />
        AllowTrailingWhite    = 0x00000002, //Bitflag indicating trailing whitespace is allowed.
        //| <include path='docs/doc[@for="NumberStyles.AllowLeadingSign"]/*' />
        AllowLeadingSign      = 0x00000004, //Can the number start with a sign char.
                                            //Specified by NumberFormatInfo.PositiveSign and NumberFormatInfo.NegativeSign
        //| <include path='docs/doc[@for="NumberStyles.AllowTrailingSign"]/*' />
        AllowTrailingSign     = 0x00000008, //Allow the number to end with a sign char
        //| <include path='docs/doc[@for="NumberStyles.AllowParentheses"]/*' />
        AllowParentheses      = 0x00000010, //Allow the number to be enclosed in parens
        //| <include path='docs/doc[@for="NumberStyles.AllowDecimalPoint"]/*' />
        AllowDecimalPoint     = 0x00000020, //Allow a decimal point
        //| <include path='docs/doc[@for="NumberStyles.AllowThousands"]/*' />
        AllowThousands        = 0x00000040, //Allow thousands separators (more properly, allow group separators)
        //| <include path='docs/doc[@for="NumberStyles.AllowExponent"]/*' />
        AllowExponent         = 0x00000080, //Allow an exponent
        //| <include path='docs/doc[@for="NumberStyles.AllowHexSpecifier"]/*' />
        AllowHexSpecifier     = 0x00000200, //Allow specifying hexadecimal.
        //Common uses.  These represent some of the most common combinations of these flags.

        //| <include path='docs/doc[@for="NumberStyles.Integer"]/*' />
        Integer  = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign,
        //| <include path='docs/doc[@for="NumberStyles.HexNumber"]/*' />
        HexNumber = AllowLeadingWhite | AllowTrailingWhite | AllowHexSpecifier,
        //| <include path='docs/doc[@for="NumberStyles.Number"]/*' />
        Number   = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowTrailingSign |
                   AllowDecimalPoint | AllowThousands,
        //| <include path='docs/doc[@for="NumberStyles.Float"]/*' />
        Float    = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign |
                   AllowDecimalPoint | AllowExponent,
        //| <include path='docs/doc[@for="NumberStyles.Any"]/*' />
        Any      = AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowTrailingSign |
                   AllowParentheses  | AllowDecimalPoint | AllowThousands | AllowExponent,

    }
}
