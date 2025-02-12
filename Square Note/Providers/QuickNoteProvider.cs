using Square_Note.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Square_Note.Providers
{
    /// <summary>
    /// Permet la lecture et l'écriture des pense-bêtes sur le disque local
    /// </summary>
    internal class QuickNoteProvider
    {
        private static readonly XmlSerializer serializer = new(typeof(QuickNote));
        private static readonly string RootRepertory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote\\QuickNotes\\";

        public static event EventHandler? QuickNotesModified;

        public static List<QuickNote> GetQuickNotes()
        {
            var list = new List<QuickNote>();
            var fileEnum = Directory.EnumerateFiles(RootRepertory).GetEnumerator();
            while (fileEnum.MoveNext())
            {
                FileStream file = File.OpenRead(fileEnum.Current);
                try
                {
                    object? deserialized = serializer.Deserialize(file);
                    if (deserialized is not null)
                    {
                        list.Add((QuickNote)deserialized);
                    }
                    file.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    file.Close();
                    File.Delete(fileEnum.Current);
                }
            }

            return list;
        }

        /// <summary>
        /// Tenter de lire une note rapide
        /// </summary>
        /// <param name="id">ID de la note rapide</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static QuickNote GetQuickNote(int id)
        {
            if (File.Exists($"{RootRepertory}{id}.xml"))
            {
                FileStream file = File.OpenRead($"{RootRepertory}{id}.xml");
                object? deserialized = serializer.Deserialize(file);
                if (deserialized is not null)
                {
                    file.Close();
                    return (QuickNote)deserialized;
                }
            }

            throw new FileNotFoundException();
        }

        public static QuickNote CreateNewQuickNote()
        {
            QuickNote note = new();
            int id = note.ID;

            while (File.Exists($"{RootRepertory}{id}.xml"))
            {
                note = new QuickNote();
                id = note.ID;
            }

            return note;
        }

        /// <summary>
        /// Tenter d'écrire une note
        /// </summary>
        /// <param name="note"></param>
        public static void SaveQuickNote(QuickNote note)
        {
            if (File.Exists($"{RootRepertory}{note.ID}.xml"))
            {
                File.Delete($"{RootRepertory}{note.ID}.xml");
            }

            FileStream file = File.OpenWrite($"{RootRepertory}{note.ID}.xml");
            serializer.Serialize(file, note);
            file.Close();
            if(QuickNotesModified is not null) QuickNotesModified!.Invoke(typeof(QuickNoteProvider), EventArgs.Empty);
        }

        public static void DeleteQuickNote(int noteID)
        {
            if (File.Exists($"{RootRepertory}{noteID}.xml"))
            {
                File.Delete($"{RootRepertory}{noteID}.xml");
                if (QuickNotesModified is not null) QuickNotesModified!.Invoke(typeof(QuickNoteProvider), EventArgs.Empty);
            }
        }
    }
}
