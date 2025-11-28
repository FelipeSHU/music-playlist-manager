using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playlistmanager {
    internal class Song<T> {
        public T Title;
        public T Artist;
        public T Album;
        public int Duration; // Duration in seconds

    }
}
