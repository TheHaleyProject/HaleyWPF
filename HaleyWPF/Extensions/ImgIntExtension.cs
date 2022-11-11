using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Haley.Enums;
using Haley.WPF.Controls;
using Haley.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Haley.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Markup;
using Isolated.Haley.WPF;

namespace Haley.Utils
{
    public class ImgIntExtension : MarkupExtension {
        public ImgIntExtension() {
            Kind = IconKind.empty_image;
        }

        //[ConstructorArgument("@enum")] //If we setup constructor argument, empty values will throw exception
        public IconKind Kind { get; set; }
       

        bool Process(out IconSourceKey source_key, out string resource_key) {
            source_key = IconSourceKey.Default;
            resource_key = Kind.ToString();
            return true;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            if (!Process(out var @enum, out var @key)) return null;
            return IconFinder.GetIcon(key, @enum);
        }
    }
}