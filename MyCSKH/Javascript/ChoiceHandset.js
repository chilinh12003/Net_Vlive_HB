var div_ResultPhone;

//Cho biết là game hay là soft hay là them
// 1: Là gamge, 2: Là soft, 3: là Theme
var MediaType = 1;

//Load Ajax trang chọn handset
function LoadChoiceHandset(ctr_This)
{
    ShowLoading(ctr_This, false, false);
    var URL = "../Admin/Ad_Game_ChoiceHandset.ashx";
    if (MediaType == 2)
    {
        URL = "../Admin/Ad_Soft_ChoiceHandset.ashx";
    }
    else if (MediaType == 3)
    {
        URL = "../Admin/Ad_Theme_ChoiceHandset.ashx";
    }   
    Show_FaceBoxAjax(URL);
}

//Hàm khởi tạo khi tìm handset
function SearchPhone_Init()
{
    div_ResultPhone = document.getElementById("div_Result");
}

//Hàm bắt đầu tìm kiếm Handset
function BeginSearch(ctr_This)
{
    ShowLoading(ctr_This,false,false);
    
    var BranchID = document.getElementById("sel_PhoneBranch").options[document.getElementById("sel_PhoneBranch").selectedIndex].value;
    var OptionID = document.getElementById("sel_Option").options[document.getElementById("sel_Option").selectedIndex].value;
    var SearchContent = document.getElementById("tbx_Search").value;

    SearchPhone(BranchID, OptionID, SearchContent);
}

//Tìm kiếm handset
function SearchPhone(BranchID, OptionID, SearchContent)
{
   
    SearchPhone_Init();
    var URL_Get = "../Admin/Ad_Game_SearchHandset.ashx";
    if (MediaType == 2)
    {
        URL_Get = "../Admin/Ad_Soft_SearchHandset.ashx";
    }
    else if (MediaType == 3)
    {
        URL_Get = "../Admin/Ad_Theme_SearchHandset.ashx";
    }
    
    var Para = "?BranchID=" + BranchID + "&OptionID=" + OptionID + "&SearchContent=" + SearchContent;
    URL_Get += encodeURI(Para);
    URL_Get = CreateParameterTime(URL_Get);

    var xmlHttp = CreateAjax();
    xmlHttp.onreadystatechange = function()
    {
        if (xmlHttp.readyState == 4)
        {
            div_ResultPhone.innerHTML = xmlHttp.responseText;
            FaceBoxLoadComplete();
            HideLoading();
        }

    }
    xmlHttp.open("GET", URL_Get, true);
    xmlHttp.send(null);
}

//Chọn tất cả các handset đã tìm ra
function CheckAllPhone(ctr_This)
{
    for(var i = 0; i< 100000; i++)
    {
        var chk_current = document.getElementById("chk_"+i);
        if(chk_current)
        {
            chk_current.checked = ctr_This.checked;
        }
        else
            break;
    }
}


//Lấy danh sách PhoneID đã chọn
function GetPhoneSelected(Begin,End)
{
    
    var listPhoneID = "";
    for (var i = Begin; i < End; i++)
    {
        var chk_current = document.getElementById("chk_" + i);
        if (chk_current)
        {
            if (chk_current.checked)
            {
                if (listPhoneID == "")
                {
                    listPhoneID = chk_current.value;
                }
                else
                {
                    listPhoneID += "|" + chk_current.value;
                }
            }
        }
        else
            break;
    }
    return listPhoneID;
}
function SavePhone_Repeat(Para)
{
    var URL_Get = "../Admin/Ad_Game_SavePhone.ashx";
    if (MediaType == 2)
    {
        URL_Get = "../Admin/Ad_Soft_SavePhone.ashx";
    }
    else if (MediaType == 3)
    {
        URL_Get = "../Admin/Ad_Theme_SavePhone.ashx";
    }
    
    URL_Get += "?id=" + encodeURI(Para);
    URL_Get = CreateParameterTime(URL_Get);

    var xmlHttp = CreateAjax();
    xmlHttp.onreadystatechange = function()
    {
        if (xmlHttp.readyState == 4)
        {
            TempIndex += 100;
            if (xmlHttp.responseText == "1")
            {
                ErrorType = 1;
            }
            else
            {
                ErrorType = -1
            }
            return;
        }
    }

    xmlHttp.open("GET", URL_Get, true);
    xmlHttp.send(null);
}

var BeginIndex = 0;
var EndIndex = 100;
var TempIndex = 101;
var ErrorType = 1;
var SavePhone_Interval;
function SavePhone_Delay()
{
    var Para = "";
    if (TempIndex > BeginIndex)
    {
        //Lấy phone đã chọn, mỗi lần lần lấy chỉ lấy 1 số lượng nhất định
        //Vì Querystring không thể truyền quá dài
        Para = GetPhoneSelected(BeginIndex, EndIndex);

        if (Para == "")
        {
            clearInterval(SavePhone_Interval);
            HideLoading();
            if (TempIndex == 11)
            {
                alert("Xin hãy chọn phone trước.");
            }
            else if (ErrorType == 1)
            {
                alert("Cập nhật dữ liệu thành công.");
            }
            else if (ErrorType == 0)
            {
                alert("Cập nhật dữ liệu KHÔNG thành công.");
            }
            else if (ErrorType == -1)
            {
                alert("Có lỗi trong quá trịnh cập nhật.");
            }
            return;
        }
        else
        {
            SavePhone_Repeat(Para);
        }
        
        BeginIndex = TempIndex;
        EndIndex = BeginIndex + 100;
    }
}

