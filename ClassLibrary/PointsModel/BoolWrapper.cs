using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ClassLibrary.PointsModel
{
    public class BoolWrapper:INotifyPropertyChanged
    {
        private bool value;
        public bool Value {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        public BoolWrapper(bool value) { Value = value; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
