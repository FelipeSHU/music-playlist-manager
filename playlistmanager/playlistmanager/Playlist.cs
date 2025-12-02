using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playlistmanager {
    internal class Playlist<T> {
        Song<T> head, tail;
        public Playlist() {
            head = null;
            tail = null;
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
            if(head == null) {
                Console.WriteLine("Playlist is empty.");
                return;
            }
            Song<T> current = head;
            while (current != null) {
                Console.WriteLine($"Title: {current.title}, Artist: {current.artist}, Album: {current.album}, Duration: {current.duration} seconds");
                current = current.next;
            }
        }

    }
}
