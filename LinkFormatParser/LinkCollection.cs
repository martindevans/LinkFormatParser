using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinkFormatParser
{
    public class LinkCollection
        : IEnumerable<Link>
    {
        private readonly List<Link> _links = new List<Link>();

        public Link this[string url] {
            get
            {
                return _links.SingleOrDefault(a => a.Uri == url);
            }
        }

        public LinkCollection(string linkFormat)
        {
            linkFormat = linkFormat.Replace("\n", "");

            int start = 0;
            while (start < linkFormat.Length - 1)
            {
                _links.Add(new Link(linkFormat, ref start));

                //Consume the comma between links
                if (start < linkFormat.Length && linkFormat[start] != ',')
                    throw new ParseException("linkFormat", "Failed to find comma between links");
                start += 1;
            }
        }

        public IEnumerator<Link> GetEnumerator()
        {
            return _links.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
