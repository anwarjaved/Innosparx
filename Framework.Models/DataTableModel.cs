namespace Framework.Models
{
    using Newtonsoft.Json;

    public class DataTableModel 
    {
        public DataTableModel(string echo, int totalCount, int totalFilterCount, object items)
        {
            this.Echo = echo;
            this.TotalCount = totalCount;
            this.TotalFilterCount = totalFilterCount;
            this.Items = items;
        }

        [JsonProperty("sEcho")]
        public string Echo { get; private set; }

        [JsonProperty("iTotalRecords")]
        public int TotalCount { get; private set; }

        [JsonProperty("iTotalDisplayRecords")]
        public int TotalFilterCount { get; private set; }

        [JsonProperty("aaData")]
        public object Items { get; private set; }
    }
}
