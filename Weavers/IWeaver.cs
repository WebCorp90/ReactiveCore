﻿using Mono.Cecil;
using System;

namespace MyWeaver.Fody
{
    public interface IWeaver
    {
         ModuleDefinition ModuleDefinition { get; set; }

        // Will log an MessageImportance.High message to MSBuild. 
        Action<string> LogInfo { get; set; }

        // Will log an error message to MSBuild. OPTIONAL
        Action<string> LogError { get; set; }

        Action<string> LogWarning { get; set; }

        Action<string> LogDebug { get; set; }
    }
}
