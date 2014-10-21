<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_ExportExcel.aspx.cs" Inherits="MyAdmin.Admin_Vlive.Ad_ExportExcel" %>
<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />

    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>Chọn ngày</label><input type="text" runat="server" id="tbx_ReportDay" /><input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_ReportDay.ClientID %>'),'dd/mm/yyyy',this)" />
    <asp:Button runat="server" ID="btn_Export" Text="Xuất Excel" OnClick="btn_Export_Click" />
    <div style="display:none;">
        <asp:GridView runat="server" ID="grv_Data" EnableModelValidation="True"  Visible="true"
            AutoGenerateColumns="False" onrowdatabound="grv_Data_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="TT">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày">                    
                    <ItemTemplate>
                        <%# ((DateTime)Eval("PushTime")).ToString("dd/MM/yyyy") %>
                        <asp:Label ID="lbl_LetterID" Visible="false" runat="server" Text='<%# Bind("LetterID") %>' ToolTip='<%# Bind("RowCount") %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CateName" HeaderText="Nhóm nội dung" />
                <asp:BoundField DataField="LetterName" HeaderText="Nội dung tin nổi" />
                <asp:BoundField DataField="Introduction" HeaderText="Nội dung Menu SIM" />
                 <asp:TemplateField HeaderText="Thời gian phát tin">                    
                    <ItemTemplate>
                        <%# ((DateTime)Eval("PushTime")).ToString("HH:mm") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Price" HeaderText="Giá cước địch vụ" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
     <table class="Data" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr class="Table_Header">
                <th class="Table_TL"></th>
                <th width="10">STT </th>
                <th>FileName</th>
                <th>Ngày tạo</th>
                <th>Download</th>
                <th>Xóa</th>
                <th class="Table_TR"></th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_Data">
                <ItemTemplate>
                    <tr class="Table_Row_1">
                        <td class="Table_ML_1">
                        </td>
                        <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>
                        <td>
                            <%#Eval("FileName") %>
                        </td>
                        <td>
                            <%# ((DateTime) Eval("CreateDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                        </td>
                        <td>
                            <a href="<%#Eval("DownloadLink")%>" target="_blank">Download</a>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btn_Delete" Text="Xóa" CommandArgument='<%# Bind("FileName") %>'  OnClick="btn_Delete_Click"/>
                        </td>
                        <td class="Table_MR_1">
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="Table_Row_2">
                        <td class="Table_ML_2">
                        </td>
                         <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>
                        <td>
                            <%#Eval("FileName") %>
                        </td>
                        <td>
                            <%# ((DateTime) Eval("CreateDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                        </td>
                        <td>
                            <a href="<%#Eval("DownloadLink")%>" target="_blank">Download</a>
                        </td>   
                         <td>
                            <asp:Button runat="server" ID="btn_Delete" Text="Xóa" CommandArgument='<%# Bind("FileName") %>'  OnClick="btn_Delete_Click"/>
                        </td>
                        <td class="Table_MR_2">
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="Table_Footer">
        <div class="Table_BL">
            <uc1:Admin_Paging ID="Admin_Paging1" runat="server" ShowPageSize="true" />
        </div>
        <div class="Table_BR">
        </div>
    </div>
    <div class="Div_Hidden">
        <input type="hidden" runat="server" id="hid_ListCheckAll" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
