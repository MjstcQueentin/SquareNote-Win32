using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace Square_Note.Objects
{
    /// <summary>
    /// Représente une note rapide
    /// </summary>
    public class QuickNote
    {
        public int ID;
        public Color BackgroundColour;
        public string Body;
        public DateTime CreateTime;
        public DateTime? UpdateTime;
        public bool IsDeleted;

        public QuickNote()
        {
            ID = new Random().Next();
            BackgroundColour = Color.FromArgb(255, 238, 219, 78);
            Body = "";
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }

        public SolidColorBrush BackgroundBrush
        {
            get
            {
                return new SolidColorBrush(BackgroundColour);
            }
        }

        public static readonly Color[] Colours = [
            Color.FromArgb(255, 238, 219, 78), // Jaune
            Color.FromArgb(255, 255, 204, 204), // Rouge
            Color.FromArgb(255, 179, 217, 255), // Bleu
            Color.FromArgb(255, 179, 255, 204), // Vert
        ];
    }
}
