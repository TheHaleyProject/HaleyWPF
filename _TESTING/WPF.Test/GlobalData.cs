using Haley.Models;

namespace WPF.Test
{
    public class GlobalData : ChangeNotifier
    {
        private Theme _current_theme;
        public Theme current_theme
        {
            get { return _current_theme; }
            set { SetProp(ref _current_theme, value); }
        }

        private Theme _old_theme;
        public Theme old_theme
        {
            get { return _old_theme; }
            set { SetProp(ref _old_theme, value); }
        }

        public static GlobalData Singleton = new GlobalData();
        public static GlobalData getSingleton()
        {
            if (Singleton == null) Singleton = new GlobalData();
            return Singleton;
        }

        public static void Clear()
        {
            Singleton = new GlobalData();
        }
        private GlobalData() { current_theme = new Theme() { }; old_theme = null; }

    }
}
