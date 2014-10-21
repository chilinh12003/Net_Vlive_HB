<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_CustomerCare.aspx.cs" Inherits="MyAdmin.Admin_CCare.Ad_CustomerCare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../CSS/VliveGame.css" rel="stylesheet" type="text/css" />

    <script src="../Javascript/CustomerCare.js" type="text/javascript"></script>

    <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />

    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>

    <link href="../CSS/Admin_Paging.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Số điện thoại</label>
    <input type="text" id="tbx_MSISDN" />
    <select runat="server" id="sel_Week" style="float: none;">
        <option value="1">Tuần 1</option>
        <option value="2">Tuần 2</option>
        <option value="3">Tuần 3</option>
        <option value="4">Tuần 4</option>
        <option value="5">Tuần 5</option>
    </select>
    <label>
        Từ Ngày:</label>
    <input type="text" runat="server" id="tbx_StartDate" style="width: 70px;" />
    <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_StartDate.ClientID %>'),'dd/mm/yyyy',this)" />
    <label>
        Giờ:</label>
    <select style="float: none;" runat="server" id="sel_StartHour">
    </select>
    <label>
        Đến Ngày:</label>
    <input type="text" runat="server" id="tbx_StopDate" style="width: 70px;" />
    <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_StopDate.ClientID %>'),'dd/mm/yyyy',this)" />
    <label>
        Giờ:</label>
    <select style="float: none;" runat="server" id="sel_StopHour">
    </select>
    <input type="button" value="Thông tin" id="btn_Info" onclick="return ViewInfo(this);" />
    <input type="button" value="LS Trả lời" id="btn_History" onclick="return ViewHistoryAnswer(this,1);"/>
    <input type="button" value="LS MO/MT" id="btn_MOLog" onclick="return ViewMOAnswer(this,1);"/>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <div class="VliveGame">
        <fieldset>
            <legend>Thông tin hiện tại</legend>
            <div class="NoiDung" id="div_info">
            
            </div>
        </fieldset>
        <fieldset style="width: 60%">
            <legend>Lịch sử trả lời câu hỏi</legend>
            <div class="NoiDung" id="div_AnswerHistory">
               
            </div>
        </fieldset>
    </div>
    <div class="VliveGame">
        <fieldset style="width: 98%;">
            <legend>Lịch sử MO/MT</legend>
            <div class="NoiDung" id="div_MOLog">
                
            </div>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
<script language="javascript" type="text/javascript">

    var sel_Week = document.getElementById('<%=sel_Week.ClientID %>');
    var tbx_StartDate = document.getElementById('<%=tbx_StartDate.ClientID %>');
    var sel_StartHour = document.getElementById('<%=sel_StartHour.ClientID %>');
    
    var tbx_StopDate = document.getElementById('<%=tbx_StopDate.ClientID %>');
    var sel_StopHour = document.getElementById('<%=sel_StopHour.ClientID %>');
    
</script>
</asp:Content>
