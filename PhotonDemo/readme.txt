
This is the readme for the Photon Client SDKs.
(C) Exit Games GmbH 2014



Reference Documentation
----------------------------------------------------------------------------------------------------
The API reference is included in this package as CHM file.
Find the current API reference, manuals, tutorials and more online:
http://doc.exitgames.com



Running the Demos
----------------------------------------------------------------------------------------------------
Our demos are built for the Photon Cloud to reduce the effort to run them. No server setup needed.
The service is free for development and signing up takes less than 5 minutes.

Every game title on the Cloud gets it's own AppId string, which must be copied into the clients. 
The demos use a property "AppId" in the source files. Set it's value before you build them.

Sign Up:                https://cloud.exitgames.com
App ID in Dashboard:    https://cloud.exitgames.com/dashboard

The Application ID is per application type (Realtime, Turnbased, Chat)!
You will find specific sections of the Dashboard per application type.


Alternatively you can host a "Photon Cloud" yourself and use any AppId. This is what you need to do:
- Download the Photon Server SDK (it comes as 7zip file or self-extracting exe).
- Unpack-
- Execute PhotonControl.exe.
- Click "Game Server IP Config" and pick a suitable IP address (local network or public).
- This IP must be used in your game as (master) server address. Don't forget the port after ":".
- Click "Loadbalancing (MyCloud)" and "Start as Application".

How to start the server:
http://doc.exitgames.com/en/onpremise/current/getting-started/photon-server-in-5min

Download the server SDK here:
https://www.exitgames.com/OnPremise/Download




Unity3d Notes
----------------------------------------------------------------------------------------------------
If you don't use Unity3D, skip this chapter.

Currently supported export platforms are: 
    Standalone (Windows, OSx and Linux)
    Web (Windows and MacOS)
    iOS (needs iOS Pro Unity license)
    Android (needs Android Pro Unity license)
    Windows 8 Store and Phone
    PS3, PS4 and XBox (certified developers should get in contact with us on demand)

As of v3.2.1.6 all Unity projects must use ExitGames.Client.Photon.Hashtable!
This provides compatibility with Win 8 RT and Win 8 Phone exports.
Add this to your code (at the beginning), to resolve the "ambiguous Hashtable" declaration:
using Hashtable = ExitGames.Client.Photon.Hashtable;

Web players do a policy-file request to port TCP 843 before they connect to a remote server. 
The Photon Cloud and Server SDK will handle these requests.
If you host the server, open the additional "policy request" port: TCP 843. If you configure
your server applications, run "Policy Application" for webplayers.


How to add Photon to your Unity project:
1) Copy the Photon .dll into the "Assets\Plugins" folder of your project. Avoid dll duplicates!
2) Make sure to have the following line of code somewhere in your scripts, to run it in background:
   Application.runInBackground = true; //without this Photon will loose connection if not focussed
3) Add "using Hashtable = ExitGames.Client.Photon.Hashtable;" to your scripts. Without quotation.
4) iOS build settings (Edit->Project Settings->Player) 
   "iPhone Stripping Level" to "Strip Bytecode" and use 
   "DotNet 2.0 subset".
   If your project runs fine in IDE but fails on device, check the "DotNet 2.0 Subset" option!
5) Change the server address in the client. A default of "localhost:5055" won't work on device.
6) Implement OnApplicationQuit() and disconnect the client when the app stops.



Windows Phone Notes
----------------------------------------------------------------------------------------------------
To run demos on a smartphone or simulator, you need to setup your server's network address or 
use the Photon Cloud. 

We assume you run a Photon Server on the same development machine, so by default, our demos have a 
address set to "127.0.0.1:5055" and use UDP.
Demos for LoadBalancing or the Cloud will be set to: "app.exitgamescloud.com:5055" and use UDP.

Search the code and replace those values to run the demos on your own servers or in a simulator.
The Peer.Connect() method is usually used to set the server's address.

A smartphone has it's own IP address, so 127.0.0.1 will contact the phone instead of the machine 
that also runs Photon.



Xamarin Notes
----------------------------------------------------------------------------------------------------
Edit MainActivity.cs with your own AppId (approx. line 33). Read "Running Loadbalancing Demos".
Alternatively change the ServerAddress to a Photon Server you are hosting. Don't forget the ":port".

Our library dlls are fully Xamarin.iOS, Xamarin.Android and Xamarin.mac compatible, 
but the projects to build them are not. So you can't reference the LoadBalancingApi project 
directly.
Instead, we reference the dlls to minimize the number of projects to maintain and potential for 
desaster.

The demo particle links directly to some of the source files of demo-particle-logic for similar 
reasons.

Let us know if our workflow doesn't work for you.




Playstation Mobile Notes
----------------------------------------------------------------------------------------------------
The demo by default connects to the EU Photon Cloud servers. You only need to register to run your 
own application (game) on it. This gives you the AppId which must be inserted into:
demo-particle-psm\AppMain.cs