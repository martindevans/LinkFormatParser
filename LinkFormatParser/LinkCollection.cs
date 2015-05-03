using System.Collections;
using System.Collections.Generic;

namespace LinkFormatParser
{
    public class LinkCollection
        : IEnumerable<Link>
    {
        private readonly List<Link> _links = new List<Link>();

        public LinkCollection(string linkFormat)
        {
            linkFormat = linkFormat.Replace("\n", "");

            int start = 0;
            while (start < linkFormat.Length - 1)
                _links.Add(new Link(linkFormat, ref start));
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
