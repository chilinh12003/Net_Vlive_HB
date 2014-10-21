<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_GenerationXML_Edit.aspx.cs" Inherits="MyAdmin.Admin_Vlive.Ad_GenerationXML_Edit" %>

<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../CSS/Ad_ViewDetail.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Vlive.css" rel="stylesheet" type="text/css" />
    <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />

    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_GenerationXML.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_GenerationXML_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
   <div style="float: left;">
        <label>
            Từ khóa:</label>
        <input type="text" runat="server" id="tbx_Search" />
    </div>
    <select runat="server" id="sel_SearchType">
        <option value="0">- - Tìm theo tất cả - - </option>
        <option value="1">Theo tên</option>
    </select>
    <asp:DropDownList runat="server" ID="ddl_CateID_1" AutoPostBack="true" OnSelectedIndexChanged="ddl_CateID_1_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:DropDownList runat="server" ID="ddl_CateID_2" AutoPostBack="true" OnSelectedIndexChanged="ddl_CateID_2_SelectedIndexChanged">
        <asp:ListItem Value="0" Text=" - - Thể loại mức 2 - - "></asp:ListItem>
    </asp:DropDownList>
    <select runat="server" id="sel_CateID_3">
        <option value="0">- - Thể loại mức 3 - - </option>
    </select>
    <asp:Button runat="server" ID="btn_Search" Text="Tìm kiếm" OnClick="btn_Search_Click" />
    <asp:Button runat="server" ID="btn_CancelSearch" Text="Hủy tìm" OnClick="btn_CancelSearch_Click" Visible="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <table class="Data" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr class="Table_Header">
                <th class="Table_TL">
                </th>
                <th width="10">
                    STT
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_1" CommandArgument="NewsID ASC" OnClick="lbtn_Sort_Click">Mã</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_2" CommandArgument="NewsName ASC" OnClick="lbtn_Sort_Click">Tiêu đề</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_3" CommandArgument="CateName ASC" OnClick="lbtn_Sort_Click">Thể loại</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_6" CommandArgument="CreateDate ASC" OnClick="lbtn_Sort_Click">Ngày tạo</asp:LinkButton>
                </th>
                <th>
                    Duyệt
                </th>
                <th>
                    Xem
                </th>
                <th align="center" width="10">
                    <input type="checkbox" onclick="SelectCheckBox_All(this);" />
                </th>
                <th class="Table_TR">
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_Data">
                <ItemTemplate>
                    <tr class="Table_Row_1">
                        <td class="Table_ML_1"></td>
                        <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>
                        <td>
                            <%#Eval("NewsID") %>
                        </td>
                        <td>
                            <a href="Ad_News_Edit.aspx?ID=<%#Eval("NewsID") %>">
                                <%#Eval("NewsName")%></a>
                        </td>
                        <td>
                            <%#Eval("CateName") %>
                        </td>
                        <td>
                            <%#Eval("CreateDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("CreateDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                        </td>
                        <td>
                            <img src="<%#(((bool)Eval("IsPublish"))?"../Images/Buttons/Active_Grid.png":"../Images/Buttons/UnActive_Grid.png") %>" />
                        </td>
                        <td align="center">
                            <img src="../Images/Buttons/View_en.png" onclick="ShowLoading(this,false,false); Show_FaceBoxAjax('../Admin_Vlive/Ad_News_Detail.ashx?ID=<%#Eval("NewsID") %>');" />
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("NewsID").ToString() + "' />"%>
                        </td>
                        <td class="Table_MR_1"></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="Table_Row_2">
                        <td class="Table_ML_2"></td>
                        <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>
                        <td>
                            <%#Eval("NewsID") %>
                        </td>
                        <td>
                            <a href="Ad_News_Edit.aspx?ID=<%#Eval("NewsID") %>">
                                <%#Eval("NewsName")%></a>
                        </td>
                        <td>
                            <%#Eval("CateName") %>
                        </td>
                        <td>
                            <%#Eval("CreateDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("CreateDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                        </td>
                        <td>
                            <img src="<%#(((bool)Eval("IsPublish"))?"../Images/Buttons/Active_Grid.png":"../Images/Buttons/UnActive_Grid.png") %>" />
                        </td>
                        <td align="center">
                            <img src="../Images/Buttons/View_en.png" onclick="ShowLoading(this,false,false); Show_FaceBoxAjax('../Admin_Vlive/Ad_News_Detail.ashx?ID=<%#Eval("NewsID") %>');" />
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("NewsID").ToString() + "' />"%>
                        </td>
                        <td class="Table_MR_2"></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="Table_Footer">
        <div class="Table_BL">
            <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />
        </div>
        <div class="Table_BR">
        </div>
    </div>
    <div class="Div_Hidden">
        <input type="hidden" runat="server" id="hid_ListCheckAll" />
    </div>
    <div class="NewLine_Pad" style="text-align: center;">
        <label>
            Ngày
        </label>
        <input type="text" runat="server" id="tbx_Day" style="width: 70px;" /><input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_Day.ClientID %>'),'dd/mm/yyyy',this)" />
        <select runat="server" id="sel_Hour" style="float: none;">
        </select>
        <select runat="server" id="sel_Minute" style="float: none;">
        </select>
        <asp:Button runat="server" ID="btn_Save_Temp" Text="Lưu" OnClick="btn_Save_Temp_Click" OnClientClick="return Save();"/>
    </div>
    <div class="NewLine_Pad">
        <table class="Data" border="0" cellpadding="0" cellspacing="0">
            <tbody>
                <tr class="Table_Header">
                    <th class="Table_TL">
                    </th>
                    <th width="10">
                        STT
                    </th>
                    <th>
                        Mã tin
                    </th>
                    <th>
                        Tiêu đề
                    </th>
                    <th>
                        Thể loại
                    </th>
                    <th>
                        Ngày xuất XML
                    </th>
                    <th>
                        Xem tin
                    </th>       
                    <th>Xóa</th>            
                    <th class="Table_TR">
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="rpt_Gen">
                    <ItemTemplate>
                        <tr class="Table_Row_1">
                            <td class="Table_ML_1"></td>
                            <td>
                                <%#(Container.ItemIndex + PageIndex).ToString()%>
                            </td>
                            <td>
                                <%#Eval("NewsID") %>
                            </td>
                            <td>
                                    <%#Eval("NewsName")%>
                            </td>
                            <td>
                                <%#Eval("CateName") %>
                            </td>
                            <td>
                                <%#Eval("GenDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("GenDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                            </td>
                            <td align="center">
                                <img src="../Images/Buttons/View_en.png" onclick="ShowLoading(this,false,false); Show_FaceBoxAjax('../Admin_Vlive/Ad_News_Detail.ashx?ID=<%#Eval("NewsID") %>');" />
                            </td>
                           <td>
                            <asp:Button runat="server" ID="btn_Delete_Item" OnClick="btn_Delete_Item_Click" Text="Xóa" CommandArgument='<%#Eval("NewsID") %>' />
                           </td>
                            <td class="Table_MR_1"></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="Table_Row_2">
                            <td class="Table_ML_2"></td>
                            <td>
                                <%#(Container.ItemIndex + PageIndex).ToString()%>
                            </td>
                            <td>
                                <%#Eval("NewsID") %>
                            </td>
                            <td>                                
                                    <%#Eval("NewsName")%>
                            </td>
                            <td>
                                <%#Eval("CateName") %>
                            </td>
                            <td>
                                <%#Eval("GenDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("GenDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                            </td>
                            <td align="center">
                                <img src="../Images/Buttons/View_en.png" onclick="ShowLoading(this,false,false); Show_FaceBoxAjax('../Admin_Vlive/Ad_News_Detail.ashx?ID=<%#Eval("NewsID") %>');" />
                            </td>    
                            <td>
                            <asp:Button runat="server" ID="btn_Delete_Item" OnClick="btn_Delete_Item_Click" Text="Xóa" CommandArgument='<%#Eval("NewsID") %>' />
                           </td>                       
                            <td class="Table_MR_2"></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="Table_Footer">
            <div class="Table_BL">
            </div>
            <div class="Table_BR">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">

    <script language="javascript" type="text/javascript">
        hid_ListCheckAll = document.getElementById("<%=hid_ListCheckAll.ClientID %>");

        ReCheck_CheckboxOnGrid();

        function EditData()
        {
            if (BeforeEditData())
            {
                document.location = '../Admin_Vlive/Ad_News_Edit.aspx?ID=' + hid_ListCheckAll.value;

                return true;
            }
            return false;
        }

        function Save()
        {
            if (GetAllCheck('Xin hãy chọn ít nhất một mục để lưu'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    </script>

</asp:Content>
