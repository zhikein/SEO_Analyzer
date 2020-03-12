using mshtml;
using SEOBO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SEOAnalyzerLogic
{
    public class SEOSubmitLogic: ISeoSubmitLogic
    {
        private static SEOSubmitLogic Instance = new SEOSubmitLogic();
        private SEOSubmitLogic()
        {

        }

        public static SEOSubmitLogic GetInstance
        {
            get
            {
                return Instance;
            }
        }

        /// <summary>
        /// Read stop word file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> ReadStopWordFile(string path)
        {
            try
            {   
                if(string.IsNullOrEmpty(path))
                {
                    throw new Exception("empty path");
                }

                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(path))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();

                    List<string> idList = line.Split(new[] { "\r\n" }, StringSplitOptions.None)
                                                 .ToList();
                    return idList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate count from provided list of text.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="textList"></param>
        /// <returns></returns>
        public int countOfString(string searchText, List<string> textList)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    throw new Exception("empty searchText");
                }

                if (textList == null)
                {
                    throw new Exception("empty textList");
                }
                else
                {
                    if (textList.Count < 1)
                    {
                        throw new Exception("empty textList");
                    }
                }

                string searchTerm = searchText;

                //Convert the string into an array of words  
                string[] source = textList.ToArray();

                // Create the query.  Use ToLowerInvariant to match "data" and "Data"   
                var matchQuery = from word in source
                                 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                 select word;

                // Count the matches, which executes the query.  
                int wordCount = matchQuery.Count();

                return wordCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Filter out stop words from the list of HTML string. Get count for each word.
        /// </summary>
        /// <param name="listOfRawText"></param>
        /// <param name="stopWordList"></param>
        /// <returns>List of ResultBO. ResultBO fields: Word, Count</returns>
        public List<ResultBO> GetSEOResult(List<string> listOfRawText, List<string> stopWordList)
        {
            if (listOfRawText == null)
            {
                throw new Exception("empty listOfRawText");
            }
            else
            {
                if (listOfRawText.Count < 1)
                {
                    throw new Exception("empty listOfRawText");
                }
            }

            if (stopWordList == null)
            {
                throw new Exception("empty stopWordList");
            }
            else
            {
                if (stopWordList.Count < 1)
                {
                    throw new Exception("empty stopWordList");
                }
            }

            List<string> queryresult = listOfRawText.Where(item => !stopWordList.Contains(item)).ToList();
            List<ResultBO> resultBoList = new List<ResultBO>();
            foreach (string a in queryresult)
            {
                var checking = resultBoList.Where(o => o.Word == a).FirstOrDefault();

                if (checking == null)
                {
                    ResultBO resultBO = new ResultBO();
                    resultBO.Word = a;
                    resultBO.Count = countOfString(a, queryresult);
                    resultBoList.Add(resultBO);
                }
            }

            return resultBoList;
        }

        /// <summary>
        /// Pass URL to get HTML of the page
        /// </summary>
        /// <param name="Url"></param>
        /// <returns>HTML string</returns>
        public string GetContentString(string Url)
        {
            try
            {

                if (string.IsNullOrEmpty(Url))
                {
                    throw new Exception("empty Url");
                }

                // Create a request for the URL.             
                WebRequest request = WebRequest.Create(Url);
                // If required by the server, set the credentials.    
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.    
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.    
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.    
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.    
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.    
                string responseFromServer = reader.ReadToEnd();
                // Display the content.    
                Console.WriteLine(responseFromServer);
                // Cleanup the streams and the response.    
                reader.Close();
                dataStream.Close();
                response.Close();

               

                return responseFromServer;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }


        /// <summary>
        /// The search type options are URL and English Text. 
        /// The Type options are Text, Metadata or link.
        /// The content can be URL or english text.
        /// Based on parameters passed in, the method will process and return list of string.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="searchtype"></param>
        /// <returns></returns>
        public List<string> GetHtmlComponentlist(string content, Type type, SearchType searchtype)
        {
            List<string> AllContents = new List<string>();
            var rawText = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("empty content");
                }

                HtmlDocument doc = new HtmlDocument();
                if(searchtype == SearchType.URL)
                {
                    var web = new HtmlWeb();
                    doc = web.Load(content);

                    if (web.StatusCode == HttpStatusCode.OK)
                    {
                        if (type == Type.Text)
                        {
                            AllContents.AddRange(GetTextList(doc));
                        }
                        else if (type == Type.Metadata)
                        {
                            AllContents.AddRange(GetMetadataList(doc));
                        }
                        else
                        {
                            AllContents.AddRange(GetAnchorList(doc));
                        }
                        return AllContents;
                    }
                    else if (web.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new Exception("Requested URL not found");
                    }

                    else if (web.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        throw new Exception("Requested URL Service Unavailable");
                    }

                    else if (web.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception("Requested URL internal server error");
                    }
                    else
                    {
                        throw new Exception("Requested URL Service Unavailable");
                    }
                }
                else
                {
                    doc.LoadHtml(content);

                    if (type == Type.Text)
                    {
                        AllContents.AddRange(GetTextList(doc));
                    }
                    else if (type == Type.Metadata)
                    {
                        AllContents.AddRange(GetMetadataList(doc));
                    }
                    else
                    {
                        AllContents.AddRange(GetAnchorList(doc));
                    }
                    return AllContents;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Get Metadata list from HTML doc
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<string> GetMetadataList(HtmlDocument doc)
        {
            List<string> metalist = new List<string>();
            try
            {
                if(doc == null)
                {
                    throw new Exception("empty doc");
                }

                var listmeta = doc.DocumentNode.SelectNodes("//meta");

                if(listmeta != null)
                {
                    if(listmeta.Count > 0)
                    {
                        foreach (var node in listmeta)
                        {
                            var temp = node.GetAttributeValue("content", "");

                            if (temp != string.Empty)
                            {
                                metalist.Add(temp);
                            }
                        }
                    }
                }
                
            }     
            catch(Exception ex)
            {
                throw ex;
            }
            

            return metalist;
        }

        /// <summary>
        /// Get anchor link list from HTML doc
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<string> GetAnchorList(HtmlDocument doc)
        {
            List<string> anchorlist = new List<string>();

            try
            {
                if (doc == null)
                {
                    throw new Exception("empty doc");
                }

                var listanchor = doc.DocumentNode.SelectNodes("//a");
                if (listanchor != null)
                {
                    if (listanchor.Count > 0)
                    {
                        foreach (var node in listanchor)
                        {
                            var temp = node.GetAttributeValue("href", "");

                            if (temp != string.Empty)
                            {
                                anchorlist.Add(temp);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            return anchorlist;
        }

        /// <summary>
        /// Get word list from HTML doc
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<string> GetTextList(HtmlDocument doc)
        {
            List<string> textlist = new List<string>();

            try
            {
                if (doc == null)
                {
                    throw new Exception("empty doc");
                }
                // Remove script & style nodes

                doc.DocumentNode.Descendants().Where(n => n.Name == "script" || n.Name == "style").ToList().ForEach(n => n.Remove());

                var listtext = doc.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']");

                if (listtext != null)
                {
                    if (listtext.Count > 0)
                    {
                        foreach (var node in listtext)
                        {
                            textlist.Add(node.InnerText.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            return textlist;
        }
    }

    
}

