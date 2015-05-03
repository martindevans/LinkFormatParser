using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LinkFormatParser
{
    public class Link
    {
        public struct AttributeCollection
            : IEnumerable<KeyValuePair<string, string>> 
        {
            private readonly Dictionary<string, string> _attributes;

            public string this[string name]
            {
                get
                {
                    string value;
                    _attributes.TryGetValue(name, out value);
                    return value;
                }
            }

            internal AttributeCollection(Dictionary<string, string> attributes)
            {
                _attributes = attributes;
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                return _attributes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly AttributeCollection _attributes;
        public AttributeCollection Attributes
        {
            get
            {
                return _attributes;
            }
        }

        private readonly string _uri;
        public string Uri
        {
            get
            {
                return _uri;
            }
        }

        public Link(string link, ref int start)
        {
            _uri = ParseUriReference(link, ref start);
            _attributes = new AttributeCollection(ParseAttributes(link, ref start));
        }

        private static readonly Regex _resourceNameRegex = new Regex(@"<[^>]*>");
        private static readonly Regex _attributeNameRegex = new Regex(@"<?\;?(?<key>.*?)=");
        private static readonly Regex _attributeValueRegex = new Regex("(\"(?<value>.*?)((?<!\\\\)(?:\\\\\\\\)*\"))|((?<value>[0-9]+?)(,|;|$))");

        private string ParseUriReference(string link, ref int start)
        {
            var m = _resourceNameRegex.Match(link, start);
            if (!m.Success)
                throw new ParseException("link", "Could not match resource name");

            start += m.Length;
            return m.Value.TrimStart('<').TrimEnd('>');
        }

        private Dictionary<string, string> ParseAttributes(string link, ref int start)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            while ((start + 1 < link.Length) && link[start] != ',')
                ParseAttribute(link, ref start, attributes);

            return attributes;
        }

        private void ParseAttribute(string link, ref int start, IDictionary<string, string> attributes)
        {
            var sub = link.Substring(start);
            var k = _attributeNameRegex.Match(link, start);
            if (!k.Success)
                throw new ParseException("link", "Could not match attribute key");

            var key = k.Groups["key"].Value;
            start += k.Length;

            var sub2 = link.Substring(start);

            var v = _attributeValueRegex.Match(link, start);
            if (!v.Success)
                throw new ParseException("link", "Could not match attribute value");

            var value = v.Groups["value"].Value;
            start += v.Length;

            attributes.Add(key, value);
        }
    }
}
