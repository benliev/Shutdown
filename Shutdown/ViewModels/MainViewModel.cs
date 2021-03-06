﻿using Shutdown.Commands;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;

namespace Shutdown.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Shortcuts

        public TimeSpan[] Shortcuts { get; } = new TimeSpan[]
        {
            new TimeSpan(0, 10, 0),
            new TimeSpan(0, 15, 0),
            new TimeSpan(0, 30, 0),
            new TimeSpan(1, 0, 0),
            new TimeSpan(2, 0, 0),
            new TimeSpan(3, 0, 0),
            new TimeSpan(4, 0, 0),
            new TimeSpan(5, 0, 0),
            new TimeSpan(6, 0, 0),
        };

        #endregion

        #region fields

        const string ShutdownPath = @"C:\Windows\System32\shutdown";
        bool _isEnabledClosureTime = true;
        TimeSpan _nowTime;
        TimeSpan _remainingTime;
        TimeSpan _closureTime;

        #endregion

        #region properties

        /// <summary>
        /// Active or desactive edition on the TimeSpanPicker for closure time
        /// </summary>
        public bool IsEnabledClosureTime
        {
            get => _isEnabledClosureTime;
            set
            {
                _isEnabledClosureTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Timer with 1 second intervall call when the user confirm the time or
        /// click on a shortcut buttons
        /// </summary>
        public Timer RemainingTimer { get; }

        /// <summary>
        /// Timer with 1 second interval
        /// </summary>
        public Timer NowTimer { get; }

        /// <summary>
        /// Remaining time for shutdown
        /// </summary>
        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set
            {
                RemainingTimer.Enabled = value.TotalMilliseconds != 0;
                _remainingTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The current time
        /// </summary>
        public TimeSpan NowTime
        {
            get => _nowTime;
            set
            {
                _nowTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The closure time when the pc is shutdown
        /// </summary>
        public TimeSpan ClosureTime
        {
            get => _closureTime;
            set
            {
                _closureTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region commands

        public ICommand ShutdownCommand { get; }
        public ICommand RebootCommand { get; }
        public ICommand SetRemainingTimeCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand ResetCommand { get; }

        #endregion

        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region constructors

        /// <summary>
        /// Construct MainViewModel
        /// </summary>
        public MainViewModel()
        {
            // Create commands
            ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
            ResetCommand = new RelayCommand(Reset, CanReset);
            ShutdownCommand = new RelayCommand(Shutdown);
            RebootCommand = new RelayCommand(Restart);
            SetRemainingTimeCommand = new RelayCommand<TimeSpan>(SetRemainingTime);

            // Construct the Now timer and enabled it
            NowTime = DateTime.Now.TimeOfDay;
            NowTimer = new Timer(1000);
            NowTimer.Elapsed += NowTimer_Elapsed;
            NowTimer.Enabled = true;
            
            // Construct the Remaining Timer
            RemainingTimer = new Timer(1000);
            RemainingTimer.Elapsed += RemainingTimer_Elapsed;        
        }

        #endregion

        #region Elapsed timer

        /// <summary>
        /// Call when the remaining timer is enabled and shutdown
        /// if the remaining time is finished or substract 1 second
        /// </summary>
        /// <param name="sender">The RemainingTimer</param>
        /// <param name="e">the event</param>
        private void RemainingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (RemainingTime.TotalMilliseconds <= 0)
            {
                RemainingTimer.Enabled = false;
                Shutdown();
            }
            else
            {
                RemainingTime -= new TimeSpan(0, 0, 1);
            }
        }

        /// <summary>
        /// Call each seconds to update the now time string
        /// </summary>
        /// <param name="sender">the timer</param>
        /// <param name="e">the elapsed event args</param>
        void NowTimer_Elapsed(object sender, ElapsedEventArgs e)
            => NowTime = DateTime.Now.TimeOfDay;

        #endregion

        #region Raise Property Changed

        /// <summary>
        /// Raise property changed
        /// </summary>
        /// <param name="propertyName">the property name to raise</param>
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Commands implementation

        /// <summary>
        /// Shutdown the computer
        /// </summary>
        void Shutdown() => Process.Start(ShutdownPath, "-s -t 0");

        /// <summary>
        /// Restart the computer
        /// </summary>
        void Restart() => Process.Start(ShutdownPath, "-r -t 0");

        /// <summary>
        /// Can confirm check if the remaining time is not enabled and if closure time
        /// is greather than the now time of day
        /// </summary>
        /// <returns>true if we can invoke confirm else false</returns>
        bool CanConfirm() => !RemainingTimer.Enabled;

        /// <summary>
        /// Enabled the remaining timer
        /// </summary>
        void Confirm()
        {
            IsEnabledClosureTime = false;
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            TimeSpan hour24 = new TimeSpan(24, 0, 0);
            RemainingTime = timeOfDay < ClosureTime ?
                ClosureTime - timeOfDay : (hour24 - timeOfDay) + ClosureTime;
            RemainingTimer.Enabled = true;
        }

        /// <summary>
        /// Reset the remaining time
        /// </summary>
        void Reset()
        {
            RemainingTime = new TimeSpan();
            IsEnabledClosureTime = true;
        }

        /// <summary>
        /// Can reset if the remaining timer is enabled
        /// </summary>
        /// <returns>true if the remaining timer is enabled false otherwise</returns>
        bool CanReset() => RemainingTimer.Enabled;

        /// <summary>
        /// Change the remaining time
        /// </summary>
        /// <param name="minutes">the number of minutes to set to the remaining time</param>
        void SetRemainingTime(TimeSpan time)
        {
            ClosureTime = DateTime.Now.TimeOfDay + time;
            Confirm();
        }

        #endregion
    }
}
