using System.Text;

namespace MetaDataParser.Parsers
{
    public class ParseResult
    {
        public bool Error { get; private set; } = false;
        public List<Exception> Errors { get; private set; } = [];
        public StringBuilder Messages { get; private set; } = new();
    }
}
