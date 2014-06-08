[<TechTalk.SpecFlow.Binding>]
module PublicServerSteps

open FsUnit
open System.Linq
open TechTalk.SpecFlow

type FakeServer = { Id: int; Address: string; Port: int }
type FakeServerList = { Public: seq<FakeServer> }

type Context() =
    let servers = { Public = Seq.empty<FakeServer> }

    member this.Servers =
        servers

    member this.Start() =
        ""

let client = Context()

let [<Given>] ``there are (\d+) public servers``(numberOfServers:int) =
    let createIpv4Address (id:int) =
        sprintf "%d.%d.%d.%d" (id &&& 0xff) ((id >>> 8) &&& 0xff) ((id >>> 16) &&& 0xff) ((id >>> 24) &&& 0xff)

    let createFakeServer (id:int) =
        { Id = id; Address = createIpv4Address id; Port = 7777 }

    client.Servers.Public.Concat (seq { for i in 1 .. numberOfServers -> createFakeServer i })

let [<When>] ``I start the client``() =
    client.Start

let [<Then>] ``I should see (\d+) servers``(numberOfServers:int) =
    client.Servers.Public |> should haveCount numberOfServers

let [<Given>] ``the reason is because I cannot connect to the master list``() =
    ScenarioContext.Current.Pending()
        

let [<Then>] ``I should see a message stating why``() =
    ScenarioContext.Current.Pending();
        