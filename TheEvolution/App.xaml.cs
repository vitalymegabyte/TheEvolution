using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace TheEvolution
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Environment environment;
        public Environment Environment { get { return environment; } set { environment = value; ((MainWindow)MainWindow).SetEnvironment(environment); } }
    }
}
