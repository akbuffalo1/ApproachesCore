﻿#if _CALENDARS_

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Plugins.Calendars
{
    /// <summary>
    /// Calendar reminder that happens before the event such as an alert
    /// </summary>
    public class CalendarEventReminder
    {
        /// <summary>
        /// Amount of time to set the reminder before the start of an event.
        /// Default is 15 minutes
        /// </summary>
        public TimeSpan TimeBefore { get; set; } = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Type of reminder to display on Android. (not used by Windows/iOS)
        /// </summary>
        public CalendarReminderMethod Method { get; set; } = CalendarReminderMethod.Default;
    }

    /// <summary>
    /// Android reminder methods
    /// </summary>
    public enum CalendarReminderMethod
    {
        /// <summary>
        /// Use system default
        /// </summary>
        Default,
        /// <summary>
        /// Pop up alert
        /// </summary>
        Alert,
        /// <summary>
        /// Send an email
        /// </summary>
        Email,
        /// <summary>
        /// Send an sms
        /// </summary>
        Sms
    }
}

#endif