using ReactiveUI;
using System.Reactive.Disposables;

namespace ReactiveUISample
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new();

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IsAvailable,
                    view => view.searchResultsListBox.Visibility)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SearchResults,
                    view => view.searchResultsListBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.SearchTerm,
                    view => view.searchTextBox.Text)
                    .DisposeWith(disposableRegistration);
            });
        }
    }
}
