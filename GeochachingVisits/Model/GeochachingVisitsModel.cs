using GeochachingVisits.Helper;

namespace GeochachingVisits.Model
{
    public class GeochachingVisitsModel : NotifyObject
    {
        private string _GcCode;
        private string _FoundDate;
        private string _FoundStatus;
        private string _Message;


        public string GcCode
        {
            get { return this._GcCode; }
            set
            {
                if (value == this._GcCode)
                {
                    return;
                }

                this._GcCode = value;
                this.OnPropertyChanged();
            }
        }

        public string FoundDate
        {
            get { return this._FoundDate; }
            set
            {
                if (value == this._FoundDate)
                {
                    return;
                }

                this._FoundDate = value;
                this.OnPropertyChanged();
            }
        }

        public string FoundStatus
        {
            get { return this._FoundStatus; }
            set
            {
                if (value == this._FoundStatus)
                {
                    return;
                }

                this._FoundStatus = value;
                this.OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return this._Message; }
            set
            {
                if (value == this._Message)
                {
                    return;
                }

                this._Message = value;
                this.OnPropertyChanged();
            }
        }
    }
}
