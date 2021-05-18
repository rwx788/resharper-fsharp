﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Diagnostics;

namespace JetBrains.ReSharper.Plugins.FSharp.Fantomas.Host
{
  public static class FantomasAssemblyResolver
  {
    private const string AdditionalProbingPathsEnvVar = "RIDER_PLUGIN_ADDITIONAL_PROBING_PATHS";
    private const string FantomasAssembliesPathEnvVar = "FSHARP_FANTOMAS_ASSEMBLIES_PATH";
    private static readonly List<string> OurAdditionalProbingPaths = new List<string>();

    static FantomasAssemblyResolver()
    {
      var fantomasPath = Environment.GetEnvironmentVariable(FantomasAssembliesPathEnvVar);
      var riderPaths = Environment.GetEnvironmentVariable(AdditionalProbingPathsEnvVar);
      Assertion.Assert(!string.IsNullOrWhiteSpace(fantomasPath), $"{FantomasAssembliesPathEnvVar} IsNullOrWhiteSpace");
      Assertion.Assert(!string.IsNullOrWhiteSpace(riderPaths), $"{AdditionalProbingPathsEnvVar} IsNullOrWhiteSpace");

      OurAdditionalProbingPaths.Add(fantomasPath);
      foreach (var path in riderPaths.Split(';'))
      {
        if (!string.IsNullOrEmpty(path)) OurAdditionalProbingPaths.Add(path);
      }
    }

    public static Assembly Resolve(object sender, ResolveEventArgs eventArgs)
    {
      var assemblyName = $"{new AssemblyName(eventArgs.Name).Name}.dll";

      foreach (var path in OurAdditionalProbingPaths)
      {
        var assemblyPath = Path.Combine(path, assemblyName);
        if (!File.Exists(assemblyPath)) continue;
        return Assembly.LoadFrom(assemblyPath);
      }

      Console.Error.Write($"\nFailed to resolve assembly by name '{eventArgs.Name}'" +
                          $"\n  Requesting assembly: {eventArgs.RequestingAssembly?.FullName}" +
                          $"\n  Probing paths: {string.Join("\n", OurAdditionalProbingPaths)}");
      return null;
    }
  }
}