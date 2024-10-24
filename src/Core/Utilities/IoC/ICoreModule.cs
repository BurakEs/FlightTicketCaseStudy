﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Core.Utilities.IoC
{
    public interface ICoreModule 
    {
        void Load(IServiceCollection serviceCollection, IConfiguration configuration);

    }
}
