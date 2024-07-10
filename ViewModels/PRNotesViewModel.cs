using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class PRNotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.PRNoteViewModel> AllNotes { get; }
    public ICommand PRNewCommand { get; }
    public ICommand PRSelectNoteCommand { get; }

    public PRNotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.PRNoteViewModel>(Models.PRNote.LoadAll().Select(n => new PRNoteViewModel(n)));
        PRNewCommand = new AsyncRelayCommand(PRNewNoteAsync);
        PRSelectNoteCommand = new AsyncRelayCommand<ViewModels.PRNoteViewModel>(PRSelectNoteAsync);
    }

    private async Task PRNewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task PRSelectNoteAsync(ViewModels.PRNoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.PRIdentifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            PRNoteViewModel matchedNote = AllNotes.Where((n) => n.PRIdentifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            PRNoteViewModel matchedNote = AllNotes.Where((n) => n.PRIdentifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.PRReload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new PRNoteViewModel(Models.PRNote.Load(noteId)));
        }
    }
}
