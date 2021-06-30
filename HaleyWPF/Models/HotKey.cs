using System.Text;
using System.Windows.Input;

namespace Haley.Models
{
    /// <summary>
    /// Main class for hotkey
    /// </summary>
    public class HotKey
    {
        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the key 
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// Gets modifier keys 
        /// </summary>
        public ModifierKeys Modifiers { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey" /> class.
        /// </summary>
        public HotKey(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
            DisplayName = GetDisplayName();
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
            return (input.Key == this.Key && input.Modifiers == this.Modifiers);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string GetDisplayName()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }
    }
}