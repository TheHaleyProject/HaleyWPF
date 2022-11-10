using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Enums;
using System.Windows.Media;
using Haley.Utils;

namespace Isolated.Haley.WPF {
    public static class IconFinder {
        static bool _initialized = false;
        const string PACK_PATH = "pack://application:,,,/Haley.WPF;component/Dictionaries/";
        static object sourceLock = new object();
        static void EnsureInitialized() {
            if (_initialized) return;

            lock (sourceLock) {
                if (_initialized) return;
                //try to initialize
                ResourceFetcher.AddSource(IconSourceKey.Default, new Uri($@"{PACK_PATH}internalIcons.xaml", UriKind.RelativeOrAbsolute));
                _initialized = true;
            }
        }

        public static ImageSource GetIcon(string resource_key, IconSourceKey sourceKey) {
            EnsureInitialized();
            return ResourceFetcher.GetResource(sourceKey, resource_key) as ImageSource;
        }

        public static ImageSource GetIcon(string resource_key) {
            EnsureInitialized();
            //Get from any
            return ResourceFetcher.GetResourceAny(resource_key) as ImageSource;
        }

        public static ImageSource GetDefaultIcon() {
            return GetIcon(IconKind.empty_image.ToString(), IconSourceKey.Default);
        }
    }
}
