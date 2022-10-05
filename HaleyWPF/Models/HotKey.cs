using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using System;
using System.Linq;

namespace Haley.Models
{
    /// <summary>
    /// Main class for hotkey
    /// </summary>
    public class HotKey
    {
        string _displayname = null;
        public string Id { get; }
        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the key 
        /// </summary>
        public IEnumerable<Key> Keys { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey" /> class.
        /// </summary>
        public HotKey(Key key):this(new List<Key> { key}){

        }

        public HotKey(IEnumerable<Key> keys) {
            Keys = keys;
            DisplayName = GetDisplayName();
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetDisplayName();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var input = obj as HotKey;
            if (input == null) return false;
            //https://stackoverflow.com/questions/22173762/check-if-two-lists-are-equal
            return (this.Keys.All(input.Keys.Contains) &&  this.Keys.Count() == input.Keys.Count());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string GetDisplayName()
        {
            if (_displayname != null) return _displayname;
            var incoming = new List<Key>(Keys);
            //incoming.Sort((first, next) => ((int)first).CompareTo((int)next)); //use "-first.CompareTo(next)" for inverse
            //incoming.Sort(); //will sort by string

            //44 to 69 are alphabets
            var _alphabets = incoming.Where(p => (int)p >= 44 && (int)p <= 69)?.ToList() ?? new List<Key>();
            var _others = incoming.Except(_alphabets).ToList();

            //Sort
            _alphabets?.Sort();
            _others?.Sort();

            _others.AddRange(_alphabets); //without sorting, add at the end.
          
            var str = new StringBuilder();
            int i = 0;
            int count = _others.Count();
            foreach (var key in _others) {
                str.Append($@"{key.ToString()}");
                i++;
                if (i < count) {
                    str.Append(" + ");
                }
            }
            _displayname = str.ToString();
            return _displayname;
        }
    }
}