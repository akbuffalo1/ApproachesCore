#if _CALENDARS_

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Plugins.Calendars
{
	public interface ICalendars
	{
		Task<bool> RequestPermission();

	    /// <summary>
	    /// Gets a list of all calendars on the device.
	    /// </summary>
	    /// <returns>Calendars</returns>
	    /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
	    /// <exception cref="Plugin.Calendars.Abstractions.PlatformException">Unexpected platform-specific error</exception>
	    Task<IEnumerable<Calendar>> GetCalendars();

        /// <summary>
        /// Gets a single calendar by platform-specific ID.
        /// </summary>
        /// <param name="externalId">Platform-specific calendar identifier</param>
        /// <returns>The corresponding calendar, or null if not found</returns>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        /// <exception cref="Plugin.Calendars.Abstractions.PlatformException">Unexpected platform-specific error</exception>
        Task<Calendar> GetCalendar(string externalId);

        /// <summary>
        /// Gets all events for a calendar within the specified time range.
        /// </summary>
        /// <param name="calendar">Calendar containing events</param>
        /// <param name="start">Start of event range</param>
        /// <param name="end">End of event range</param>
        /// <returns>Calendar events</returns>
        /// <exception cref="System.ArgumentException">Calendar does not exist on device</exception>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        Task<IEnumerable<CalendarEvent>> GetEvents(Calendar calendar, DateTime start, DateTime end);

        /// <summary>
        /// Gets a single calendar event by platform-specific ID.
        /// </summary>
        /// <param name="externalId">Platform-specific calendar event identifier</param>
        /// <returns>The corresponding calendar event, or null if not found</returns>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        Task<CalendarEvent> GetEvent(string externalId);


        /// <summary>
        /// Creates a new calendar or updates the name and color of an existing one.
        /// </summary>
        /// <param name="calendar">The calendar to create/update</param>
        /// <exception cref="System.ArgumentException">Calendar does not exist on device or is read-only</exception>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        /// <exception cref="System.InvalidOperationException">No active calendar sources available to create calendar on (iOS-only)</exception>
        Task AddOrUpdateCalendar(Calendar calendar);

        /// <summary>
        /// Add new event to a calendar or update an existing event.
        /// </summary>
        /// <param name="calendar">Destination calendar</param>
        /// <param name="calendarEvent">Event to add or update</param>
        /// <exception cref="System.ArgumentException">Calendar is not specified, does not exist on device, or is read-only</exception>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        /// <exception cref="System.InvalidOperationException">Editing recurring events is not supported</exception>
        Task AddOrUpdateEvent(Calendar calendar, CalendarEvent calendarEvent);


        /// <summary>
        /// Removes a calendar and all its events from the system.
        /// </summary>
        /// <param name="calendar">Calendar to delete</param>
        /// <returns>True if successfully removed</returns>
        /// <exception cref="System.ArgumentException">Calendar is read-only</exception>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        Task<bool> DeleteCalendar(Calendar calendar);

        /// <summary>
        /// Removes an event from the specified calendar.
        /// </summary>
        /// <param name="calendar">Calendar to remove event from</param>
        /// <param name="calendarEvent">Event to remove</param>
        /// <returns>True if successfully removed</returns>
        /// <exception cref="System.ArgumentException">Calendar is read-only</exception>
        /// <exception cref="System.UnauthorizedAccessException">Calendar access denied</exception>
        /// <exception cref="System.InvalidOperationException">Editing recurring events is not supported</exception>
        Task<bool> DeleteEvent(Calendar calendar, CalendarEvent calendarEvent);

        /// <summary>
        /// iOS/Android: Adds a reminder to the specified calendar event
        /// Windows: Sets/replaces the reminder for the specified calendar event
        /// </summary>
        /// <param name="calendarEvent">Event to add</param>
        /// <param name="reminder">Reminder to add</param>
        /// <returns>If successful</returns>
        /// <exception cref="ArgumentException">Calendar event is not created or not valid</exception>
        /// <exception cref="System.InvalidOperationException">Editing recurring events is not supported</exception>
        Task<bool> AddEventReminder(CalendarEvent calendarEvent, CalendarEventReminder reminder);
    }
}

#endif
