using Haley.Models;

namespace WPF.Test
{
    public class GlobalData : ChangeNotifier
    {
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
        private GlobalData() { }

    }
}
