using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System.Windows;
using WPF.Test.Controls;
using Haley.WPF.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;
using System;
using Haley.Services;
using WPF.Test.External;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            prepareTheme();
            LangUtils.Register("WPF.Test.Properties.Resources");
            LangUtils.ChangeCulture("ta");
            //TempMethod();
            ContainerRegistrations(); //For notification test
            var _basewndw = new BaseWindow();
            _basewndw.ShowDialog();
        }

        private void ContainerRegistrations()
        {
            var _key =ContainerStore.Controls.RegisterWithKey<MainVM, LocalView2>("localdemokey", mode: RegisterMode.Transient);
            var _key2 =ContainerStore.Controls.RegisterWithKey<MainVM, VanakkamView>("testkitty2", mode: RegisterMode.Transient);
        }

        private void prepareTheme()
        {
            var _ts = ThemeService.Singleton;
            _ts.SetupInternalTheme(HaleyThemeProvider.Prepare);//Attach internal themes.

            //Group 1
            var internalBuilder = new InternalThemeBuilder()
                .Add("Theme1", InternalThemeMode.Normal)
                .Add("Theme2", InternalThemeMode.Mild)
                .Add("Theme3", InternalThemeMode.Dark);
            _ts.RegisterGroup(internalBuilder);

            //Register external themes if required.
            var _lightTheme = new Uri($@"pack://application:,,,/WPF.Test;component/Resources/ThemeLight.xaml", UriKind.RelativeOrAbsolute);
            var _darkTheme = new Uri($@"pack://application:,,,/WPF.Test;component/Resources/ThemeDark.xaml", UriKind.RelativeOrAbsolute);

            _ts.RegisterGroup(new AssemblyThemeBuilder()
                .Add("Theme1", _lightTheme)
                .Add("Theme2", _darkTheme));


            _ts.RegisterGroup(new AssemblyThemeBuilder()
               .Add("Theme1", new Uri("pack://application:,,,/WPF.Test.External;component/Resources/ThemeDark.xaml", UriKind.RelativeOrAbsolute))
               .Add("Theme2", new Uri("pack://application:,,,/WPF.Test.External;component/Resources/ThemeLight.xaml", UriKind.RelativeOrAbsolute)));

            EntryModule.Initiate(_ts);

            //Set startuptheme
            _ts.SetStartupTheme("Theme1");
            _ts.ReplaceMode = ThemeReplaceMode.IgnoreFindReplaceAndCreateCopy;
            _ts.BuildAndFillMissing();

        }
    }
}
