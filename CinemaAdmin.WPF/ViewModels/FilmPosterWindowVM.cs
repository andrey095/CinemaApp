using Cinema.DAL.Infrastructure;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAdmin.WPF.ViewModels
{
    public class FilmPosterWindowVM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private byte[] picture;
        public byte[] Picture { get => picture; set { picture = value; OnPropertyChanged(); } }
        public bool IsSaved { get; set; }

        public FilmPosterWindowVM()
        {
            AddPicture = new RelayCommand(AddPictureMethod);
            SavePicture = new RelayCommand(SavePictureMethod);
        }
        public RelayCommand AddPicture { get; set; }
        public void AddPictureMethod(object obj)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.JPEG;*.JPE;*.ICO;*.PNG";
            if(ofd.ShowDialog() == true)
            {
                IsSaved = false;
                FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                Picture = br.ReadBytes((int)fStream.Length);
                br.Dispose();
            }
        }
        public RelayCommand SavePicture { get; set; }
        public void SavePictureMethod(object obj)
        {
            IsSaved = true;
        }
    }
}
