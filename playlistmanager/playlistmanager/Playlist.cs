using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playlistmanager {
    internal class Playlist<T> {
        Song<T> head, tail, current;
        bool play = false;
        bool loop = false;
        public Playlist() {
            head = null;
            tail = null;
            current = null;
            bool play = false;
            bool loop = false;
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
            if (head == null) {
                Console.WriteLine("Playlist is empty. Cannot play.");
                return;
            }
            current = head;
            play = true;
            int timer = 0;
            while (current != null) {
                Console.WriteLine($"Playing: {current.title} || {current.artist}");
                timer = 0;
                //problem with this is that its stuck in the while loop and cant be interrupted to fix this I could multithread?
                while (timer < current.duration) {
                    if (!play) {
                        Console.WriteLine("Playback paused.");
                        return;
                    }
                    Console.Write("Time: " + timer + "s / " + current.duration + "s\r");
                    System.Threading.Thread.Sleep(1000); // Simulate 1 second of playback
                    timer++;
                }
            }

        }
        public void Pause() {
            play = false;
        }
        public void Stop() {
            play = false;
            current = head;

        }
        public void Skip() {
            if (current != null) {
                current = current.next;
            } else {
                play = false;
                current = head;
            }
        }
    }
}
