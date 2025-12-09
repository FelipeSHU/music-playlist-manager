using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;

namespace playlistmanager {
    internal class Playlist<T> {
        Song<T> head, tail, current;
        bool play = false;
        bool loop = false;
        int timer = 0;
        Thread thread;
        public Playlist() {
            head = null;
            tail = null;
            current = null;
            bool play = false;
            bool loop = false;
            int timer = 0;
            thread = null;
        }
        public void Append(T title, T artist, T album, int duration) {
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
            current = head;
            while (current != null) {
                Console.WriteLine($"Title: {current.title}, Artist: {current.artist}, Album: {current.album}, Duration: {current.duration} seconds");
                current = current.next;
            }
        }
        public void Play() {
            if (head == null || tail == null) {
                Console.WriteLine("Playlist is empty. Cannot play.");
                return;
            }
            current = head;
            play = true;
            thread = new Thread(()=>PlayLoop());
            thread.Start();
        }
  
        private void PlayLoop() {
            while (current != null) {
                timer = 0;
                try {
                    while (timer < current.duration) {
                        Console.Write($"Playing:{current.title} || {current.artist} || Time: " + timer + "s / " + current.duration + "s\r");
                        if (play) {
                            timer++;
                            System.Threading.Thread.Sleep(1000);
                        } else {
                            Thread.Sleep(Timeout.InfiniteTimeSpan);
                            
                        }
                    }
                    if (!loop) {
                        current = current.next;
                    }
                } catch (ThreadInterruptedException) {
                    
                }
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
            if(thread.IsAlive){
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
            if (current != null) {
                if (loop) {
                    timer = 0;
                    return;
                } else if (current.next != null) {
                    current = current.next;
                    timer = 0; 
                }

            } else {
                play = false;
                current = head;
            }
        }
        public void Prev() {
            if (current != null) {
                if (loop) {
                    timer = 0;
                    return;
                } else if (current.prev != null) {
                    current = current.prev;
                    timer = 0;
                }
            } else {
                Console.WriteLine("\nNo previous song available.");
            }
        }
        //problem with the delete function is that if the song youre currently listening to is deleted, it breaks the play function.
        public void Delete(string name) {
            var temp = head;
            if(temp == null) {
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
    }
    
}
