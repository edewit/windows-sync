using System;
using aerogear_windows_sync;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json.Linq;

namespace tests
{
    [TestClass]
    public class JsonMergePatchDiffTest
    {
        [TestMethod]
        public void ConstructWithNullPatch()
        {
            try
            {
                JsonMergePatchDiff.FromJToken(null);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is NullReferenceException);
            }
        }

        [TestMethod]
        public void EqualsReflective()
        {
            //given
            JsonMergePatchDiff x = JsonMergePatchDiff.FromJToken(JsonDiff());

            //then
            Assert.AreEqual(x, x);
        }

        [TestMethod]
        public void EqualsSymmetric()
        {
            //given
            var json = JsonDiff();
            JsonMergePatchDiff x = JsonMergePatchDiff.FromJToken(json);
            JsonMergePatchDiff y = JsonMergePatchDiff.FromJToken(json);

            //then
            Assert.AreEqual(x, y);
        }

        public void NonEquals()
        {
            //given
            JsonMergePatchDiff x = JsonMergePatchDiff.FromJToken(JsonDiff());
            JsonMergePatchDiff y = JsonMergePatchDiff.FromJToken(JObject.Parse(@"{'name': 'test'}"));

            //then
            Assert.AreEqual(x, y);
        }

        private static JObject JsonDiff()
        {
            return JObject.Parse(@"{ 'op': 'add', 'path': '/baz', 'value': 'qux' }");
        }
    }
}
