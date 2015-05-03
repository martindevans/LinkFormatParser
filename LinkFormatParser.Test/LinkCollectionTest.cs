using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LinkFormatParser.Test
{
    [TestClass]
    public class LinkCollectionTest
    {
        [TestMethod]
        public void ParseSingleLink()
        {
            const string DOCUMENT = "</sensors/temp>;if=\"sensor\"";

            LinkCollection l = new LinkCollection(DOCUMENT);

            Assert.AreEqual(1, l.Count());
            Assert.AreEqual("/sensors/temp", l.Single().Uri);
            Assert.AreEqual("sensor", l.Single().Attributes.Single().Value);
        }

        [TestMethod]
        public void ParseMultipleLinks()
        {
            const string DOCUMENT = "</sensors/temp>;if=\"sensor\",</sensors/light>;if=\"sensor\"";

            LinkCollection l = new LinkCollection(DOCUMENT);

            Assert.AreEqual(2, l.Count());
            Assert.IsTrue(l.Any(a => a.Uri == "/sensors/temp"));
            Assert.IsTrue(l.Any(a => a.Uri == "/sensors/light"));
        }

        [TestMethod]
        public void ParseManyLinksAndAttributes()
        {
            const string DOCUMENT = "</sensors>;ct=40;title=\"Sensor Index\",</sensors/temp>;rt=\"temperature-c\";if=\"sensor\",</sensors/light>;rt=\"light-lux\";if=\"sensor\",<http://www.example.com/sensors/t123>;anchor=\"/sensors/temp\";rel=\"describedby\",</t>;anchor=\"/sensors/temp\";rel=\"alternate\"";

            LinkCollection l = new LinkCollection(DOCUMENT);

            Assert.AreEqual(5, l.Count());

            var fail = from link in l
                       from attr in link.Attributes
                       let key = attr.Key
                       where key.Contains("<")
                       select key;
            Assert.IsFalse(fail.Any());
        }
    }
}
