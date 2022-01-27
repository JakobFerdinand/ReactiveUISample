using NuGet.Protocol.Core.Types;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;

namespace ReactiveUISample
{
    public class NugetDetailsViewModel
    {
        private readonly IPackageSearchMetadata _metadata;
        private readonly Uri _defaultUri;

        public Uri IconUrl => _metadata.IconUrl ?? _defaultUri;
        public string Description => _metadata.Description;
        public Uri ProjectUrl => _metadata.ProjectUrl;
        public string Title => _metadata.Title;

        public ReactiveCommand<Unit, Process?> OpenPage { get; }

        public NugetDetailsViewModel(IPackageSearchMetadata metadata)
        {
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            _defaultUri = new("https://git.io/fAlfh");
            OpenPage = ReactiveCommand.Create(() => Process.Start(new ProcessStartInfo(ProjectUrl.ToString())
            {
                UseShellExecute = true
            }));
        }
    }
}