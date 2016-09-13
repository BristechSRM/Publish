module LogExceptionHandler

open Serilog
open System
open System.Net
open System.Web.Http.ExceptionHandling

type LogExceptionHandler() =
  inherit ExceptionHandler()

  override __.Handle (context: ExceptionHandlerContext) =
    Log.Error("Exception: {0}", context.Exception)
    base.Handle(context)
