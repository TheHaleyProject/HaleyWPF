using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Haley.Enums;
using Haley.Models;
using Haley.Abstractions;

namespace Haley.Utils {
    internal class IconSourceProvider : ChangeNotifier, IIconSourceProvider {

        Enum resource_key;
        IconSourceKey source_key;

        public ImageSource IconSource {
            get {
                //Based on the enum value, we try to fetch the data.
                return IconFinder.GetIcon(resource_key.ToString(), source_key);
            }
        }

        public void OnDataChanged(object input) {
            //We expect the data to be in the format of Enum or string.
            object data = input; //incoming string

            //Currently we have only one type and in future as well we might try to restrict to one type only.
            if (data is string dstr) {
                //Try to change the string to enum
                do {
                    //Check brand kind
                    if (Enum.TryParse<IconKind>(dstr, true, out var _ikind)) {
                        data = _ikind;
                        break;
                    }
                } while (false);
            }

            if (!(data is Enum @enum)) {
                SetDefault();
            } else {
                resource_key = @enum;
                if (@enum is IconKind) {
                    source_key = IconSourceKey.Default;
                } else {
                    //If enum itself is not our kind, we just reset the resource_key as well
                    SetDefault();
                }
            }
            OnPropertyChanged(nameof(IconSource));
        }

        void SetDefault() {
            resource_key = IconKind.empty_image;
            source_key = IconSourceKey.Default;
        }

        public IconSourceProvider() { SetDefault(); }

    }
}
