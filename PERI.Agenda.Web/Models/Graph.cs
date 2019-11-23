using System.Collections.Generic;

namespace PERI.Agenda.Web.Models
{
    public class Graph
    {
        public class Statistics
        {
            public int[] Values { get; set; }
            public string[] Labels { get; set; }
        }

        public class GraphData
        {
            public int[] Data { get; set; }
            public string Label { get; set; }
        }

        public class GraphDataSet
        {
            public List<GraphData> DataSet { get; set; }
            public string[] ChartLabels { get; set; }
        }
    }
}
