using System;

namespace Square_Note.Objects
{
    public class ToDoList
    {
        public int ID;
        public string Title;
        public ToDoListItem[] Items;
        public DateTime CreateTime;
        public DateTime? UpdateTime;
        public bool IsDeleted;

        public int Length
        {
            get { return Items.Length; }
        }

        public ToDoList()
        {
            ID = new Random().Next();
            Title = "Untitled To-Do List";
            Items = [];
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }

        private void UpdateIndices()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Index = i;
            }
        }

        public void PrependItem(ToDoListItem item)
        {
            Items = [item, .. Items];
            UpdateIndices();
        }

        public void ReplaceItem(int index, ToDoListItem item)
        {
            Items[index] = item;
            UpdateIndices();
        }

        public void RemoveItemAt(int index)
        {
            // Créer un nouveau tableau avec une taille réduite
            ToDoListItem[] nouveauTableau = new ToDoListItem[Items.Length - 1];

            // Copier les éléments avant l'index
            Array.Copy(Items, 0, nouveauTableau, 0, index);

            // Copier les éléments après l'index
            Array.Copy(Items, index + 1, nouveauTableau, index, Items.Length - index - 1);

            Items = nouveauTableau;
            UpdateIndices();
        }
    }

    public class ToDoListItem
    {
        public int Index;
        public string Label;
        public Boolean Checked;

        public ToDoListItem()
        {
            Index = 0;
            Label = "New Item";
            Checked = false;
        }

        public ToDoListItem(string label)
        {
            Index = 0;
            Label = label;
            Checked = false;
        }
    }
}