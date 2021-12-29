using System.Text;
using System.Windows.Input;

namespace Haley.Models
{
    /// <summary>
    /// Main class for Suggestion
    /// </summary>
    public class Suggestion
    {
        public string Text { get; set; }
        public object Content { get; set; }
        public Suggestion(string text,object content = null) 
        {
            Text = text;
            Content = content;
        }
        public Suggestion()
        {

        }
        public override string ToString()
        {
            return this.Text ?? base.ToString();
        }
    }
}