using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Auth;
using System.Net;
using System.Threading;

namespace SpotifyNowPlaying
{
    class Program
    {
        static string clientID = "43ed5ea454214b7d9aadc4e35ec3ebb9";
        static NowPlayingInterface npi;

        static void Main(string[] args)
        {
            ImplictGrantAuth auth = new ImplictGrantAuth(clientID, "http://localhost:4002", "http://localhost:4002", SpotifyAPI.Web.Enums.Scope.UserReadPlaybackState);
            auth.AuthReceived += async (sender, payload) =>
            {
                SpotifyWebAPI api = new SpotifyWebAPI() { TokenType = payload.TokenType, AccessToken = payload.AccessToken };

                PlaybackContext context = api.GetPlayback();
                while(context.StatusCode() != HttpStatusCode.Unauthorized)
                {
                    if (context.Item != null)
                    {
                        if(npi != null)
                        {
                            npi.updateInterface(context.Item.Name, context.Item.Artists[0].Name, context.Item.Album.Images[0].Url);
                        }
                        Console.Write("NOW PLAYING: ");
                        Console.WriteLine(context.Item.Name);
                        Console.Write("FROM: ");
                        Console.WriteLine(context.Item.Artists[0].Name);
                        Console.Write("IMAGE PIC: ");
                        Console.WriteLine(context.Item.Album.Images[0].Url);
                    }
                    Thread.Sleep(10000);
                    context = api.GetPlayback();
                }
                Console.WriteLine("While loop ended.");
                startAuthentification(auth);
            };
            auth.Start();
            
            startAuthentificationWindow(auth);

            npi = new NowPlayingInterface();
            npi.ShowDialog();

            //while (true) { }
        }

        static void startAuthentification(ImplictGrantAuth auth)
        {
            CefSharp.OffScreen.ChromiumWebBrowser browser = new CefSharp.OffScreen.ChromiumWebBrowser(auth.GetUri());
        }

        static void startAuthentificationWindow(ImplictGrantAuth auth)
        {
            Thread t = new Thread(new ParameterizedThreadStart(subLoginThread));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(auth);
        }

        static void subLoginThread(object auth)
        {
            ImplictGrantAuth aut2 = (ImplictGrantAuth)auth;
            new LoginForm(aut2.GetUri()).ShowDialog();
        }
    }
}
