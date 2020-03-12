using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SEOAnalyzerLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using SEOAnalyzerLogic;
using System.IO.Fakes;
using SEOBO;
using System.Net.Fakes;

namespace UnitTestSeoAnalyzer
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReadStopWordFile()
        {
            //Arange
            List<string> actual = new List<string>();
            string stopWord = "about";
            string bStopWord = "a";

            List<string> expected = new List<string>();
            expected.Add(stopWord);
            expected.Add(bStopWord);

            using (ShimsContext.Create())
            {
                ShimSEOSubmitLogic.AllInstances.ReadStopWordFileString = (a, b) => { return expected; };

                actual = SEOAnalyzerLogic.SEOSubmitLogic.GetInstance.ReadStopWordFile("");
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TestcountOfString()
        {
            //Arange
            List<string> textList = new List<string>();
            string aWord = "abc";
            string bWord = "def";
            textList.Add(aWord);
            textList.Add(bWord);

            int expected = 1;

            var actual = SEOSubmitLogic.GetInstance.countOfString("abc", textList);

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestGetSEOResult()
        {
            //Arange
            List<string> stopWordList = new List<string>();
            string stopWord = "about";

            stopWordList.Add(stopWord);



            List<string> contentList = new List<string>();
            string aWord = "kein";

            contentList.Add(aWord);


            int count = 1;

            List<ResultBO> expected = new List<ResultBO>()
            {
                new ResultBO()
                {
                     Word = "kein",
                    Count = 1


                }
            };
           


            using (ShimsContext.Create())
            {
                ShimSEOSubmitLogic.AllInstances.countOfStringStringListOfString = (a, b, c) => { return count; };

                var actual = SEOSubmitLogic.GetInstance.GetSEOResult(contentList, stopWordList);

                Assert.AreEqual(expected.Count, actual.Count);

            }
        }


        [TestMethod]
        public void TestGetContentString()
        {
            //Arange

            string expected = "abc";

            using (ShimsContext.Create())
            {
                ShimStreamReader.AllInstances.ReadToEnd = (a) => { return expected; };
               

                var actual = SEOSubmitLogic.GetInstance.GetContentString("https://www.google.com");

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TestGetHtmlComponentlist()
        {
            //Arange
            List<string> expected = new List<string>();

            string astring = "abc";
            expected.Add(astring);



            var actual = SEOSubmitLogic.GetInstance.GetHtmlComponentlist(astring, SEOAnalyzerLogic.Type.Text, SearchType.EngText);

            Assert.AreEqual(expected.Count, actual.Count);

        }
    }
}
