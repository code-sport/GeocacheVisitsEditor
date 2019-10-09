using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GeochachingVisits.Helper
{

    public class NotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "Must not be set")]
        protected void InvokePropertyChanged([CallerMemberName] string name = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /* protected void OnPropertiesChanged([CallerMemberName] string trigger = "")
         {
             this.PropertyChanged?.Invoke(this, new ModelChangedEventArgs(string.Empty, trigger));
         }*/
    }

}
