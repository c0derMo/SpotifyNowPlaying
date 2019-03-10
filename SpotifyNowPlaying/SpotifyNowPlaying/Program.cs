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
        static LoginForm lForm;

        static void Main(string[] args)
        {
            ImplictGrantAuth auth = new ImplictGrantAuth(clientID, "http://localhost:4002", "http://localhost:4002", SpotifyAPI.Web.Enums.Scope.UserReadPlaybackState);
            auth.AuthReceived += async (sender, payload) =>
            {
                //auth.Stop(); // `sender` is also the auth instance
                SpotifyWebAPI api = new SpotifyWebAPI() { TokenType = payload.TokenType, AccessToken = payload.AccessToken };
                //Console.WriteLine("New auth recieved.");
                // Do requests with API client
                //Console.WriteLine("Auth expires in: " + payload.ExpiresIn);
                PlaybackContext context = api.GetPlayback();
                //while(context.StatusCode() != HttpStatusCode.Unauthorized)
                //{
                    if (context.Item != null)
                    {
                        Console.Write("NOW PLAYING: ");
                        Console.WriteLine(context.Item.Name);
                    }
                    Thread.Sleep(10000);
                    context = api.GetPlayback();
                //}
                Console.WriteLine("While loop ended.");
                startAuthentification(auth);
            };
            auth.Start(); // Starts an internal HTTP Server

            //auth.OpenBrowser();
            startAuthentification(auth);

            while (true)
            {
            }
        }

        static void startAuthentification(ImplictGrantAuth auth)
        {
            Thread t = new Thread(new ParameterizedThreadStart(subLoginThread));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(auth);
        }

        static void subLoginThread(object auth)
        {
            ImplictGrantAuth aut2 = (ImplictGrantAuth)auth;
            if(lForm == null) { 
                lForm = new LoginForm();
            }
            lForm.chromeBrowser.Load(aut2.GetUri());
            lForm.ShowDialog();
        }
    }
}
