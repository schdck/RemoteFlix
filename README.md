<div align="center">
  
  [<img width=128 height=128 src="https://github.com/schdck/RemoteFlix/blob/master/Other/icon.png?raw=true">](https://github.com/schdck/RemoteFlix)

  # RemoteFlix  (beta)
  
 [![Build status](https://ci.appveyor.com/api/projects/status/5ue8gqox9da69e0w/branch/master?svg=true)](https://ci.appveyor.com/project/schdck/remoteflix/branch/master)
 [![GitHub issues](https://img.shields.io/github/issues/schdck/RemoteFlix.svg)](https://github.com/schdck/RemoteFlix/issues)
 [![License](https://img.shields.io/github/license/schdck/RemoteFlix.svg)](https://github.com/schdck/RemoteFlix/blob/master/LICENSE)
 
  :cinema: RemoteFlix is a desktop application aimed to allow you to control any <br> media player running on your computer directly from a web-browser in your phone.
</div>

<hr>

## Why?
Well, basically because I was tired of getting up to pause my stream and answer a text.

## How?
The idea behind RemoteFlix is very simple. Most (if not all) media players provide shortcuts that enable you to perform a task (e.g. pausing the video). What RemoteFlix do is allow you to execute these (pre-defined) shortcuts from your phone's browser.

To do that, it exposes a very simple web-server which is responsible for listing the avaliable players and actions (which we call commands). Also, the web-server is responsible for handling requests. When a request is received, it'll look for the player's process on the machine and send the keys corresponding to the action's shortcut to it.

## Installing and running
As of this moment, you'll have to build and run the application yourself. When I feel confortable to release the application, I'll provide an installer which offers the possibility to auto-start the application and to automatically open the needed firewall ports.

After you run the executable, it's pretty straightforward. Just go ahead and scan the QRCode or navigate to the indicated URL.

[<img src="https://github.com/schdck/RemoteFlix/blob/master/Other/Screenshots/MainScreen.png?raw=true">](https://raw.githubusercontent.com/schdck/RemoteFlix/master/Other/Screenshots/MainScreen.png)

## Troubleshooting
If you are running RemoteFlix but can't connect to it, you can try these steps:

* Try to connect to it from a browser running on your computer (this will help narrowing down the issue)
* Make sure that you are connected to the same network on both your computer and your phone
* Try restarting the application if you changed the network your computer is connected to
* Make sure that your firewall is not in the way (the server is using TCP port 50505 by default)
