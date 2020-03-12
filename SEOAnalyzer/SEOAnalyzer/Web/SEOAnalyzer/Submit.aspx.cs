using mshtml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SEOAnalyzerLogic;
using SEOBO;

namespace SEOAnalyzer.Web.SEOAnalyzer
{
    public partial class Submit : System.Web.UI.Page
    {
        Boolean isEng = false;
        List<string> stopWordList = null;
        ISeoSubmitLogic seoSubmitLogic = SEOSubmitLogic.GetInstance;

        protected void Page_Load(object sender, EventArgs e)
        {
            divSuccess.Visible = false;
            divSuccess.InnerHtml = "";
            divError.Visible = false;
            divError.InnerHtml = "";

            stopWordList = seoSubmitLogic.ReadStopWordFile(ConfigurationManager.AppSettings["StopWordLocation"]);
       
        }

        private bool PassValidation()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtTextToSearch.Text.Trim()) && string.IsNullOrEmpty(txtUrlToSearch.Text.Trim()))
            {
                divError.Visible = true;
                divError.InnerHtml = "Please enter text.";
                return false;
            }

            if (stopWordList == null)
            {
                divError.Visible = true;
                divError.InnerHtml = "Stopword list not found.";
                return false;
            }
            return isValid;
        }

        public void Reset()
        {
            txtResult.Text = string.Empty;
            txtTextToSearch.Text = string.Empty;
            txtUrlToSearch.Text = string.Empty;

            pnlListView.Visible = false;
            pnlIframe.Visible = false;

            lvResultEachWord.Visible = false;
            lvResultEachWord.DataSource = null;
            lvResultEachWord.DataBind();

            lvMetatag.Visible = false;
            lvMetatag.DataSource = null;
            lvMetatag.DataBind();

            lvExternalLink.Visible = false;
            lvExternalLink.DataSource = null;
            lvExternalLink.DataBind();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                if (!PassValidation())
                {
                    return;
                }

                if (!string.IsNullOrEmpty(txtTextToSearch.Text.Trim()))
                {
                    //user submit eng text
                    isEng = true;
                }
                else
                {
                    // user submit URL
                    isEng = false;
                }

                // user submit url
                if (isEng == false)
                {
                    //Not enabled SEO
                    if(chkSEO.Checked == false)
                    {
                        pnlListView.Visible = false;
                        pnlIframe.Visible = true;

                        // show textresult
                        txtResult.Visible = true;
                        txtResult.Text = seoSubmitLogic.GetContentString(txtUrlToSearch.Text.Trim());

                    }
                    else
                    {
                        pnlListView.Visible = true;
                        pnlIframe.Visible = false;

                        //Enabled SEO            

                        List<string> listOfRawText = seoSubmitLogic.GetHtmlComponentlist(txtUrlToSearch.Text.Trim(), SEOAnalyzerLogic.Type.Text,
                            SEOAnalyzerLogic.SearchType.URL);

                        lvResultEachWord.DataSource = null;
                        if (listOfRawText != null)
                        {
                            if(listOfRawText .Count > 0)
                            {
                                var resultBoList = seoSubmitLogic.GetSEOResult(listOfRawText, stopWordList);
                                
                                lvResultEachWord.DataSource = resultBoList;
                                
                            }
                        }
                        lvResultEachWord.Visible = true;
                        lvResultEachWord.DataBind();

                        List<string> listOfMetadataText = seoSubmitLogic.GetHtmlComponentlist(txtUrlToSearch.Text.Trim(), SEOAnalyzerLogic.Type.Metadata, 
                            SEOAnalyzerLogic.SearchType.URL);

                        lvMetatag.DataSource = null;
                        if (listOfMetadataText != null)
                        {
                            if(listOfMetadataText.Count > 0)
                            {
                                var resultMetaList = seoSubmitLogic.GetSEOResult(listOfMetadataText, stopWordList);
                                lvMetatag.DataSource = resultMetaList;
                                
                            }
                        }
                        lvMetatag.Visible = true;
                        lvMetatag.DataBind();

                        List<string> listOfAnchorText = seoSubmitLogic.GetHtmlComponentlist(txtUrlToSearch.Text.Trim(), SEOAnalyzerLogic.Type.Anchor,
                            SEOAnalyzerLogic.SearchType.URL);

                        lvExternalLink.DataSource = null;
                        if (listOfAnchorText != null)
                        {
                            if(listOfAnchorText.Count > 0)
                            {
                                var resultAnchorList = seoSubmitLogic.GetSEOResult(listOfAnchorText, stopWordList);
                                lvExternalLink.DataSource = resultAnchorList;   
                            }
                        }
                        lvExternalLink.Visible = true;
                        lvExternalLink.DataBind();

                        txtTextToSearch.Visible = false;
                        txtTextToSearch.Text = string.Empty;
                    }
                }
                else
                {
                    // user submit eng text

                    //Not enabled SEO
                    if (chkSEO.Checked == false)
                    {
                        pnlListView.Visible = false;
                        pnlIframe.Visible = true;

                        //show textbox

                        txtResult.Visible = true;
                        txtResult.Text = txtTextToSearch.Text.Trim();

                    }
                    else
                    {
                        pnlListView.Visible = true;
                        pnlIframe.Visible = false;

                        //Enabled SEO

                        List<string> listOfRawText = seoSubmitLogic.GetHtmlComponentlist(txtTextToSearch.Text.Trim(), SEOAnalyzerLogic.Type.Text, SEOAnalyzerLogic.SearchType.EngText);
                        lvResultEachWord.DataSource = null;
                        if (listOfRawText != null)
                        {
                            if (listOfRawText.Count > 0)
                            {
                                var resultBoList = seoSubmitLogic.GetSEOResult(listOfRawText, stopWordList);
                                lvResultEachWord.DataSource = resultBoList;
                            }
                            
                        }
                        lvResultEachWord.Visible = true;
                        lvResultEachWord.DataBind();

                        List<string> listOfMetadataText = seoSubmitLogic.GetHtmlComponentlist(txtTextToSearch.Text.Trim(), SEOAnalyzerLogic.Type.Metadata, SEOAnalyzerLogic.SearchType.EngText);
                        lvMetatag.DataSource = null;
                        if (listOfMetadataText != null)
                        {
                            if (listOfMetadataText.Count > 0)
                            {
                                var resultMetaList = seoSubmitLogic.GetSEOResult(listOfMetadataText, stopWordList);
                                lvMetatag.DataSource = resultMetaList;

                            }
                        }
                        lvMetatag.Visible = true;
                        lvMetatag.DataBind();

                        List<string> listOfAnchorText = seoSubmitLogic.GetHtmlComponentlist(txtTextToSearch.Text.Trim(), SEOAnalyzerLogic.Type.Anchor, SEOAnalyzerLogic.SearchType.EngText);

                        lvExternalLink.DataSource = null;
                        if (listOfAnchorText != null)
                        {
                            if (listOfAnchorText.Count > 0)
                            {
                                var resultAnchorList = seoSubmitLogic.GetSEOResult(listOfAnchorText, stopWordList); 
                                lvExternalLink.DataSource = resultAnchorList;
                            }
                        }
                        lvExternalLink.Visible = true;
                        lvExternalLink.DataBind();
                    }

                }


            }
            catch (Exception ex)
            {
                //log4net
                divError.Visible = true;
                divError.InnerHtml = ex.Message;

                lvResultEachWord.DataSource = null;
                lvResultEachWord.DataBind();

                lvMetatag.DataSource = null;
                lvMetatag.DataBind();

                lvExternalLink.DataSource = null;
                lvExternalLink.DataBind();

                return;
            }
        }

        protected void ddlSEO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlSEO.SelectedValue =="1")
            {
                txtTextToSearch.Visible = true;
                txtTextToSearch.Text  = "";
                divText.Visible = true;
                txtUrlToSearch.Visible = false;
                txtUrlToSearch.Text = "";
                divUrl.Visible = false;
            }
            else  
            {
                txtTextToSearch.Visible = false;
                txtTextToSearch.Text = "";
                divText.Visible = false;
                txtUrlToSearch.Visible = true;
                txtUrlToSearch.Text = "";
                divUrl.Visible = true;
            }
        }
    }


}