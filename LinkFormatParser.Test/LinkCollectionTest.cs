using System.Collections.Generic;
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

            CheckLink(l, "/sensors/temp", new KeyValuePair<string, string>("if", "sensor"));
            CheckLink(l, "/sensors/light", new KeyValuePair<string, string>("if", "sensor"));
        }

        [TestMethod]
        public void ParseManyLinksAndAttributes()
        {
            const string DOCUMENT = "</sensors>;ct=40;title=\"Sensor Index\",</sensors/temp>;rt=\"temperature-c\";if=\"sensor\",</sensors/light>;rt=\"light-lux\";if=\"sensor\",<http://www.example.com/sensors/t123>;anchor=\"/sensors/temp\";rel=\"describedby\",</t>;anchor=\"/sensors/temp\";rel=\"alternate\"";

            LinkCollection l = new LinkCollection(DOCUMENT);

            Assert.AreEqual(5, l.Count());

            CheckLink(l, "/sensors", new KeyValuePair<string, string>("ct", "40"), new KeyValuePair<string, string>("title", "Sensor Index"));
            CheckLink(l, "/sensors/temp", new KeyValuePair<string, string>("rt", "temperature-c"), new KeyValuePair<string, string>("if", "sensor"));
            CheckLink(l, "/sensors/light", new KeyValuePair<string, string>("rt", "light-lux"), new KeyValuePair<string, string>("if", "sensor"));
        }

        [TestMethod]
        public void NodeBastetWellKnownCore()
        {
            const string DOC = "</.well-known>;rt=\"0\",</slug>;rt=\"1\",</hostname>;rt=\"2\",</ip>;rt=\"3\",</cpu>;rt=\"4\",</cpu-percentage>;rt=\"5\",</memory>;rt=\"6\",</type>;rt=\"7\",</platform>;rt=\"8\",</arch>;rt=\"9\",</release>;rt=\"10\",</uptime>;rt=\"11\",</load>;rt=\"12\",</network>;rt=\"13\"";

            var c = new LinkCollection(DOC);

            CheckLink(c, "/.well-known", new KeyValuePair<string, string>("rt", "0"));
            CheckLink(c, "/slug", new KeyValuePair<string, string>("rt", "1"));
            CheckLink(c, "/hostname", new KeyValuePair<string, string>("rt", "2"));
            CheckLink(c, "/ip", new KeyValuePair<string, string>("rt", "3"));
            CheckLink(c, "/cpu", new KeyValuePair<string, string>("rt", "4"));
            CheckLink(c, "/cpu-percentage", new KeyValuePair<string, string>("rt", "5"));
            CheckLink(c, "/memory", new KeyValuePair<string, string>("rt", "6"));
            CheckLink(c, "/type", new KeyValuePair<string, string>("rt", "7"));
            CheckLink(c, "/platform", new KeyValuePair<string, string>("rt", "8"));
            CheckLink(c, "/arch", new KeyValuePair<string, string>("rt", "9"));
            CheckLink(c, "/release", new KeyValuePair<string, string>("rt", "10"));
            CheckLink(c, "/uptime", new KeyValuePair<string, string>("rt", "11"));
            CheckLink(c, "/load", new KeyValuePair<string, string>("rt", "12"));
            CheckLink(c, "/network", new KeyValuePair<string, string>("rt", "13"));
        }

        private static void CheckLink(LinkCollection c, string uri, params KeyValuePair<string, string>[] attributes)
        {
            var l = c[uri];
            Assert.IsNotNull(l);

            foreach (KeyValuePair<string, string> attr in attributes)
                Assert.AreEqual(attr.Value, l.Attributes[attr.Key]);
        }
    }
}
