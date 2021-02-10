using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnitTest.Annotations;

namespace UnitTest
{
    public class Patient:Person,INotifyPropertyChanged
    {
        public Patient()
        {
            IsNew = true;
            BloodSugar = 4.900003F;
            History = new List<string>();

            
        }


        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public int HeartBeatRate { get; set; }
        public bool IsNew { get; set; }
        public float BloodSugar { get; set; }

        public List<string> History { get; set; }

        public void NotAllowed()
        {
            throw new InvalidOperationException("Not able to create");
        }

        public void IncreaseHeartBearRate()
        {
            HeartBeatRate = CalculateHeartBeatRate() + 2;
            OnPropertyChanged(nameof(HeartBeatRate));
        }

        private int CalculateHeartBeatRate()
        {
            var random = new Random();
            return random.Next(1, 100);
        }

        public event EventHandler<EventArgs> PatientSleep;

        public void OnPatientSleep()
        {
            PatientSleep?.Invoke(this,EventArgs.Empty);
        }

        public void Sleep()
        {
            OnPatientSleep();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
