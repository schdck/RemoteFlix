using RemoteFlix.Base.Classes;
using RemoteFlix.Base.Enums;
using RemoteFlix.Base.Helpers;
using RemoteFlix.Base.Players;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static RemoteFlix.Base.Helpers.ResponseHelper;

namespace RemoteFlix.Base
{
    public sealed class RemoteFlixServer
    {
        private const int HTTP_OK = 200;
        private const int HTTP_BAD_REQUEST = 400;
        private const int HTTP_NOT_FOUND = 404;
        private const int HTTP_INTERNAL_SERVER_ERROR = 500;
        public const ushort PORT = 50505;
        private HttpListener Listener;
        private CancellationTokenSource CancellationTokenSource;

        private int RequestId;
        public bool IsRunning { get; private set; }

        // We don't need this to be lazy, since we start the
        // the server when the program starts
        public static RemoteFlixServer Instance { get; } = new RemoteFlixServer();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static RemoteFlixServer()
        {
        }

        private RemoteFlixServer()
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Windows XP SP2 or Server 2003 is required to use the HttpListener class");
            }

            Listener = new HttpListener();
            Listener.Prefixes.Add($"http://+:{PORT}/");

            IsRunning = false;
        }

        public void Start()
        {
            CancellationTokenSource = new CancellationTokenSource();            

            Logger.Instance.Log(LogLevel.Message, "Starting the HttpListener server...");

            Listener.Start();

            Logger.Instance.Log(LogLevel.Message, "Server started.");

            Task.Factory.StartNew(ReceiveRequests, CancellationTokenSource.Token);

            IsRunning = true;
        }

        public void Stop()
        {
            Listener.Stop();

            CancellationTokenSource.Cancel();

            IsRunning = false;
        }

        private void ReceiveRequests()
        {
            Logger.Instance.Log(LogLevel.Message, "Listening for requests...");

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                ThreadPool.QueueUserWorkItem(ProcessRequest, Listener.GetContext());
            }

            Logger.Instance.Log(LogLevel.Message, "Stopped listening for requests.");
        }

        private void ProcessRequest(object state)
        {
            var requestId = Interlocked.Increment(ref RequestId);
            var context = state as HttpListenerContext;
            var request = context.Request;
            var resourceName = request.RawUrl.Remove(0, 1);

            Logger.Instance.Log(LogLevel.Message, $"Received {request.HttpMethod} request on '{request.RawUrl}' (ID {requestId})");

            if (request.HttpMethod == "GET")
            {
                if(string.IsNullOrEmpty(resourceName))
                {
                    HandleGetForHome(context);
                }
                else
                {
                    var assembly = Assembly.GetExecutingAssembly();

                    using(var stream = assembly.GetManifestResourceStream($"RemoteFlix.Base.Resources.{resourceName}"))
                    {
                        if (stream != null)
                        {
                            HandleGetForResource(context, stream);
                        }
                        else
                        {
                            HandleGetForPlayer(context, resourceName);
                        }
                    }     
                }
            }
            else if(request.HttpMethod == "POST")
            {
                HandlePost(context, request);
            }

            Logger.Instance.Log(LogLevel.Message, $"Finished serving request ID {requestId}.");
        }

        private void HandleGetForHome(HttpListenerContext context)
        {
            var builder = new StringBuilder();


            foreach (var player in RemoteFlixPlayers.AvaliablePlayers)
            {
                // $"http://{NetworkHelper.GetLocalIPAddress()}:{RemoteFlixServer.PORT}"
                builder.AppendLine($"<button onclick=\"location.href='http://{NetworkHelper.GetLocalIPAddress()}:{PORT}/{player.Id}'\" type='button'>{player.Name}</button>");
            }

            SendResponse(context, builder.ToString());
        }

        private void HandleGetForResource(HttpListenerContext context, Stream stream)
        {
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, buffer.Length);

            SendResponse(context, buffer);
        }

        private void HandleGetForPlayer(HttpListenerContext context, string playerName)
        {
            var player = RemoteFlixPlayers.AvaliablePlayers.FirstOrDefault(x => x.Id == playerName);

            if (player == null)
            {
                Logger.Instance.Log(LogLevel.Warning, $"Responded with 404 (NOT_FOUND) for resource/player '{playerName}'");
                SendEmptyResponse(context, HTTP_NOT_FOUND);
                return;
            }

            var builder = new StringBuilder();

            foreach (var command in player.Commands)
            {
                builder.AppendLine($"<button onclick=\"postCommand('{player.Id}', '{command.Id}')\">{command.ActionName}</button>");
            }

            SendResponse(context, builder.ToString());
        }

        private void HandlePost(HttpListenerContext context, HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Logger.Instance.Log(LogLevel.Error, $"Responded with 400 (BAD_REQUEST) because the POST has no EntityBody");
                SendEmptyResponse(context, HTTP_BAD_REQUEST);
                return;
            }

            var playerName = request.RawUrl.Remove(0, 1);
            var player = RemoteFlixPlayers.AvaliablePlayers.FirstOrDefault(x => x.Id == playerName);

            if (player == null)
            {
                SendEmptyResponse(context, HTTP_NOT_FOUND);
                return;
            }

            using (var stream = request.InputStream)
            using (var reader = new StreamReader(stream, request.ContentEncoding))
            {
                var data = reader.ReadToEnd().Split('&');
                var commandId = data.FirstOrDefault(x => x.StartsWith("command"))?.Remove(0, 8);
                var command = player.Commands.FirstOrDefault(x => x.Id == commandId);

                if (commandId == null)
                {
                    Logger.Instance.Log(LogLevel.Error, $"Responded with 400 (BAD_REQUEST) because the command was not specified");
                    SendEmptyResponse(context, HTTP_BAD_REQUEST);
                    return;
                }
                else if(command == null)
                {
                    Logger.Instance.Log(LogLevel.Error, $"Responded with 400 (BAD_REQUEST) because the command '{commandId}' was not found");
                    SendEmptyResponse(context, HTTP_BAD_REQUEST);
                    return;
                }

                if(ExecuteCommand(player, command))
                {
                    SendEmptyResponse(context, HTTP_OK);
                }
                else
                {
                    Logger.Instance.Log(LogLevel.Error, $"Responded with 500 (INTERNAL_SERVER_ERROR) because a handle was not found to send the keys to");
                    SendEmptyResponse(context, HTTP_INTERNAL_SERVER_ERROR);
                }
            }
        }

        private bool ExecuteCommand(BasePlayer player, PlayerCommand command)
        {
            // Try to get the player's specific handle
            IntPtr? playerHandle = player.GetHandle();

            // In case of fail, try to get the foreground window
            if (playerHandle == null)
            {
                playerHandle = KeyboardSimulationHelper.GetForegroundWindow();

                // If this also fails, there isn't much we can do
                if (playerHandle.Value == IntPtr.Zero)
                {
                    return false;
                }
            }

            KeyboardSimulationHelper.SendKeys(playerHandle.Value, command.ActionShortcut);

            return true;
        }
    }
}
