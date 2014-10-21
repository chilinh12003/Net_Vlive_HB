<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_News_Edit.aspx.cs" Inherits="MyAdmin.Admin_Vlive.Ad_News_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />

    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>

    <link href="../CSS/Vlive.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_News.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_News_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left" style="width: 48%;">
        <div class="Edit_Title">
            Thể loại mức 1</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_CateID_1" OnSelectedIndexChanged="ddl_CateID_1_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 2</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_CateID_2" OnSelectedIndexChanged="ddl_CateID_2_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 3</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_CateID_3">
                <option value="0">- - Thể loại mức 3 - - </option>
            </select>
        </div>
        <div class="NewLine">
            <div class="Edit_Title">
                Mô tả tin:</div>
            <div class="Edit_Control_Editor">
                <input type="text" runat="server" id="tbx_NewsName" style="width: 99%;" />
            </div>
        </div>
    </div>
    <div class="Edit_Right" style="width: 50%;">
        <div class="Properties_Header">
            <div class="Properties_Header_In">
                Thêm mới bản tin
            </div>
        </div>
        <div class="Properties">
            <div class="Properties_Title">
                Tên bản tin:</div>
            <div class="Properties_Control">
                <input type="text" id="tbx_LetterName" style="width: 99%;" maxlength="84" />
            </div>
            <div class="Properties_Title">
                Độ ưu tiên:</div>
            <div class="Properties_Control">
                <input type="text" id="tbx_Priority_Letter" value="0" style="width: 70px;" />
            </div>
            <div class="Accept NewLine">
                <div class="Properties_Title" style="height: 40px;">
                    Ngày phát tin</div>
                <div class="Properties_Control" style="height: 40px;">
                    <input type="text" runat="server" id="tbx_StartDate" style="width: 70px;" />
                    <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_StartDate.ClientID %>'),'dd/mm/yyyy',this)" />
                    <div class="NewLine">
                        <select runat="server" id="sel_StartHour">
                        </select>
                        <select runat="server" id="sel_StartMinute">
                        </select>
                        <select runat="server" id="sel_StartSecond">
                        </select>
                    </div>
                </div>
            </div>
            <div class="NewLine">
                <div class="Properties_Title">
                </div>
                <div class="Properties_Control">
                    <input type="button" value="Thêm" id="btn_Save_Letter" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <div class="Letter">
        <fieldset id="News">
            <legend>Danh sách bản tin </legend>
            <div id="div_Letter" style="float: left; width: 100%;">
            </div>
        </fieldset>
        <fieldset id="Letter">
            <legend>Danh sách mẩu tin</legend>
            <div id="div_Record_Parent">
            </div>
        </fieldset>
        <fieldset id="Record">
            <legend>Danh sách mẩu tin con</legend>
            <div id="div_Record_Child">
            </div>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">

    <script type="text/javascript" language="javascript">
        var NewsID = "<%=EditID.ToString() %>"
    </script>

    <script src="../Javascript/Vlive.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        ViewLetter();
        var LetterID_Current = 0;


        $("#btn_Save_Letter").click(function()
        {
            var tbx_StartDate = document.getElementById("<%=tbx_StartDate.ClientID %>");
            var sel_StartHour = document.getElementById("<%=sel_StartHour.ClientID %>");
            var sel_StartMinute = document.getElementById("<%=sel_StartMinute.ClientID %>");
            var sel_StartSecond = document.getElementById("<%=sel_StartSecond.ClientID %>");

            var url = Domain + "/MyVlive.SaveLetter.ajax";

            var PushTime = $(tbx_StartDate).val() + "T" + $(sel_StartHour).val() + ":" + $(sel_StartMinute).val() + ":" + $(sel_StartSecond).val();

            ShowLoading($("#btn_Save_Letter"), false, false);
            try
            {
                $.ajax({
                    type: "POST", //Phương thức gửi request là POST hoặc GET
                    data: "LetterID=" + LetterID_Current + "&NewsID=" + NewsID + "&LetterName=" + $("#tbx_LetterName").attr("value") + "&Priority=" + $("#tbx_Priority_Letter").attr("value") + "&PushTime=" + PushTime, //tham số gửi kèm
                    url: url, //Đường dẫn tới nơi xử lý request ajax
                    success: function(string)
                    {
                        debugger;
                        var arr_JSON = $.parseJSON(string);
                        $.each(arr_JSON, function()
                        {
                            //nếu như biến là kết quả trả về
                            if (this.Parameter == "Result")
                            {
                                //nếu kiểu trả về là success
                                if (this.CurrentTypeResult == "1")
                                {
                                    ViewLetter();
                                }
                                else
                                {
                                    alert(this.Description);
                                }
                            }
                            return false;
                        });
                        HideLoading();
                        LetterID_Current = 0;
                    }
                });
            }
            catch (e)
            {
                alert(e);
            }
        });
    </script>

</asp:Content>
