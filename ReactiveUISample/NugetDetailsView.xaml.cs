using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Media.Imaging;

namespace ReactiveUISample
{
    public partial class NugetDetailsView
    {
        public NugetDetailsView()
        {
            InitializeComponent();
            this.WhenActivated(disposableRegistrations =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IconUrl,
                    view => view.iconImage.Source,
                    url => url is null ? null : new BitmapImage(url))
                    .DisposeWith(disposableRegistrations);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Title,
                    view => view.titleRun.Text)
                    .DisposeWith(disposableRegistrations);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Description,
                    view => view.descriptionRun.Text)
                    .DisposeWith(disposableRegistrations);
                
                this.BindCommand(ViewModel,
                    viewModel => viewModel.OpenPage,
                    view => view.openButton)
                    .DisposeWith(disposableRegistrations);
            });
        }
    }
}
