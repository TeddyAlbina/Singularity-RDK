// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
using System;

namespace System.Diagnostics
{
    //| <include path='docs/doc[@for="ConditionalAttribute"]/*' />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public sealed class ConditionalAttribute : Attribute
    {
        //| <include path='docs/doc[@for="ConditionalAttribute.ConditionalAttribute"]/*' />
        public ConditionalAttribute(String conditionString)
        {
            m_conditionString = conditionString;
        }

        //| <include path='docs/doc[@for="ConditionalAttribute.ConditionString"]/*' />
        public String ConditionString {
            get {
                return m_conditionString;
            }
        }

        private String m_conditionString;
    }
}