//Lưu phone đã chọn xuống session
function SavePhone(ctr_This)
{
    BeginIndex = 0;
    EndIndex = 100;
    TempIndex = 101;
    ErrorType = 1;
    ShowLoading(ctr_This, false, false);
    SavePhone_Interval = setInterval("SavePhone_Delay()", 100);
}

//Xem tất cả các handset đã được lưu xuống
function ViewHandset(ctr_This)
{
   
    if (ctr_This)
    {
        ShowLoading(ctr_This, false, false);
    }
    var URL_Get = "../Admin/Ad_Game_ViewHandset.ashx";
    
    if (MediaType == 2)
    {
        URL_Get = "../Admin/Ad_Soft_ViewHandset.ashx";
    }
    else if (MediaType == 3)
    {
        URL_Get = "../Admin/Ad_Theme_ViewHandset.ashx";
    }

    Show_FaceBoxAjax(URL_Get);
}

//Xóa 1 dòng(tr) trong 1 table
function DeleteItem(ctr_This)
{
    if (!ctr_This)
        return;
    var div_PhoneItem = ctr_This.parentNode;
    
    if (!div_PhoneItem)
        return;

    var div_Search_PhoneSaved_Data = div_PhoneItem.parentNode;
    
    if (!div_Search_PhoneSaved_Data)
        return;

    div_Search_PhoneSaved_Data.removeChild(div_PhoneItem);

}
function DeleteItem_All(ctr_This)
{
    if (!ctr_This)
        return;
    var div_PhoneItem = ctr_This.parentNode;

    if (!div_PhoneItem)
        return;

    var div_Search_PhoneSaved_Data = div_PhoneItem.parentNode;

    if (!div_Search_PhoneSaved_Data)
        return;

    div_Search_PhoneSaved_Data.innerHTML = "";
}
function DeletePhone_All(ctr_This)
{
    if (!confirm("Bạn có chắc muốn xóa hết dữ liệu?"))
    {
        return;
    }
    ShowLoading(ctr_This, false, false);
    var URL_Get = "../Admin/Ad_Game_DeleteHandset.ashx";
    
    if (MediaType == 2)
    {
        URL_Get = "../Admin/Ad_Soft_DeleteHandset.ashx";
    }
    else if (MediaType == 3)
    {
        URL_Get = "../Admin/Ad_Theme_DeleteHandset.ashx";
    }
    
    URL_Get += "?id=" + encodeURI("-1");
    URL_Get = CreateParameterTime(URL_Get);

    var xmlHttp = CreateAjax();
    xmlHttp.onreadystatechange = function()
    {
        if (xmlHttp.readyState == 4)
        {
            if (xmlHttp.responseText == "1")
            {
                DeleteItem_All(ctr_This);
            }
            else if (xmlHttp.responseText == "0")
            {
                alert("Xóa dữ liệu KHÔNG thành công.");
            }
            else
            {
                alert("Có lỗi trong quá trình xóa dữ liệu.");
            }

            HideLoading();
        }

    }
    xmlHttp.open("GET", URL_Get, true);
    xmlHttp.send(null);
}
//Xóa handset trong danh sách handset đã lưu
function DeletePhone(ctr_This, PhoneID)
{
    if (!confirm("Bạn có chắc muốn xóa mục này?"))
    {
        return;
    }
    ShowLoading(ctr_This, false, false);
    var URL_Get = "../Admin/Ad_Game_DeleteHandset.ashx";
    
    if (MediaType == 2)
    {
        URL_Get = "../Admin/Ad_Soft_DeleteHandset.ashx";
    }
    else if (MediaType == 3)
    {
        URL_Get = "../Admin/Ad_Theme_DeleteHandset.ashx";
    }

    URL_Get += "?id=" + encodeURI(PhoneID);
    URL_Get = CreateParameterTime(URL_Get);

    var xmlHttp = CreateAjax();
    xmlHttp.onreadystatechange = function()
    {
        if (xmlHttp.readyState == 4)
        {
            if (xmlHttp.responseText == "1")
            {
                DeleteItem(ctr_This);
            }
            else if (xmlHttp.responseText == "0")
            {
                alert("Xóa dữ liệu KHÔNG thành công.");
            }
            else
            {
                alert("Có lỗi trong quá trình xóa dữ liệu.");
            }
            
            HideLoading();
        }

    }
    xmlHttp.open("GET", URL_Get, true);
    xmlHttp.send(null);
}