using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace LinkFormatParser.Test
{
    [TestClass]
    public class LinkTest
    {
        [TestMethod]
        public void LinkUriIsParsed()
        {
            int a = 0;
            var link = new Link("</sensors>;rt=\"stuff,things;punctuation\\\"even quotes\";ct=40", ref a);

            Assert.AreEqual("/sensors", link.Uri);

            Assert.AreEqual("stuff,things;punctuation\\\"even quotes", link.Attributes["rt"]);
            Assert.AreEqual("40", link.Attributes["ct"]);
        }

        readonly Regex _roshegex = new Regex("(\"(?<value>.*?)((?<!\\\\)(?:\\\\\\\\)*\"))|((?<value>[0-9]+?)(,|;|$))");

        [TestMethod]
        public void ParseNonQuotedAttributes()
        {
            const string CT = "ct=40";

            var v = _regex.Match(CT);
            Assert.IsTrue(v.Success);

            var val = v.Groups["value"].Value;

            Console.WriteLine(val);
            Assert.AreEqual("40", val);
        }

        [TestMethod]
        public void ParseQuotedAttributes()
        {
            const string RT = "rt=\"stuff,things;punctuation\\\"even quotes\"";

            var v = _regex.Match(RT);
            Assert.IsTrue(v.Success);

            var val = v.Groups["value"].Value;

            Console.WriteLine(val);
            Assert.AreEqual("stuff,things;punctuation\\\"even quotes", val);
        }

        [TestMethod]
        public void ParseTwoAttributes()
        {
            const string ATTR = "rt=\"stuff,things;punctuation\\\"even quotes\";ct=40,";

            var sub = ATTR.Substring(3);
            var v = _regex.Match(ATTR, 3);
            Assert.IsTrue(v.Success);

            var val = v.Groups["value"].Value;

            Console.WriteLine(val);
            Assert.AreEqual("stuff,things;punctuation\\\"even quotes", val);
        }
    }
}
