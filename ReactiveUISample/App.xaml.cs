using ReactiveUI;
using Splat;
using System.Reflection;
using System.Windows;

namespace ReactiveUISample
{
    public partial class App : Application
    {
        public App()
            => Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
    }
}
