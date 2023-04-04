# J4JLoggerCommon

A library containing common routines supporting the [J4JLoggerEnhancements](../../README.md) system.

Conforms to NET 7.

Nullability is enabled.

The libraries are licensed under the GNU GPL-v3 or later. For more details see the [license file](../../LICENSE.md).

See the [change log](changes.md) for a history of significant changes.

## Source Code Path Trimming

`SourcePathTrimmer` and `SourceCodeRootPathAttribute` together create a way to trim source code file paths so they are relative to the project root.

For trimming to take place you must add a `SourceCodeRootPathAttribute` to an `AssemblyAttribute` in the project's **.csproj** file:

```xml
<ItemGroup>
    <AssemblyAttribute Include="J4JSoftware.Logging.SourceCodeRootPathAttribute">
        <_Parameter1>c:/Programming/J4JLoggerEnhancements/SerilogTests</_Parameter1>
    </AssemblyAttribute>
</ItemGroup>
```

Then, you call the `SourceCode()` extension method for the `ILogger` to which you are logging. There are separate `SourceCode()` extensions defined for both the Microsoft and Serilog loggers.

`SourceCode()` also accepts a boolean `include` parameter. When true, the default, source code information is included in the log event. If set to false, source code information is not included. This is to support only including source code information when debugging code (i.e., by defining a variable that is true during debugging and false during production).
