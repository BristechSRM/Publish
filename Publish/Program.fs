open Microsoft.Owin.Hosting
open System
open System.Configuration
open System.Threading
open Serilog
open Startup
open Config

(*
    Do not run Visual Studio as Administrator!

    Open a command prompt as Administrator and run the following command, replacing username with your username, and the port number that you have selected in config
    netsh http add urlacl url=http://*:8080/ user=username
*)
[<EntryPoint>]
let main _ = 
    JsonSettings.setDefaults()

    use server = WebApp.Start<Startup>(baseUrl)
    Log.Information("Listening on {0}", baseUrl)

    let waitIndefinitelyWithToken = 
        let cancelSource = new CancellationTokenSource()
        cancelSource.Token.WaitHandle.WaitOne() |> ignore
    0
