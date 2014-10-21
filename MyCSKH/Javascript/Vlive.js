/// <reference path="jquery.js" />
/// <reference path="ForAll.js" />
var Domain = "http://localhost:8080/VliveCMS";

//Lưu ID của Letter mà đang được sử dụng để view List Record.
var LetterID_CurrentView = 1;
var ParentID_CurrentView = 0;

//Hàm callback khi gọi show FaceBox
CallBack_FaceBox = function()
{
    ChangeType();
}

//Hàm callback khi close FaceBox
CallBack_CloseFaceBox = function()
{
    LetterID_CurrentView = 1;
    ParentID_CurrentView = 0;
}

function ChangeType()
{
    if ($("#sel_RecordType").val() == "1")
    {
        $("#div_ServiceID").css("display", "none");
        $("#div_Keyword").css("display", "none");

        $("#tbx_Keyword").val("");
        
        $("#div_Content").css("display", "inline");
    }
    else
    {
        $("#div_ServiceID").css("display", "inline");
        $("#div_Keyword").css("display", "inline");

        $("#div_Content").css("display", "none");

        $("#div_Content").css("display", "none");
        $("#tbx_Content").val("");
        
    }
}
function ChangeType_HoldValue()
{
    if ($("#sel_RecordType").val() == "1")
    {
        $("#div_ServiceID").css("display", "none");
        $("#div_Keyword").css("display", "none");
        $("#div_Content").css("display", "inline");
    }
    else
    {
        $("#div_ServiceID").css("display", "inline");
        $("#div_Keyword").css("display", "inline");
        $("#div_Content").css("display", "none");
        $("#div_Content").css("display", "none");       
    }
}


function ViewLetter()
{
    
    var url = Domain + "/MyVlive.ViewLetter.ajax";
    
    try
    {
        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "",
            url: url, //Đường dẫn tới nơi xử lý request ajax
            success: function(string)
            {
                var arr_JSON = $.parseJSON(string);

                $.each(arr_JSON, function()
                {
                    
                    //nếu như biến là kết quả trả về
                    if (this.Parameter == "Result")
                    {
                        //nếu kiểu trả về là success
                        if (this.CurrentTypeResult == "1")
                        {
                            $("#div_Letter").html(this.Value);
                        }
                        else
                        {
                            $("#div_Letter").html(this.Value);
                        }
                    }
                    return false;
                });               
            }
        });
    }
    catch (e)
    {
        alert(e);
    }
}
function ViewInsertRecord(ctr_This, LetterID)
{
    CallBack_FaceBox = function() { ChangeType(); };
    LetterID_CurrentView = LetterID;
    ParentID_CurrentView = 0;
    ShowLoading($(ctr_This), false, false);
    Show_FaceBoxAjax('Ad_News_InsertLetter.ashx?LetterID=' + LetterID + '');
}

function ViewInsertRecord_Child(ctr_This, LetterID, Record_ParentID)
{
    CallBack_FaceBox = function() { ChangeType(); };
    LetterID_CurrentView = LetterID;
    ParentID_CurrentView = Record_ParentID;
    ShowLoading($(ctr_This), false, false);
    Show_FaceBoxAjax('Ad_News_InsertLetter.ashx?LetterID=' + LetterID + '&ParentID=' + Record_ParentID);
}
function UpdateRecord_Child(ctr_This,ParentID, RecordID)
{
    ParentID_CurrentView = ParentID;
    ShowLoading($(ctr_This), false, false);
    CallBack_FaceBox = function() { ChangeType_HoldValue(); };
    Show_FaceBoxAjax('Ad_News_InsertLetter.ashx?RecordID=' + RecordID);
    
}
function UpdateRecord_Parent(ctr_This, RecordID)
{
    ShowLoading($(ctr_This), false, false);
    CallBack_FaceBox = function() { ChangeType_HoldValue(); };
    Show_FaceBoxAjax('Ad_News_InsertLetter.ashx?RecordID=' + RecordID);
}
//Lấy nội dung các mẩu tin con
function ViewRecord_Parent(ctr_This,LetterID)
{
    LetterID_CurrentView = LetterID;
    
    var url = Domain + "/MyVlive.ViewRecord.ajax";
    if(ctr_This)
        ShowLoading($(ctr_This), false, false);
        
    try
    {
        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "LetterID=" + LetterID +"&IsParent=True",
            url: url, //Đường dẫn tới nơi xử lý request ajax
            success: function(string)
            {
                var arr_JSON = $.parseJSON(string);

                $.each(arr_JSON, function()
                {
                    
                    //nếu như biến là kết quả trả về
                    if (this.Parameter == "Result")
                    {
                        //nếu kiểu trả về là success
                        if (this.CurrentTypeResult == "1")
                        {
                            $("#div_Record_Parent").html(this.Value);
                        }
                        else
                        {
                            $("#div_Record_Parent").html(this.Value);
                        }
                    }
                    return false;
                });
                
                HideLoading();
            }
        });
    }
    catch (e)
    {
        alert(e);
    }
}


