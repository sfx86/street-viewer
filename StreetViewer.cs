using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StreetViewer
{
    internal class StreetViewer : MapTool
    {
        public StreetViewer()
        {
            //
        }
        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                e.Handled = true;
            }
        }
        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            return QueuedTask.Run(() =>
            {
                var point = ActiveMapView.ClientToMap(e.ClientPoint);
                var coords = GeometryEngine.Instance.Project(point, SpatialReferences.WGS84) as MapPoint;
                if (coords == null) { 
                    return;
                };

                // q=     : query
                // layer= : 't' -> traffic, 'c' -> street view, or both
                // cbll=  : lat, long
                // cbp=   : map layout: 11 -> lower half map, 12 -> corner map
                //        : bearing
                //        : tilt
                //        : zoom level (0..2)
                //        : pitch (defaults to 5 deg)
                var url = $"http://maps.google.com/maps?q=&layer=c&cbll={coords.Y},{coords.X}&cbp=12,0,0,0,0";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            });
        }
    }
}
