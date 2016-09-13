module Logging

open Serilog

let initialize() =
    Log.Logger <- LoggerConfiguration().ReadFrom.AppSettings().CreateLogger()
    Log.Logger.Information("Serilog logging initialised")
