using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Insurance.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {

        }

        public virtual void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
