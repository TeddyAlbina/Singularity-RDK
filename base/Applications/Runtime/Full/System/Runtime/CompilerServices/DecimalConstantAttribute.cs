// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
namespace System.Runtime.CompilerServices
{
    //| <include path='docs/doc[@for="DecimalConstantAttribute"]/*' />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited=false)]
    [CLSCompliant(false)]
    public sealed class DecimalConstantAttribute : Attribute
    {
        //| <include path='docs/doc[@for="DecimalConstantAttribute.DecimalConstantAttribute"]/*' />
        public DecimalConstantAttribute(
            byte scale,
            byte sign,
            uint hi,
            uint mid,
            uint low
        )
        {
            dec = new System.Decimal((int) low, (int)mid, (int)hi, (sign != 0), scale);
        }

        //| <include path='docs/doc[@for="DecimalConstantAttribute.Value"]/*' />
        public System.Decimal Value
        {
            get {
                return dec;
            }
        }

        private System.Decimal dec;
    }
}

