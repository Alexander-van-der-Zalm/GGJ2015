
This is the readme for the Photon Chat Api.
(C) Exit Games GmbH 2014



Documentation
----------------------------------------------------------------------------------------------------
    http://doc.exitgames.com/en/chat
    http://forum.exitgames.com



Implementing Photon Chat
----------------------------------------------------------------------------------------------------
    Photon Chat is separated from other Photon Applications, so it needs it's own AppId.
    Our demos usually don't have an AppId set. 
    In code, find "<your appid>" to copy yours in. In Unity, we usually use a component
    to set the AppId via the Inspector. Look for the "Scripts" GameObject in the scenes.

    Register your Chat Application in the Dashboard:
    https://www.exitgames.com/en/Chat/Dashboard
    
    The class ChatClient and the interface IChatClientListener wrap up most important parts
    of the Chat API. Please refer to their documentation on how to use this API. 
    More documentation will follow.
    
    If you use Photon Realtime or PUN, you can copy the source from the ChatApi folder
    into your project. It should run in most cases (unless your Photon assembly is very old).



Content
----------------------------------------------------------------------------------------------------
    The zip contains a pure DotNet console demo and a Unity 4.3 project with a basic GUI.
    The demos don't contain any "game" code. They are just about the Chat API.

    See the ChatClient for some documentation, if your IDE does not show the code comments (like MD).
    The ChatClient is also what is used to subscribe to channels, etc. It can run parallel to PUN or 
    a LoadBalancingClient and represents an independent connection.
    
