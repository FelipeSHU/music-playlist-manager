using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playlistmanager {
    internal class Song<T> {
        public string album, artist, title;
        public int duration; // Duration in seconds
        public Song<T> next, prev;

        public Song( string eTitle, string eArtist, string eAlbum, int eDuration) {
            title = eTitle;
            artist = eArtist;
            album = eAlbum;
            duration = eDuration;
            next = null;
            prev = null;

        }
        private Song<T> Split(Song<T> headNode) {
            var fast = headNode;
            var slow = headNode;
            while (fast != null && fast.next != null && fast.next.next !=null) {
                fast = fast.next.next;
                slow = slow.next;
            }
            var temp = slow.next;
            slow.next = null;
            if (temp != null) {
                temp.prev = null;
            }
            return temp;
        }
        private Song<T> AlphaMerge(Song<T> first, Song<T> second) {
            if (first == null) {
                return second;
            }
            if (second == null) {
                return first;
            }
            if (first.title.CompareTo(second.title) < 0) {
                first.next = AlphaMerge(first.next, second);
                if (first.next != null) {
                    first.next.prev = first;
                }
                first.prev = null;
                return first;
            } else {
                second.next = AlphaMerge(first, second.next);
                if (second.next != null) {
                    second.next.prev = second;
                }
                second.prev = null;
                return second;

            }
        }
        private Song<T> IntMerge(Song<T> first, Song<T> second) {
            if (first == null) {
                return second;
            }
            if (second == null) {
                return first;
            }
            if (first.duration < second.duration) {
                first.next = IntMerge(first.next, second);
                if (first.next != null) {
                    first.next.prev = first;
                }
                first.prev = null;
                return first;
            } else {
                second.next = IntMerge(first, second.next);
                if (second.next != null) {
                    second.next.prev = second;
                }
                second.prev = null;
                return second;

            }
        }
        public Song<T> AlphaMergeSort(Song<T> headNode) {
            if (headNode == null || headNode.next == null) {
                return headNode;
            }
            var second = Split(headNode);
            headNode = AlphaMergeSort(headNode);
            second = AlphaMergeSort(second);
            return AlphaMerge(headNode, second);
        }

        public Song<T> IntMergeSort(Song<T> headNode) {
            if (headNode == null || headNode.next == null) {
                return headNode;
            }
            var second = Split(headNode);
            headNode = IntMergeSort(headNode);
            second = IntMergeSort(second);
            return IntMerge(headNode, second);
        }

    }
}
