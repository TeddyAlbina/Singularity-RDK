﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuBuild
{
    class SourceConfigurationError : Exception
    {
        public SourceConfigurationError(string msg)
            : base(msg)
        { }
    }
}
