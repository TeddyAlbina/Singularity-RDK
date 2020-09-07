//------------------------------------------------------------------------------
// <copyright file="SymbolEqualComparer.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace System.Web.Util
{
    using System.Collections;
    using System.Globalization;

    //| <include file='doc\SymbolEqualComparer.uex' path='docs/doc[@for="SymbolEqualComparer"]/*' />
    /// <devdoc>
    ///  <para>
    ///    For internal use only. This implements a comparison that only
    ///    checks for equality, so this should only be used in un-sorted data
    ///    structures like Hashtable and ListDictionary. This is a little faster
    ///    than using CaseInsensitiveComparer because it does a strict
    ///    character-by-character equality check rather than a sorted comparison.
    ///  </para>
    /// </devdoc>
    internal class SymbolEqualComparer: IComparer {

        //| <include file='doc\SymbolEqualComparer.uex' path='docs/doc[@for="SymbolEqualComparer.Default"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        internal static readonly IComparer Default = new SymbolEqualComparer();

        internal SymbolEqualComparer() {
        }

        int IComparer.Compare(object keyLeft, object keyRight) {

            string sLeft = keyLeft as string;
            string sRight = keyRight as string;
            if (sLeft == null) {
                throw new ArgumentNullException("keyLeft");
            }
            if (sRight == null) {
                throw new ArgumentNullException("keyRight");
            }
            int lLeft = sLeft.Length;
            int lRight = sRight.Length;
            if (lLeft != lRight) {
                return 1;
            }
            for (int i = 0; i < lLeft; i++) {
                char charLeft = sLeft[i];
                char charRight = sRight[i];
                if (charLeft == charRight) {
                    continue;
                }
                UnicodeCategory catLeft = Char.GetUnicodeCategory(charLeft);
                UnicodeCategory catRight = Char.GetUnicodeCategory(charRight);
                if (catLeft == UnicodeCategory.UppercaseLetter
                    && catRight == UnicodeCategory.LowercaseLetter) {
                    if (Char.ToLower(charLeft) == charRight) {
                        continue;
                    }
                }
                else if (catRight == UnicodeCategory.UppercaseLetter
                    && catLeft == UnicodeCategory.LowercaseLetter){
                    if (Char.ToLower(charRight) == charLeft) {
                        continue;
                    }
                }
                return 1;
            }
            return 0;
        }
    }
}
