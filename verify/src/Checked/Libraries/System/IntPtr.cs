// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
//============================================================
//
// Class:  IntPtr
//
// Purpose: Platform neutral integer
//
//===========================================================  

namespace System
{

    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    //| <include path='docs/doc[@for="IntPtr"]/*' />
    [NoCCtor]
    public struct IntPtr
    {
        private uint m_value; // The compiler treats void* closest to uint hence explicit casts are required to preserve int behavior

        //| <include path='docs/doc[@for="IntPtr.Zero"]/*' />
        [Intrinsic]
        public static readonly IntPtr Zero;

        //| <include path='docs/doc[@for="IntPtr.Size"]/*' />
        [Intrinsic]
        public static readonly int Size;

        //| <include path='docs/doc[@for="IntPtr.IntPtr"]/*' />
        [Intrinsic]
        public extern IntPtr(int value);

        //| <include path='docs/doc[@for="IntPtr.IntPtr1"]/*' />
        [Intrinsic]
        public extern IntPtr(long value);

        //| <include path='docs/doc[@for="IntPtr.Equals"]/*' />
        public override bool Equals(Object obj)
        {
            if (obj is IntPtr) {
                return (m_value == ((IntPtr)obj).m_value);
            }
            return false;
        }

        //| <include path='docs/doc[@for="IntPtr.GetHashCode"]/*' />
        public override int GetHashCode()
        {
            return (int)m_value;
        }

        //| <include path='docs/doc[@for="IntPtr.ToInt32"]/*' />
        [Intrinsic]
        public extern int ToInt32();

        //| <include path='docs/doc[@for="IntPtr.ToInt64"]/*' />
        [Intrinsic]
        public extern long ToInt64();

        //| <include path='docs/doc[@for="IntPtr.ToString"]/*' />
        [Inline]
        public override String ToString()
        {
            return this.ToInt32().ToString();
        }

        [CLSCompliant(false)]
        [Intrinsic]
        public extern static explicit operator UIntPtr (IntPtr value);

        //| <include path='docs/doc[@for="IntPtr.operatorIntPtr"]/*' />
        [Intrinsic]
        public extern static explicit operator IntPtr (int value);

        //| <include path='docs/doc[@for="IntPtr.operatorIntPtr1"]/*' />
        [Intrinsic]
        public extern static explicit operator IntPtr (long value);

        //| <include path='docs/doc[@for="IntPtr.operatorint"]/*' />
        [Intrinsic]
        public extern static explicit operator int (IntPtr value);

        //| <include path='docs/doc[@for="IntPtr.operatorlong"]/*' />
        [Intrinsic]
        public extern static explicit operator long (IntPtr value);

        //| <include path='docs/doc[@for="IntPtr.operatorEQ"]/*' />
        [Intrinsic]
        public extern static bool operator == (IntPtr value1, IntPtr value2);

        //| <include path='docs/doc[@for="IntPtr.operatorNE"]/*' />
        [Intrinsic]
        public extern static bool operator != (IntPtr value1, IntPtr value2);

        [Intrinsic]
        public extern static IntPtr operator - (IntPtr value1, IntPtr value2);

        [Intrinsic]
        public extern static IntPtr operator - (IntPtr value1, int value2);

        [Intrinsic]
        public extern static IntPtr operator -- (IntPtr value);

        [Intrinsic]
        public extern static IntPtr operator + (IntPtr value1, IntPtr value2);

        [Intrinsic]
        public extern static IntPtr operator ++ (IntPtr value);

        [Intrinsic]
        public extern static bool operator < (IntPtr value1, IntPtr value2);

        [Intrinsic]
        public extern static bool operator > (IntPtr value1, IntPtr value2);

        [Intrinsic]
        public extern static bool operator <= (IntPtr value1, IntPtr value2);

        [Intrinsic]
        public extern static bool operator >= (IntPtr value1, IntPtr value2);
    }
}
