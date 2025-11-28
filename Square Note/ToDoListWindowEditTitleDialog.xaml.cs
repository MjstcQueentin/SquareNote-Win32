using Microsoft.UI.Xaml.Controls;

namespace Square_Note
{
    /// <summary>
    /// Dialog pour renommer une ToDoList
    /// </summary>
    public sealed partial class ToDoListWindowEditTitleDialog : Page
    {
        public string InputText { 
            get
            {
                return ListTitleTextBox.Text;
            } 
            private set
            {
                ListTitleTextBox.Text = value;
            }
        }

        public ToDoListWindowEditTitleDialog()
        {
            InitializeComponent();
        }

        public ToDoListWindowEditTitleDialog(string defaultTitle)
        {
            InitializeComponent();
            ListTitleTextBox.Text = defaultTitle;
        }
    }
}
