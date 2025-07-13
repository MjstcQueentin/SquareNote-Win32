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

        public ToDoList()
        {
            ID = new Random().Next();
            Title = "Untitled To-Do List";
            Items = [];
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
    }

    public class ToDoListItem
    {
        public string Label;
        public Boolean Checked;

        public ToDoListItem()
        {
            Label = "New Item";
            Checked = false;
        }
    }
}
