﻿using System;
using System.Diagnostics;

namespace Plugin.Geofence
{
  /// <summary>
  /// Cross platform Geofence implemenations
  /// </summary>
  public class CrossGeofence
  {
    static Lazy<IGeofence> Implementation = new Lazy<IGeofence>(() => CreateGeofence(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Checks if plugin is initialized
        /// </summary>
        public static bool IsInitialized { get { return (GeofenceListener != null); } }
        /// <summary>
        /// Plugin id
        /// </summary>
        public const string Id = "CrossGeofence";
        /// <summary>
        /// Geofence state events listener
        /// </summary>
        public static IGeofenceListener GeofenceListener { get; private set; }
        /// <summary>
        /// Geofence location accuracy priority
        /// </summary>
        public static GeofencePriority GeofencePriority { get; set; }

        /// <summary>
        /// Smallest displacement for location updates
        /// </summary>
        public static float SmallestDisplacement { get; set; }

        /// <summary>
        /// Request the user for Notifications Permission.  Set to false if this is already handled in the client application.
        /// </summary>
        /// <value><c>true</c> if request notification permission; otherwise, <c>false</c>.</value>
        public static bool RequestNotificationPermission { get; set; }

        /// <summary>
        /// Request the user for Location Services Permissions. Set to false if this is already handled in the client application. 
        /// </summary>
        /// <value><c>true</c> if request location permission; otherwise, <c>false</c>.</value>
        public static bool RequestLocationPermission { get; set; }

#if MONOANDROID
      /// <summary>
      /// Icon resource used for notification
      /// </summary>
      public static int IconResource { get; set; }
      /// <summary>
      /// ARGB Color used for notification
      /// </summary>
      public static int Color { get; set; }
      /// <summary>
      /// Large icon resource used for notification
      /// </summary>
      public static Android.Graphics.Bitmap LargeIconResource { get; set; }
      /// <summary>
      /// Sound for notification
      /// </summary>
      public static Android.Net.Uri SoundUri { get; set; }
      /// <summary>
      /// Location updates internal
      /// </summary>
      public static int LocationUpdatesInterval { get; set; }
      /// <summary>
      /// Fastest location updates interval
      /// </summary>
      public static int FastestLocationUpdatesInterval { get; set; }
#endif

        /// <summary>
        /// Initializes geofence plugin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="priority"></param>
        /// <param name="smallestDisplacement"></param>
        /// <param name="requestNotificationPermission"></param>
        /// <param name="requestLocationPermission"></param>
        public static void Initialize<T>(GeofencePriority priority = GeofencePriority.BalancedPower, float smallestDisplacement = 0, bool requestNotificationPermission = true, bool requestLocationPermission = true)
     where T : IGeofenceListener, new()
        {
            if (GeofenceListener == null)
            {

                GeofenceListener = (IGeofenceListener)Activator.CreateInstance(typeof(T));
                Debug.WriteLine("Geofence plugin initialized.");
            }
            else
            {
                Debug.WriteLine("Geofence plugin already initialized.");
            }
            GeofencePriority = priority;
            SmallestDisplacement = smallestDisplacement;

            RequestNotificationPermission = requestNotificationPermission;
            RequestLocationPermission = requestLocationPermission;
        }
        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IGeofence Current
        {
            get
            {
                //Should always initialize plugin before use
                if (!CrossGeofence.IsInitialized)
                {
                    throw GeofenceNotInitializedException();
                }
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;

            }
        }

    static IGeofence CreateGeofence()
    {

#if NETSTANDARD2_0
            return null;
#else
            System.Diagnostics.Debug.WriteLine("Creating GeofenceImplementation");
            var geofenceImplementation = new GeofenceImplementation();
            geofenceImplementation.RequestNotificationPermission = RequestNotificationPermission;
            geofenceImplementation.RequestLocationPermission = RequestLocationPermission;
            return geofenceImplementation;
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
        internal static GeofenceNotInitializedException GeofenceNotInitializedException()
        {
            string description = string.Format("{0} - {1}", CrossGeofence.Id, "Plugin is not initialized. Should initialize before use with CrossGeofence Initialize method. Example:  CrossGeofence.Initialize<CrossGeofenceListener>()");

            return new GeofenceNotInitializedException(description);
        }
    }
}
