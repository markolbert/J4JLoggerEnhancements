#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// SourcePathTrimmer.cs
//
// This file is part of JumpForJoy Software's J4JLoggerCommon.
// 
// J4JLoggerCommon is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLoggerCommon is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLoggerCommon. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Collections.ObjectModel;
using System.Reflection;

namespace J4JSoftware.Logging;

public class SourcePathTrimmer 
{
    private readonly List<string> _srcRootPaths = new();

    protected SourcePathTrimmer(
        StringComparison? fileSystemComparer = null
    )
    {
        FileSystemComparer = fileSystemComparer ?? Environment.OSVersion.Platform switch
        {
            PlatformID.MacOSX => StringComparison.Ordinal,
            PlatformID.Unix => StringComparison.Ordinal,
            PlatformID.Other => StringComparison.Ordinal,
            _ => StringComparison.OrdinalIgnoreCase
        };
    }

    public StringComparison FileSystemComparer { get; }
    public ReadOnlyCollection<string> SourceRootPaths => _srcRootPaths.AsReadOnly();

    public void AddAssembly(Assembly assembly) => AddSourceCodeRootPath(assembly);

    public void AddAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddSourceCodeRootPath(assembly);
        }
    }

    public void AddAssemblies(IEnumerable<Assembly> assemblies) => AddAssemblies(assemblies.ToArray());

    private void AddSourceCodeRootPath(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<SourceCodeRootPathAttribute>() is not { } attr)
            return;

        if (string.IsNullOrEmpty(attr.RootPath))
            return;

        // the wrapping call is needed to deal with / vs \ in Windows paths
        var path = Path.GetFullPath(attr.RootPath);
        if (!Path.EndsInDirectorySeparator(path))
            path = $"{path}{Path.DirectorySeparatorChar}";

        _srcRootPaths.Add(path);
    }

    public void AddAssemblyFromType(Type type) => AddSourceCodeRootPath(type.Assembly);

    public void AddAssembliesFromTypes(params Type[] types)
    {
        foreach (var type in types)
        {
            AddAssemblyFromType(type);
        }
    }

    public void AddAssembliesFromTypes(IEnumerable<Type> types) => AddAssembliesFromTypes(types.ToArray());
}