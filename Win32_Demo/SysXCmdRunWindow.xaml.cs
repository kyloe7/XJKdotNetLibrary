﻿using System;
using System.ComponentModel;
using System.Windows;
using XJK;
using XJK.Win32;
using XJK.Win32.CommandHelper;

namespace Win32_Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SysXCmdRunWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        public SysXCmdRunWindow()
        {
            InitializeComponent();
            var listener = new XJK.Logger.Listener(s =>
            {
                LogText(s);
            })
            { Registered = true };
            this.Title = AppEnv.IsAdministrator() ? "Administrator" : "Normal User";
            this.CommandBox.Text = ENV.EntryLocation; //"D:\\space space.exe";
            foreach (var x in Environment.GetCommandLineArgs())
            {
                LogText(x + Environment.NewLine);
            }
        }

        private void StreamResultSync(object sender, RoutedEventArgs e)
        {
            using (CommandStreamSynchronous CommandStream = new CommandStreamSynchronous("cmd", ""))
            {
                CommandStream.WriteLine(Command + " " + Args);
                LogText(CommandStream.Output.ReadLine());
            }
        }

        private void StreamResultAsync(object sender, RoutedEventArgs e)
        {
            using (CommandStreamAsync CommandStream = new CommandStreamAsync("cmd", ""))
            {
                CommandStream.Receive(s => LogText(s), s => LogText(s));
                CommandStream.WriteLine(Command + " " + Args);
            }
        }

        private void LogText(string obj)
        {
            LogBox.Dispatcher.Invoke(() =>
            {
                LogBox.Text += obj;
            });
        }

        private void CatchException(Exception ex)
        {

        }

        public string Command => CommandBox.Text;
        public string Args => ArgsBox.Text;


        private Privilege s_Privilege = Privilege.NotSet;
        private LaunchType s_LaunchType = LaunchType.NotSet;
        private WindowType s_WindowType = WindowType.NotSet;

        public Privilege Privilege
        {
            get => s_Privilege; set
            {
                s_Privilege = value;
                OnPropertyChanged("Privilege");
            }
        }
        public LaunchType LaunchType
        {
            get => s_LaunchType; set
            {
                s_LaunchType = value;
                OnPropertyChanged("LaunchType");
            }
        }
        public WindowType WindowType
        {
            get => s_WindowType; set
            {
                s_WindowType = value;
                OnPropertyChanged("WindowType");
            }
        }

        private void RunAsInvoker(object sender, RoutedEventArgs e)
        {
            CmdRunner.RunAsInvoker(Command, Args);
        }

        private void RunAsAdmin(object sender, RoutedEventArgs e)
        {
            CmdRunner.RunAsAdmin(Command, Args);
        }

        private void RunWithCmdStart(object sender, RoutedEventArgs e)
        {
            CmdRunner.RunWithCmdStart(Command, Args);
        }

        private void RunRadioOption(object sender, RoutedEventArgs e)
        {
            ProcessInfoChain.New(Command, Args)
                .SetWindow(WindowType)
                .LaunchBy(LaunchType)
                .RunAs(Privilege)
                .Excute()
                .Catch(CatchException);
        }

        private void ParseArgs(object sender, RoutedEventArgs e)
        {
            string x = CommandBox.Text;
            var tuple = Cmder.SplitCommandArg(x);
            LogText($"Command :{tuple.Item1}{Environment.NewLine}Arg     :{tuple.Item2}{Environment.NewLine}");
        }

        private void SetCurrentExePath(object sender, RoutedEventArgs e)
        {
            CommandBox.Text = ENV.EntryLocation;
        }

        private void ClearLog(object sender, RoutedEventArgs e)
        {
            LogBox.Text = "";
        }

        private void SetCmdPath(object sender, RoutedEventArgs e)
        {
            CommandBox.Text = "cmd";
            ArgsBox.Text = "/c dir";
        }

        private void GetVerbs(object sender, RoutedEventArgs e)
        {
            LogText(Cmder.GetRunVerbs(Command));
        }
    }
}
