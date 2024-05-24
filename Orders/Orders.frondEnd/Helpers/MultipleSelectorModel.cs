namespace Orders.frondEnd.Helpers
{
    public class MultipleSelectorModel
    {
        public MultipleSelectorModel(String key, string value)
        {
                Key = key; 
                Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }   
    }
}
