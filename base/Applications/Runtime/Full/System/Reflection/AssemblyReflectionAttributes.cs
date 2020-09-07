// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
//=============================================================================
//
// Purpose: For Assembly-related reflection custom attributes.
//
//=============================================================================  

namespace System.Reflection
{

    using System;

    //| <include path='docs/doc[@for="AssemblyCopyrightAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyCopyrightAttribute : Attribute
    {
        private String m_copyright;

        //| <include path='docs/doc[@for="AssemblyCopyrightAttribute.AssemblyCopyrightAttribute"]/*' />
        public AssemblyCopyrightAttribute(String copyright)
        {
            m_copyright = copyright;
        }

        //| <include path='docs/doc[@for="AssemblyCopyrightAttribute.Copyright"]/*' />
        public String Copyright
        {
            get { return m_copyright; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyTrademarkAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyTrademarkAttribute : Attribute
    {
        private String m_trademark;

        //| <include path='docs/doc[@for="AssemblyTrademarkAttribute.AssemblyTrademarkAttribute"]/*' />
        public AssemblyTrademarkAttribute(String trademark)
        {
            m_trademark = trademark;
        }

        //| <include path='docs/doc[@for="AssemblyTrademarkAttribute.Trademark"]/*' />
        public String Trademark
        {
            get { return m_trademark; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyProductAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyProductAttribute : Attribute
    {
        private String m_product;

        //| <include path='docs/doc[@for="AssemblyProductAttribute.AssemblyProductAttribute"]/*' />
        public AssemblyProductAttribute(String product)
        {
            m_product = product;
        }

        //| <include path='docs/doc[@for="AssemblyProductAttribute.Product"]/*' />
        public String Product
        {
            get { return m_product; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyCompanyAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyCompanyAttribute : Attribute
    {
        private String m_company;

        //| <include path='docs/doc[@for="AssemblyCompanyAttribute.AssemblyCompanyAttribute"]/*' />
        public AssemblyCompanyAttribute(String company)
        {
            m_company = company;
        }

        //| <include path='docs/doc[@for="AssemblyCompanyAttribute.Company"]/*' />
        public String Company
        {
            get { return m_company; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyDescriptionAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyDescriptionAttribute : Attribute
    {
        private String m_description;

        //| <include path='docs/doc[@for="AssemblyDescriptionAttribute.AssemblyDescriptionAttribute"]/*' />
        public AssemblyDescriptionAttribute(String description)
        {
            m_description = description;
        }

        //| <include path='docs/doc[@for="AssemblyDescriptionAttribute.Description"]/*' />
        public String Description
        {
            get { return m_description; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyTitleAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyTitleAttribute : Attribute
    {
        private String m_title;

        //| <include path='docs/doc[@for="AssemblyTitleAttribute.AssemblyTitleAttribute"]/*' />
        public AssemblyTitleAttribute(String title)
        {
            m_title = title;
        }

        //| <include path='docs/doc[@for="AssemblyTitleAttribute.Title"]/*' />
        public String Title
        {
            get { return m_title; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyConfigurationAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyConfigurationAttribute : Attribute
    {
        private String m_configuration;

        //| <include path='docs/doc[@for="AssemblyConfigurationAttribute.AssemblyConfigurationAttribute"]/*' />
        public AssemblyConfigurationAttribute(String configuration)
        {
            m_configuration = configuration;
        }

        //| <include path='docs/doc[@for="AssemblyConfigurationAttribute.Configuration"]/*' />
        public String Configuration
        {
            get { return m_configuration; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyDefaultAliasAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyDefaultAliasAttribute : Attribute
    {
        private String m_defaultAlias;

        //| <include path='docs/doc[@for="AssemblyDefaultAliasAttribute.AssemblyDefaultAliasAttribute"]/*' />
        public AssemblyDefaultAliasAttribute(String defaultAlias)
        {
            m_defaultAlias = defaultAlias;
        }

        //| <include path='docs/doc[@for="AssemblyDefaultAliasAttribute.DefaultAlias"]/*' />
        public String DefaultAlias
        {
            get { return m_defaultAlias; }
        }
    }

    //| <include path='docs/doc[@for="AssemblyInformationalVersionAttribute"]/*' />
    [AttributeUsage (AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class AssemblyInformationalVersionAttribute : Attribute
    {
        private String m_informationalVersion;

        //| <include path='docs/doc[@for="AssemblyInformationalVersionAttribute.AssemblyInformationalVersionAttribute"]/*' />
        public AssemblyInformationalVersionAttribute(String informationalVersion)
        {
            m_informationalVersion = informationalVersion;
        }

        //| <include path='docs/doc[@for="AssemblyInformationalVersionAttribute.InformationalVersion"]/*' />
        public String InformationalVersion
        {
            get { return m_informationalVersion; }
        }
    }
}
