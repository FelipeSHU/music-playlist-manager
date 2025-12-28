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
                playlist.Play();
                        
                playlist.Shuffle();
                playlist.Display();
                playlist.TitleSort();
                playlist.Display();
                Thread.Sleep(5000);
                playlist.Skip();
                Thread.Sleep(5000);
                playlist.Skip();


            } else { Console.WriteLine("Error finding file");}



        }

        

    }
}
