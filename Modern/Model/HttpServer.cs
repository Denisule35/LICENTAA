using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Modern.ViewModel;
using Newtonsoft.Json;

namespace Modern.Model
{
    public class HttpServer
    {
        private static HttpListener _listener;
        private MainWindowViewModel _mainwindowviewmodel;
        private bool _isRunning;

        private static HashSet<string> _lockedClients = new HashSet<string>();

        public HttpServer(HttpListener listener, MainWindowViewModel mainwindowviewmodel)
        {
            _listener = listener;
            _mainwindowviewmodel = mainwindowviewmodel;
        }

        private async Task HandleRequest()
        {
            while (_isRunning)
            {
                HttpListenerContext context = null;
                try
                {
                    context = await _listener.GetContextAsync();

                    context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    context.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                    if (context.Request.HttpMethod == "OPTIONS")
                    {
                        context.Response.StatusCode = 200;
                        context.Response.Close();
                        continue;
                    }

                    if (context.Request.HttpMethod == "POST")
                    {
                        Console.WriteLine("POST request received");

                        using (StreamReader reader = new StreamReader(context.Request.InputStream))
                        {
                            string body = await reader.ReadToEndAsync();

                            dynamic data = JsonConvert.DeserializeObject(body);
                            string receivedName = data?.name;
                            string deviceId = data?.deviceId;

                            
                            string clientKey = !string.IsNullOrEmpty(deviceId)
                                ? deviceId
                                : context.Request.RemoteEndPoint.Address.ToString();

                            Console.WriteLine($"ClientKey: {clientKey}");

                            
                            if (_lockedClients.Contains(clientKey))
                            {
                                await SendResponse(context, "Access locked. You have already validated someone.", 403);
                                context.Response.Close();
                                continue;
                            }

                            using (Bazadateconnect bz = new Bazadateconnect())
                            {
                                var people = bz.Oameni.ToList();
                                var prezenta = bz.Prezenti.ToList();

                                var person = people.FirstOrDefault(om =>
                                    om.Name.Equals(receivedName, StringComparison.InvariantCultureIgnoreCase));
                                var prez = prezenta.FirstOrDefault(p => p.Data == DateOnly.FromDateTime(DateTime.Today));

                                if (person != null)
                                {
                                    if (DateOnly.FromDateTime(DateTime.Today) > person.Abonament.AddMonths(1))
                                    {
                                        await SendResponse(context, $"Hello {receivedName}! Invalid Subscription", 200);
                                    }
                                    else
                                    {
                                        // Valid — lock this device
                                        _lockedClients.Add(clientKey);
                                        await SendResponse(context, $"Hello {receivedName}! Valid Subscription", 200);

                                        foreach (var absent in _mainwindowviewmodel._absenti)
                                        {
                                            if (absent == receivedName)
                                            {
                                                _mainwindowviewmodel._absenti.Remove(absent);
                                                break;
                                            }
                                        }

                                        prez.Absenti = string.Empty;

                                        foreach (var absent in _mainwindowviewmodel._absenti)
                                        {
                                            prez.Absenti += absent + "\n";
                                        }

                                        prez.Prezenti += receivedName + "\n";
                                        bz.SaveChanges();

                                    }
                                }
                                else
                                {
                                    await SendResponse(context, "Client doesn't exist", 200);
                                }
                            }
                        }
                    }

                    context.Response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    context?.Response.Close();
                }
            }
        }

        private async Task SendResponse(HttpListenerContext context, string message, int statusCode)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.StatusCode = statusCode;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        public void UnlockClient(string deviceId)
        {
            _lockedClients.Remove(deviceId);
        }

        public void UnlockAll()
        {
            _lockedClients.Clear();
        }

        public void Start()
        {
            _listener.Prefixes.Add("http://localhost:8000/server/");
            _listener.Start();
            _isRunning = true;
            Task.Run(() => HandleRequest());
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }
    }
}
