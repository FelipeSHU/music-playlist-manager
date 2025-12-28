using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace playlistmanager {
    internal class Playlist<T> {
        Song<T> head, tail, current;
        bool play = false;
        bool loop = false;
        int timer = 0;
        Thread thread;
        private readonly object playLock = new object();
        public Playlist() {
            head = null;
            tail = null;
            current = null;
            bool play = false;
            bool loop = false;
            int timer = 0;
            thread = null;
        }
        public void Append(string title, string artist, string album, int duration) {
            var newSong = new Song<T>(title, artist, album, duration);
            if (head == null) {
                head = newSong;
                tail = newSong;
            } else {
                tail.next = newSong;
                newSong.prev = tail;
                tail = newSong;
            }
        }
        public void Display() {
            if (head == null) {
                Console.WriteLine("Playlist is empty.");
                return;
            }
            var temp = head;
                
                while (temp != null) {
                    Console.WriteLine($"Title: {temp.title}, Artist: {temp.artist}, Album: {temp.album}, Duration: {temp.duration} seconds");
                    temp = temp.next;
                
            }
        }
        public void Play() {
            if (head == null || tail == null) {
                Console.WriteLine("Playlist is empty. Cannot play.");
                return;
            }
            current = head;
            play = true;
            thread = new Thread(() => PlayLoop());
            thread.Start();
        }

        private void PlayLoop() {
            while (true) {
                Song<T> currentSong = null;
                timer = 0;
                lock (playLock) {
                    if (current == null) break;
                    currentSong = current;
                }



                try {
                    while (timer < currentSong.duration) {
                        Console.Write($"Playing:{currentSong.title} || {currentSong.artist} || Time: " + timer + "s / " + currentSong.duration + "s\r");
                        if (play) {
                            timer++;
                            System.Threading.Thread.Sleep(1000);
                        } else {
                            Thread.Sleep(Timeout.InfiniteTimeSpan);

                        }
                    }
                    lock (playLock) {
                        if (!loop) {
                            current = current.next;
                            Console.WriteLine();
                        }
                    }


                } catch (ThreadInterruptedException) {
                    continue;
                }

                Console.WriteLine();

            }
        }

        
        public void Pause() {
            if (current != null) {
                play = !play;
                if (play) {
                    thread.Interrupt();
                }
            } else {
                Console.WriteLine("\nNo song is currently selected.");
            }
        }
        public void Stop() {
            if (thread.IsAlive) {
                //i should ask for help on threads tomorrow to see if i can kill the thread here.
                play = false;
                current = head;
                timer = 0;
            }

        }

        public void Loop() {
            loop = !loop;
        }

        public void Skip() {
            lock (playLock) {
                if (current != null) {
                    if (loop) {
                        timer = 0;
                        return;
                    } else if (current.next != null) {
                        current = current.next;
                        timer = 0;
                    }
                    if (thread.IsAlive) {
                        thread.Interrupt();
                    }
                } else {
                    play = false;
                    current = head;
                }

            }
            Console.WriteLine();
        }
        public void Prev() {
                lock (playLock) {
                    if (current != null) {
                        if (loop) {
                            timer = 0;
                            return;
                        } else if (current.prev != null) {
                            current = current.prev;
                            timer = 0;
                        }
                        if (thread.IsAlive) {
                            thread.Interrupt();
                        }
                    }
                }
            Console.WriteLine();

        }
        //problem with the delete function is that if the song youre currently listening to is deleted, it breaks the play function. should add an if case for it.
        public void Delete(string name) {
            var temp = head;
            if (temp == null) {
                Console.WriteLine("\nPlaylist is empty.");
                return;
            }
            if (temp != null) {
                while (temp != null) {
                    if (temp.title.Equals(name)) {
                        if (temp.next == null) {
                            temp.prev.next = null;
                            tail = temp.prev;
                        }
                        if (temp.prev == null) {
                            temp.next.prev = null;
                            head = temp.next;
                        }
                        if (temp.next != null && temp.prev != null) {
                            temp.prev.next = temp.next;
                            temp.next.prev = temp.prev;
                        }
                        temp = null;

                        return;
                    }
                    temp = temp.next;
                }
                if (temp != null) {
                    Console.WriteLine("\nSong not found in playlist.");
                    temp = null;
                }
            }
        }
        public void Delete(int index) {
            var temp = head;
            if (temp == null) {
                Console.WriteLine("\nPlaylist is empty.");
                return;
            }
            if (index < Length()) {
                for (int i = 0; i < index; i++) {
                    temp = temp.next;
                }
                if (temp.next == null) {
                    temp.prev.next = null;
                    tail = temp.prev;
                }
                if (temp.prev == null) {
                    temp.next.prev = null;
                    head = temp.next;
                }
                if (temp.next != null && temp.prev != null) {
                    temp.prev.next = temp.next;
                    temp.next.prev = temp.prev;
                }
                temp = null;
            } else {
                Console.WriteLine("\nIndex out of bounds.");
            }
        }
        public int Length() {
            int count = 0;
            var temp = head;
            while (temp != null) {
                count++;
                temp = temp.next;
            }
            return count;
        }

        public void Search(string name) {
            if (head == null) { return; }
            var temp = head;
            int counter = 0;
            while (temp != null) {
                counter++;
                
                if (temp.title.Equals(name)) {
                    Console.WriteLine($"Title: {temp.title}, Artist: {temp.artist} || Position in Playlist: {counter}");
                    return;
                    
                }
                temp = temp.next;
            }
            Console.WriteLine("Song not found in playlist.");
        }
        public void Shuffle() { 
            if(head == null || head.next == null) {
                return;
            }
            

            lock (playLock) {
                List<Song<T>> songs = new List<Song<T>>();
                var temp = head;
                while (temp != null) {
                    songs.Add(temp);
                    temp = temp.next;
                }

                //Console.WriteLine($"\nCollected {songs.Count} songs for shuffling.");
                //shuffle
                Random rand = new Random();
                for (int i = songs.Count - 1; i > 0; i--) {
                    int j = rand.Next(i + 1);
                    (songs[i], songs[j]) = (songs[j], songs[i]);

                }

                foreach (var song in songs) {
                    song.next = null;
                    song.prev = null;
                }
                //rebuild
                head = songs[0];
                head.prev = null;
                //Console.WriteLine(songs.Count);
                for (int i = 0; i < songs.Count - 1; i++) {
                    songs[i].next = songs[i + 1];
                    songs[i + 1].prev = songs[i];
                }
                tail = songs[songs.Count-1];
                tail.next = null;
                //resuming play
                current = head;
                timer = 0;
            }
            

            //Console.WriteLine(Length());
        }

        //Merge Sorting Below
        public void TitleSort() {
            head = head.AlphaMergeSort(head);
            
            var temp = head;
            while (temp.next != null) {
                temp = temp.next;
            }
            tail = temp;
            lock (playLock) {
                current = head;
                timer = 0;
                if (thread.IsAlive) {
                    thread.Interrupt();
                }
            }
        }
        public void DurationSort() {
            head = head.IntMergeSort(head);
            
            var temp = head;
            while (temp.next != null) {
                temp = temp.next;
            }
            tail = temp;
        }
    }
}
