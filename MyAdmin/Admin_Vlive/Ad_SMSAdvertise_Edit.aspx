<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_SMSAdvertise_Edit.aspx.cs" Inherits="MyAdmin.Admin_Vlive.Ad_SMSAdvertise_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_SMSAdvertise.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_SMSAdvertise_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left" >
        <div class="Edit_Title">
            Loại tin nhắn</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_SMSType">
            </select>
        </div>
        <div class="Edit_Title">
            Loại nội dung</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_MediaType">
            </select>
        </div>
        <div class="NewLine">
            <div class="Edit_Title" style="height: 100px;">
                Nội dung (Tiêu đề):</div>
            <div class="Edit_Control_Editor">
                <textarea runat="server" id="tbx_Content" style="width: 99%; height: 100px;"></textarea>
            </div>
        </div>
        <div class="NewLine">
            <div class="Edit_Title" style="height: 100px;">
                Link download:</div>
            <div class="Edit_Control_Editor">
                <textarea runat="server" id="tbx_Wappush" style="width: 99%;"></textarea>
            </div>
        </div>
    </div>
    <div class="Edit_Right">
        <div class="Properties_Header">
            <div class="Properties_Header_In">
                Thêm mới bản tin
            </div>
        </div>
        <div class="Properties">
            <div class="Properties_Title">
                Tên quảng cáo:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_AdvName" style="width: 99%;" maxlength="84" />
            </div>
            <div class="Properties_Title">
                Keyword:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Keyword" style="width: 99%;" maxlength="84" />
            </div>
            <div class="Properties_Title">
                Đầu số:</div>
            <div class="Properties_Control">
                <select runat="server" id="sel_ShortCode">
                    <option value="9093">9093</option>
                    <option value="9193">9193</option>
                    <option value="9293">9293</option>
                    <option value="9393">9393</option>
                    <option value="9493">9493</option>
                    <option value="9593">9593</option>
                    <option value="9693">9693</option>
                    <option value="9793">9793</option>
                </select>
            </div>
            <div class="Properties_Title">
                Kích hoạt:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_Active" checked="checked" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
