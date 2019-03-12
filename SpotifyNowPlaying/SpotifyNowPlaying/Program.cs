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
        public static NowPlayingInterface npi;
        public static ImplictGrantAuth auth;
        public static bool logout = false;

        static void Main(string[] args)
        {
            auth = new ImplictGrantAuth(clientID, "http://localhost:4002", "http://localhost:4002", SpotifyAPI.Web.Enums.Scope.UserReadPlaybackState);
            auth.AuthReceived += async (sender, payload) =>
            {
                SpotifyWebAPI api = new SpotifyWebAPI() { TokenType = payload.TokenType, AccessToken = payload.AccessToken };

                PlaybackContext context = api.GetPlayback();
                while(context.StatusCode() != HttpStatusCode.Unauthorized && !logout)
                {
                    if (context.Item != null)
                    {
                        String artists = "";
                        foreach(SimpleArtist artist in context.Item.Artists)
                        {
                            artists += artist.Name + ", ";
                        }
                        artists = artists.Substring(0, artists.Length - 2);
                        String image = "";
                        if (context.Item.Album.Images.Count > 0) { 
                            image = context.Item.Album.Images[0].Url;
                        } else
                        {
                            image = "https://cdn1.iconfinder.com/data/icons/ui-glynh-05-of-5/100/UI_Glyph_09_-14-512.png";
                        }
                        if (npi != null)
                        {
                            npi.updateInterface(context.Item.Name, artists, image);
                        }
                        if (OptionManager.outputToTextFile)
                        {
                            if(OptionManager.nameFile != "")
                            {
                                System.IO.File.WriteAllText(OptionManager.nameFile, context.Item.Name);
                            }
                            if (OptionManager.artistsFile != "")
                            {
                                System.IO.File.WriteAllText(OptionManager.artistsFile, artists);
                            }
                            if (OptionManager.imageFile != "")
                            {
                                WebClient client = new WebClient();
                                client.DownloadFile(image, OptionManager.imageFile);
                            }
                        }
                    }
                    Thread.Sleep(Decimal.ToInt32(OptionManager.checkInterval));
                    context = api.GetPlayback();
                }
                Console.WriteLine("While loop ended.");
                if(!logout) { 
                    startAuthentification(auth);
                } else
                {
                    logout = false;
                }
            };
            auth.Start();

            if (!System.IO.File.Exists("settings.config"))
            {
                OptionManager.createFile();
                System.Windows.Forms.MessageBox.Show("Use \"Remember me\", or else you'll have to manually log in from the options menu every time!");
            }
            else
            {
                OptionManager.readFile();
            }

            startAuthentificationWindow(auth);

            npi = new NowPlayingInterface();
            npi.ShowDialog();
        }

        static void startAuthentification(ImplictGrantAuth auth)
        {
            CefSharp.OffScreen.ChromiumWebBrowser browser = new CefSharp.OffScreen.ChromiumWebBrowser(auth.GetUri());
        }

        public static void startAuthentificationWindow(ImplictGrantAuth auth)
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
