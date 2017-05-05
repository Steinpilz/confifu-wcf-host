// include Fake lib
#r @"..\packages\FAKE\tools\FakeLib.dll"
#r @"..\packages\Steinpilz.DevFlow.Fake\lib\net451\Steinpilz.DevFlow.Fake.dll"

open Fake
open Steinpilz.DevFlow.Fake 

Lib.setup(fun p -> 
    { p with 
        PublishProjects = !!"src/app/**/*.csproj"
        UseDotNetCliToPack = true
        NuGetFeed = 
            { p.NuGetFeed with 
                ApiKey = environVarOrFail <| "NUGET_API_KEY" |> Some
            }
    }
)
