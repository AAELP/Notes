using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class PRNoteViewModel : ObservableObject, IQueryAttributable
{
    private Models.PRNote _note;

    public string PRText
    {
        get => _note.PRText;
        set
        {
            if (_note.PRText != value)
            {
                _note.PRText = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime PRDate => _note.PRDate;

    public string PRIdentifier => _note.PRFilename;

    public ICommand PRSaveCommand { get; private set; }
    public ICommand PRDeleteCommand { get; private set; }

    public PRNoteViewModel()
    {
        _note = new Models.PRNote();
        PRSaveCommand = new AsyncRelayCommand(PRSave);
        PRDeleteCommand = new AsyncRelayCommand(MRDelete);
    }

    public PRNoteViewModel(Models.PRNote note)
    {
        _note = note;
        PRSaveCommand = new AsyncRelayCommand(PRSave);
        PRDeleteCommand = new AsyncRelayCommand(MRDelete);
    }

    private async Task PRSave()
    {
        _note.PRDate = DateTime.Now;
        _note.Save();
        await Shell.Current.GoToAsync($"..?saved={_note.PRFilename}");
    }

    private async Task MRDelete()
    {
        _note.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_note.PRFilename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = Models.PRNote.Load(query["load"].ToString());
            PRRefreshProperties();
        }
    }

    public void PRReload()
    {
        _note = Models.PRNote.Load(_note.PRFilename);
        PRRefreshProperties();
    }

    private void PRRefreshProperties()
    {
        OnPropertyChanged(nameof(PRText));
        OnPropertyChanged(nameof(PRDate));
    }
}
