using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flixte.APIV2.Models
{
    public class StationListReturn
    {
        public int StationID { get; set; }
        public string Name { get; set; }
        public string NextVideoTitle { get; set; }
        public string NextVideoDuration { get; set; }
        public string NextVideoThumbURL { get; set; }
    }
    public class PlayListReturn
    {
        public int PlayListID { get; set; }
        public string Name { get; set; }
        public string NextVideoTitle { get; set; }
        public string NextVideoDuration { get; set; }
        public string NextVideoThumbURL { get; set; }
    }
    public class StationsByCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<StationListReturn> Stations { get; set; }
    }

    public class PlaylistsByCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<PlayListReturn> PlayLists { get; set; }
    }

    public class HomeReturn
    {
        public List<StationsByCategory> StationsByCategory { get; set; }
        public List<PlayListReturn> PlaylistsByCategory { get; set; }
        public List<PlayListReturn> FeaturedPlaylists { get; set; }
    }

}