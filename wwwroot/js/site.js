// Write your Javascript code.

$(document).ready(() => {
    $("#action1").hide();    
    $("#action2").hide();
    $("#action3").hide();
});

const timeouts = {};

function takeAction(long, short) {    
    function hideOnExpire(id) {    
        const c = id.substring(1);    
        
        
        if (timeouts[id]) {
            clearTimeout(timeouts[id]);
            delete timeouts[id];
        };
        var timeout = setTimeout(() => {
            $(`.list-side-by-side.${c} #expired`).text(true);
            $(id).hide();
            clearTimeout(timeout);
            delete timeouts[id];
        }, 60*1000);       
        timeouts[id] = timeout;        
    }
    const id = `action${short}`;
    $.ajax({
        type: 'GET',
        url: `/Action/Action${long}`,
        success: function (data, textStatus, request) {
            $(`#action${short} .number`).text(request.getResponseHeader("value"));            
            $(`#action${short}`).removeClass("red").show();              
            $(`.list-side-by-side.${id} #status`).text(request.status);
            $(`.list-side-by-side.${id} #endpoint`).text(this.url);
            $(`.list-side-by-side.${id} #expired`).text(false);
            hideOnExpire(`#action${short}`);

        },
        error: function (err, textStatus) {
            $(`#action${short} .number`).text(err.getResponseHeader("value"));
            $(`#action${short} .http`).text(err.status);
            $(`#action${short}`).addClass("red").show();            
            $(`.list-side-by-side.${id} #status`).text(err.status);
            $(`.list-side-by-side.${id} #endpoint`).text(this.url);
            $(`.list-side-by-side.${id} #expired`).text(false);
            $(`${id}-status`).text(err.status);
            hideOnExpire(`#action${short}`);
        }
    });
}