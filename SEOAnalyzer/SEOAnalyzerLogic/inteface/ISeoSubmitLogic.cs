using SEOBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEOAnalyzerLogic
{
    public interface ISeoSubmitLogic
    {
        List<string> ReadStopWordFile(string path);

        int countOfString(string searchText, List<string> textList);

        List<ResultBO> GetSEOResult(List<string> listOfRawText, List<string> stopWordList);

        string GetContentString(string Url);

        List<string> GetHtmlComponentlist(string content, Type type, SearchType searchtype);


    }
}
