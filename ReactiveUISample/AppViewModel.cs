using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveUISample
{
    public class AppViewModel : ReactiveObject
    {
        private string? _searchTerm;
        public string? SearchTerm
        {
            get => _searchTerm;
            set => this.RaiseAndSetIfChanged(ref _searchTerm, value);
        }

        private readonly ObservableAsPropertyHelper<IEnumerable<NugetDetailsViewModel>> _searchResults;
        public IEnumerable<NugetDetailsViewModel> SearchResults => _searchResults.Value;

        private readonly ObservableAsPropertyHelper<bool> _isAvailable;
        public bool IsAvailable => _isAvailable.Value;

        public AppViewModel()
        {
            _searchResults = this
                .WhenAnyValue(vm => vm.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(800))
                .Select(term => term?.Trim())
                .DistinctUntilChanged()
                .Where(term => string.IsNullOrWhiteSpace(term) is false)
                .Select(term => term!)
                .SelectMany(SearchNugetPackages)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, vm => vm.SearchResults);

            _searchResults
                .ThrownExceptions
                .Subscribe(error => { });

            _isAvailable = this
                .WhenAnyValue(vm => vm.SearchResults)
                .Select(searchResults => searchResults is not null)
                .ToProperty(this, vm => vm.IsAvailable);
        }

        private async Task<IEnumerable<NugetDetailsViewModel>> SearchNugetPackages(string term, CancellationToken cancellationToken)
        {
            PackageSource packageSource = new("https://api.nuget.org/v3/index.json");
            SourceRepository source = new(packageSource, Repository.Provider.GetCoreV3());
            ILogger logger = NullLogger.Instance;

            SearchFilter filter = new(includePrerelease: false);
            var resource = await source.GetResourceAsync<PackageSearchResource>().ConfigureAwait(false);
            var metadata = await resource.SearchAsync(term, filter, skip: 0, take: 10, logger, cancellationToken).ConfigureAwait(false);
            return metadata.Select(m => new NugetDetailsViewModel(m));
        }
    }
}
