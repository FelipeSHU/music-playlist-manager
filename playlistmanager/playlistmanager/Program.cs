using System;

namespace playlistmanager {
    internal class Program {

        private static void Main(string[] args) {
            //Read songlist from file and add them as Songs to Playlist
            string path = "songs_dataset.csv";
            //have to fix the path issue later => fixed
            StreamReader reader = null;
            if (File.Exists(path)) {
                reader = new StreamReader(path);
                Playlist<string> playlist = new Playlist<string>();
                //Skip headers line
                var line = reader.ReadLine();
                while (!reader.EndOfStream) {
                    line = reader.ReadLine();
                    var values = line.Split(',');
                    string title = values[1];
                    string artist = values[2];
                    string album = values[3];
                    string textDuration = values[4];
                    var timeValues =  textDuration.Split(':');
                    int duration = Int32.Parse(timeValues[0]) * 60 + Int32.Parse(timeValues[1]);
                    playlist.Append(title, artist, album, duration);
                }
                reader.Close();
                playlist.Display();

                playlist = Shuffle(playlist);
                playlist.Display();
                Console.WriteLine("Re-Shuffled Playlist:");
                playlist = Shuffle(playlist);
                playlist.Display();






                /*
                Thread thread = new Thread(()=>PlayThread(playlist));
                thread.Start();

                thread.Join();
                */

            } else { Console.WriteLine("Error finding file");}



        }

        private static Playlist<string> Shuffle(Playlist<string> playlist) { 
            var templist = new Playlist<string>();
            Random rand = new Random();
            for (int i = playlist.Length()-1; i > 0; i-- ) {
                int r = rand.Next(0, i);
                var song = playlist.Search(r);
                templist.Append(song.title, song.artist, song.album, song.duration);
                playlist.Delete(r);

            }
            
            return templist;
        }
        /*
        static void PlayThread(Playlist<string> playlist) {
            //
            playlist.Play();
        }
        */
        

    }
}
