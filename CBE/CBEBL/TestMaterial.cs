namespace CBEBL
{
    /// <summary>
    /// This object hold data for a quote. 
    /// </summary>
    /// <value></value>
    public class TestMaterial
    {
        public TestMaterial(string content, string author, int length)
        {
            this.content = content;
            this.author = author;
            this.length = length;
        }

        public string content { get; set; }
        public string author { get; set; }
        public int length { get; set; }
        public int categoryId { get; set; }
    }
}