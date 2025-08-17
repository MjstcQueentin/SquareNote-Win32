using Square_Note.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Square_Note.Providers
{
    /// <summary>
    /// Permet la lecture et l'écriture des listes de tâches sur le disque local
    /// </summary>
    internal class ToDoListProvider
    {
        private static readonly XmlSerializer serializer = new(typeof(ToDoList));
        public static readonly string RootRepertory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote\\ToDoLists\\";

        public static event EventHandler? ToDoListsModified;

        public static List<ToDoList> GetToDoLists()
        {
            var list = new List<ToDoList>();
            var fileEnum = Directory.EnumerateFiles(RootRepertory).GetEnumerator();
            while (fileEnum.MoveNext())
            {
                FileStream file = File.OpenRead(fileEnum.Current);
                try
                {
                    object? deserialized = serializer.Deserialize(file);
                    if (deserialized is not null)
                    {
                        list.Add((ToDoList)deserialized);
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
        public static ToDoList GetToDoList(int id)
        {
            if (File.Exists($"{RootRepertory}{id}.xml"))
            {
                FileStream file = File.OpenRead($"{RootRepertory}{id}.xml");
                object? deserialized = serializer.Deserialize(file);
                if (deserialized is not null)
                {
                    file.Close();
                    return (ToDoList)deserialized;
                }
            }

            throw new FileNotFoundException();
        }

        public static ToDoList CreateNewToDoList()
        {
            ToDoList note = new();
            int id = note.ID;

            while (File.Exists($"{RootRepertory}{id}.xml"))
            {
                note = new ToDoList();
                id = note.ID;
            }

            return note;
        }

        /// <summary>
        /// Tenter d'écrire une note
        /// </summary>
        /// <param name="note"></param>
        public static void SaveToDoList(ToDoList note)
        {
            if (File.Exists($"{RootRepertory}{note.ID}.xml"))
            {
                File.Delete($"{RootRepertory}{note.ID}.xml");
            }

            FileStream file = File.OpenWrite($"{RootRepertory}{note.ID}.xml");
            serializer.Serialize(file, note);
            file.Close();
            if (ToDoListsModified is not null) ToDoListsModified!.Invoke(typeof(ToDoListProvider), EventArgs.Empty);
        }

        public static void DeleteToDoList(int noteID)
        {
            if (File.Exists($"{RootRepertory}{noteID}.xml"))
            {
                File.Delete($"{RootRepertory}{noteID}.xml");
                if (ToDoListsModified is not null) ToDoListsModified!.Invoke(typeof(ToDoListProvider), EventArgs.Empty);
            }
        }
    }
}
