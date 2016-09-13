module JsonHttpClient

open Newtonsoft.Json
open System
open System.Net
open System.Net.Http
open RestModels
open System.Text
open Serilog

let get<'Model> (uri : Uri) = 
    use client = new HttpClient()
    use response = client.GetAsync(uri).Result
    let modelName = typeof<'Model>.Name
    match response.StatusCode with
    | HttpStatusCode.OK -> 
        let json = response.Content.ReadAsStringAsync().Result
        Log.Information("Endpoint for " + modelName + " found.")
        JsonConvert.DeserializeObject<'Model>(json)
    | errorCode -> 
        let errorResponse = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in get request for %s. Status code: %i. Reason phrase: %s. Error Message: %s" modelName (int (errorCode)) response.ReasonPhrase errorResponse
        Log.Error(message)
        failwith message

let patch (uri : Uri) recordId (op : PatchOp) = 
    use client = new HttpClient()
    let targetUri = new Uri(uri, recordId.ToString())
    use content = new StringContent(JsonConvert.SerializeObject(op), Encoding.UTF8, "application/json")
    use message = new HttpRequestMessage(new HttpMethod("PATCH"), targetUri, Content = content)

    use response = client.SendAsync(message).Result
    match response.StatusCode with 
    | HttpStatusCode.NoContent -> ()
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in patch request for to uri: %A. Status code: %i. Reason phrase: %s. Error Message: %s" targetUri (int (errorCode)) response.ReasonPhrase errorMessage
        Log.Error(message)
        failwith message