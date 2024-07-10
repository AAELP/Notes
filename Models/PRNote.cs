namespace Notes.Models;

internal class PRNote
{
    public string PRFilename { get; set; }
    public string PRText { get; set; }
    public DateTime PRDate { get; set; }

    public PRNote()
    {
        PRFilename = $"{Path.GetRandomFileName()}.notes.txt";
        PRDate = DateTime.Now;
        PRText = "";
    }

    public void Save() =>
    File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, PRFilename), PRText);

    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, PRFilename));

    public static PRNote Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                PRFilename = Path.GetFileName(filename),
                PRText = File.ReadAllText(filename),
                PRDate = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<PRNote> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.notes.txt")

                // Each file name is used to load a note
                .Select(filename => PRNote.Load(Path.GetFileName(filename)))

                // With the final collection of notes, order them by date
                .OrderByDescending(note => note.PRDate);
    }
}
