--resource_manifest_version '44febabe-d386-4d18-afbe-5e627f4af937'
resource_manifest_version '77731fab-63ca-442c-a67b-abc70f28dfa5'

description 'TTT server'
author 'Appi'
version 'v0.1'
resource_type 'gametype' { name = 'Trouble in Terrorist Town' }

client_scripts {
    'NativeUI.dll',
    'ClientData.net.dll',
    'Client.net.dll',
    'FingerPoint.lua'
}

server_scripts {
    'System.Configuration.Install.dll',
    'Newtonsoft.Json.dll',
    'MySql.Data.dll',
    'ServerData.net.dll',
    'Server.net.dll',
}