using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCP.MachineLearningModelClient
{
    public class ModelInput
    {
        public int speed_limit;
        public float seconds;
        public float air_temperature;
        public int wind_direction;
        public int wind_speed_ms;
        public bool segangle;
        public int time;
        public bool weekend;
        public bool drifting;
        public bool dry;
        public bool fog;
        public bool freezing;
        public bool none;
        public bool snow;
        public bool thunder;
        public bool wet;
        public bool living_street;
        public bool motorway;
        public bool motorway_link;
        public bool primary;
        public bool residential;
        public bool secondary;
        public bool secondary_link;
        public bool service;
        public bool tertiary;
        public bool track;
        public bool trunk;
        public bool trunk_link;
        public bool unclassified;
        public bool unpaved;
    }
}
