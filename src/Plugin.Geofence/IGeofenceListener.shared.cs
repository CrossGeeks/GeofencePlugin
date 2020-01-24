using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Geofence
{
    public interface IGeofenceListener
    {
       void OnMonitoringStarted(string identifier);
       void OnMonitoringStopped();
       void OnMonitoringStopped(string identifier);
       void OnRegionStateChanged(GeofenceResult result);
       void OnError(String error);
       void OnAppStarted();
       void OnLocationChanged(GeofenceLocation location);
    }
}
