using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class PRAboutViewModel
{
    public string PRTitle => AppInfo.Name;
    public string PRVersion => AppInfo.VersionString;
    public string PRMoreInfoUrl => "https://aka.ms/maui";
    public string PRMessage => "This app is written in XAML and C# with .NET MAUI.";
    public ICommand MRShowMoreInfoCommand { get; }

    public PRAboutViewModel()
    {
        MRShowMoreInfoCommand = new AsyncRelayCommand(ShowMoreInfo);
    }

    async Task ShowMoreInfo() =>
        await Launcher.Default.OpenAsync(PRMoreInfoUrl);
}