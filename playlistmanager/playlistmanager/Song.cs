using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playlistmanager {
    internal class Song<T> {
        public T title;
        public T artist;
        public T album;
        public int duration; // Duration in seconds
        public Song<T> next, prev;

        public Song(T eTitle, T eArtist, T eAlbum, int eDuration) {
            title = eTitle;
            artist = eArtist;
            album = eAlbum;
            duration = eDuration;
            next = null;
            prev = null;

        }

    }
}
