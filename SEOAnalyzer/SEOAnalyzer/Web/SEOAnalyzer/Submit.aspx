<%@ Page Title="Submit - SEO Analyzer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Submit.aspx.cs" Inherits="SEOAnalyzer.Web.SEOAnalyzer.Submit" validateRequest="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("#magTable").tablesorter({
                ignoreCase: true,
                sortList: [
                    [0, 0],
                    [1, 0],
                    [2, 0]
                ],
                sortInitialOrder: 'asc'
            });

            $("#magMetaTable").tablesorter({
                ignoreCase: true,
                sortList: [
                    [0, 0],
                    [1, 0],
                    [2, 0]
                ],
                sortInitialOrder: 'asc'
            });

            $("#magAnchorTable").tablesorter({
                ignoreCase: true,
                sortList: [
                    [0, 0],
                    [1, 0],
                    [2, 0]
                ],
                sortInitialOrder: 'asc'
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">


            <div class="col-xs-12 col-md-12">
                <div style="margin: 20px"></div>
                <div class="h5">
                    Submit Text or URL here
                </div>
                <div style="margin: 10px"></div>
                <div style="background-color: red; color: white;" runat="server" id="divError">
                </div>
                <div style="background-color: greenyellow; color: black;" runat="server" id="divSuccess">
                </div>
                <div class="form-group">
                    <asp:DropDownList runat="server" Style="max-width: 1000px" ID="ddlSEO" AutoPostBack="true" OnSelectedIndexChanged="ddlSEO_SelectedIndexChanged" CssClass="form-control">
                        <asp:ListItem Text="English text" Value="1"></asp:ListItem>
                        <asp:ListItem Text="URL" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="form-group" runat="server" id="divText" visible="true">
                    <label for="txtTextToSearch">Text</label>
                    <asp:TextBox runat="server" class="form-control" Style="max-width: 1000px" ID="txtTextToSearch" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="form-group" runat="server" id="divUrl" visible="false">
                    <label for="txtUrlToSearch">URL</label>
                    <asp:TextBox runat="server" class="form-control" Style="max-width: 1000px" ID="txtUrlToSearch"></asp:TextBox>
                </div>
                <div class="form-group form-check">
                    <asp:CheckBox runat="server" ID="chkSEO" class="form-check-input" Checked="true" />
                    <label class="form-check-label" for="chkSEO">Enable SEO?</label>
                </div>
                <asp:Button runat="server" ID="btnSubmit" class="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />


                <asp:Panel runat="server" ID="pnlIframe" Style="margin: 20px" Visible="false">

                    <asp:TextBox runat="server" TextMode="MultiLine" Width="1000px" ID="txtResult" Visible="false" CssClass="form-control" Style="max-width: 100%; min-height: 800px"></asp:TextBox>


                </asp:Panel>

                <asp:Panel runat="server" ID="pnlListView" Style="margin: 20px" Visible="false">


                    <h4>Number of occurrences on the page of each word</h4>
                    <asp:ListView ID="lvResultEachWord" runat="server">
                        <LayoutTemplate>

                            <table class="table table-striped" id="magTable">
                                <thead>
                                    <tr>
                                        <th style="width: 30%;" class="font-bold">Word<span /></th>
                                        <th style="width: 30%;" class="font-bold">Count<span /></th>

                                    </tr>
                                </thead>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                            <br />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Word")%></td>
                                <td><%# Eval("Count")%></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            Empty!
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <div style="margin: 20px"></div>
                    <h4>Number of occurrences on the page of each word listed in meta tag</h4>

                    <asp:ListView ID="lvMetatag" runat="server">
                        <LayoutTemplate>

                            <table class="table table-striped" id="magMetaTable">
                                <thead>
                                    <tr>
                                        <th style="width: 30%;" class="font-bold">Word<span /></th>
                                        <th style="width: 30%;" class="font-bold">Count<span /></th>

                                    </tr>
                                </thead>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                            <br />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Word")%></td>
                                <td><%# Eval("Count")%></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            Empty!
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <div style="margin: 20px"></div>
                    <h4>Number of external links in the text</h4>

                    <asp:ListView ID="lvExternalLink" runat="server">
                        <LayoutTemplate>

                            <table class="table table-striped" id="magAnchorTable">
                                <thead>
                                    <tr>
                                        <th style="width: 30%;" class="font-bold">Word<span /></th>
                                        <th style="width: 30%;" class="font-bold">Count<span /></th>

                                    </tr>
                                </thead>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                            <br />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Word")%></td>
                                <td><%# Eval("Count")%></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            Empty!
                        </EmptyDataTemplate>
                    </asp:ListView>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