//Lấy nội dung của các mẩu tin con
function ViewRecord_Child(ctr_This, LetterID, ParentID)
{

    LetterID_CurrentView = LetterID;

    var url = Domain + "/MyVlive.ViewRecord.ajax";

    if (ctr_This)
        ShowLoading($(ctr_This), false, false);
    try
    {
        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "LetterID=" + LetterID + "&IsParent=True&ParentID=" + ParentID,
            url: url, //Đường dẫn tới nơi xử lý request ajax
            success: function(string)
            {
                var arr_JSON = $.parseJSON(string);

                $.each(arr_JSON, function()
                {

                    //nếu như biến là kết quả trả về
                    if (this.Parameter == "Result")
                    {
                        //nếu kiểu trả về là success
                        if (this.CurrentTypeResult == "1")
                        {
                            $("#div_Record_Child").html(this.Value);
                        }
                        else
                        {
                            $("#div_Record_Child").html(this.Value);
                        }
                    }
                    return false;
                });

                HideLoading();
            }
        });
    }
    catch (e)
    {
        alert(e);
    }
}

//Xóa dữ liệu 1 bản tin
function DeleteLetter(ctr_This,LetterID)
{
    
    if (!confirm("Bạn có chắc muốn xóa bản tin đã chọn?"))
    {
        return false;
    }
    
    var url = Domain + "/MyVlive.DeleteLetter.ajax";

    ShowLoading($(ctr_This), false, false);
    try
    {
        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "LetterID=" + LetterID,
            url: url, //Đường dẫn tới nơi xử lý request ajax
            success: function(string)
            {
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
            }
        });
    }
    catch (e)
    {
        alert(e);
    }
}


//Xóa dữ liệu 1 mẩu tin cha
function DeleteRecord(ctr_This, RecordID,IsParent)
{

    if (!confirm("Bạn có chắc muốn xóa mẩu tin đã chọn?"))
    {
        return false;
    }

    var url = Domain + "/MyVlive.DeleteRecord.ajax";

    ShowLoading($(ctr_This), false, false);
    try
    {
        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "RecordID=" + RecordID,
            url: url, //Đường dẫn tới nơi xử lý request ajax
            success: function(string)
            {
                var arr_JSON = $.parseJSON(string);

                $.each(arr_JSON, function()
                {
                    //nếu như biến là kết quả trả về
                    if (this.Parameter == "Result")
                    {
                        debugger;
                        //nếu kiểu trả về là success
                        if (this.CurrentTypeResult == "1")
                        {
                            if (IsParent)
                                ViewRecord_Parent(null, LetterID_CurrentView);
                            else
                                ViewRecord_Child(null, LetterID_CurrentView,ParentID_CurrentView);
                        }
                        else
                        {
                            alert(this.Description);
                        }
                    }
                    return false;
                });
                HideLoading();
            }
        });
    }
    catch (e)
    {
        alert(e);
    }
}

function SaveRecord(ctr_this,RecordID)
{
    if (!RecordID)
        RecordID = 0;
           
    var url = Domain + "/MyVlive.SaveRecord.ajax";
    
    ShowLoading($(ctr_this), false, false);
    try
        {
            var LetterID = LetterID_CurrentView;
            var RecordTypeID = $("#sel_RecordType").val();
            var Priority = $("#tbx_Priority").attr("value");
            var RecordName = $("#tbx_Introduction").attr("value");
            var Introduction = $("#tbx_Introduction").attr("value");
            var Content = $("#tbx_Content").attr("value");
            var MethodID = $("#sel_Method").val();
            var ServiceID = $("#sel_ServiceID").val();
            var Keyword = $("#tbx_Keyword").attr("value");
            var IsParent = "true";
            var ParentID = ParentID_CurrentView;
            
            if (ParentID_CurrentView > 0)
                IsParent = "false";
                
            $.ajax({
                type: "POST", //Phương thức gửi request là POST hoặc GET
                data: "RecordID=" + RecordID + 
                            "&NewsID=" + NewsID +
                            "&LetterID=" + LetterID +
                            "&RecordTypeID=" + RecordTypeID +
                            "&Priority=" + Priority +
                            "&RecordName=" + RecordName +
                            "&Introduction=" + Introduction +
                            "&Content=" + Content +
                            "&MethodID=" + MethodID +
                            "&ServiceID=" + ServiceID +
                            "&Keyword=" + Keyword +
                            "&IsParent=" + IsParent +
                            "&ParentID=" + ParentID, //tham số gửi kèm
                url: url, //Đường dẫn tới nơi xử lý request ajax
                success: function(string)
                {
                    var arr_JSON = $.parseJSON(string);

                    $.each(arr_JSON, function()
                    {
                        debugger;
                        //nếu như biến là kết quả trả về
                        if (this.Parameter == "Result")
                        {

                            //nếu kiểu trả về là success
                            if (this.CurrentTypeResult == "1")
                            {
                                alert(this.Description);
                                if (ParentID_CurrentView > 0)
                                {
                                    ViewRecord_Child(null, LetterID_CurrentView, ParentID_CurrentView);
                                }
                                else
                                {
                                    ViewRecord_Parent(null, LetterID_CurrentView);
                                }
                            }
                            else
                            {
                                alert(this.Description);
                            }
                        }
                        return false;
                    });
                    HideLoading();
                }
            });
    }
    catch (e)
    {
        alert(e);
    }
}


function CountCharacter(ctr_This)
{
    var text = $(ctr_This).val();
    if (text.length >= 450)
    {
        $(ctr_This).val(text.substring(0, 449));
    }

    if (text.length >= 450)
        $("#b_count").html(" 0 ");
    else
        $("#b_count").html(" " + 450 - text.length + " ");

}
