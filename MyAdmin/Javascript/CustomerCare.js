
var PageIndex_Answer = 1;
var PageIndex_MO = 1;

function ViewInfo(ctr_this)
{
    ShowLoading($(ctr_this), false, false);
    var url = Domain + "/MyCCare.ViewInfo.ajax";
    try
        {
            var MSISDN = $("#tbx_MSISDN").attr("value");
            var Week = $(sel_Week).val();
            
            var BeginDate = $(tbx_StartDate).attr("value");
            var BeginHour = $(sel_StartHour).val();
            
            var EndDate = $(tbx_StopDate).attr("value");
            var EndHour = $(sel_StopHour).val();
            

            $.ajax({
                type: "POST", //Phương thức gửi request là POST hoặc GET
                data: "MSISDN=" + MSISDN +
                    "&Week=" + Week +
                    "&BeginDate=" + BeginDate +
                    "&BeginHour=" + BeginHour +
                    "&EndDate=" + EndDate +
                    "&EndHour=" + EndHour,
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
                                $("#div_info").html(this.Value);
                            }
                            else if (this.CurrentTypeResult == "0") //Error
                            {
                                alert(this.Description);
                            }
                            else if (this.CurrentTypeResult == "5") //UnSucces
                            {
                                $("#div_info").html('<div class="AjaxUnSuccess">' + this.Value + '</div>');
                            }
                            else
                            {
                                $("#div_info").html(this.Description);
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


function ViewHistoryAnswer(ctr_this,pIndex)
{
    PageIndex_Answer = pIndex;
    
    ShowLoading($(ctr_this), false, false);
    var url = Domain + "/MyCCare.ViewAnswerHistory.ajax";
    try
    {
        var MSISDN = $("#tbx_MSISDN").attr("value");
        var Week = $(sel_Week).val();

        var BeginDate = $(tbx_StartDate).attr("value");
        var BeginHour = $(sel_StartHour).val();

        var EndDate = $(tbx_StopDate).attr("value");
        var EndHour = $(sel_StopHour).val();


        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "MSISDN=" + MSISDN +
                    "&Week=" + Week +
                    "&BeginDate=" + BeginDate + "T" + BeginHour +
                    "&EndDate=" + EndDate + "T" + EndHour +
                    "&PageIndex=" + PageIndex_Answer,
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
                            $("#div_AnswerHistory").html(this.Value);
                        }
                        else if (this.CurrentTypeResult == "0") //Error
                        {
                            alert(this.Description);
                        }
                        else if (this.CurrentTypeResult == "5") //UnSucces
                        {
                            $("#div_AnswerHistory").html('<div class="AjaxUnSuccess">' + this.Value + '</div>');
                        }
                        else
                        {
                            $("#div_AnswerHistory").html(this.Description);
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


function ViewMOAnswer(ctr_this, pIndex)
{
    PageIndex_MO = pIndex;

    ShowLoading($(ctr_this), false, false);
    var url = Domain + "/MyCCare.ViewMOHistory.ajax";
    try
    {
        var MSISDN = $("#tbx_MSISDN").attr("value");
        var Week = $(sel_Week).val();

        var BeginDate = $(tbx_StartDate).attr("value");
        var BeginHour = $(sel_StartHour).val();

        var EndDate = $(tbx_StopDate).attr("value");
        var EndHour = $(sel_StopHour).val();
        $.ajax({
            type: "POST", //Phương thức gửi request là POST hoặc GET
            data: "MSISDN=" + MSISDN +
                    "&Week=" + Week +
                    "&BeginDate=" + BeginDate + "T" + BeginHour +
                    "&EndDate=" + EndDate + "T" + EndHour +
                    "&PageIndex=" + PageIndex_MO,
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
                            $("#div_MOLog").html(this.Value);
                        }
                        else if (this.CurrentTypeResult == "0") //Error
                        {
                            alert(this.Description);
                        }
                        else if (this.CurrentTypeResult == "5") //UnSucces
                        {
                            $("#div_MOLog").html('<div class="AjaxUnSuccess">' + this.Value + '</div>');
                        }
                        else
                        {
                            $("#div_MOLog").html(this.Description);
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