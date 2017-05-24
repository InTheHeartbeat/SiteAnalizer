var isLoadAnimCompleted = false;

function ShowInfographicsPanel() {
    $("#infographics-panel").css("top", "15%");
}

function HideInfographicsPanel() {
    $("#infographics-panel").css("top", "100%");
}

function LoadComplete() {
    setTimeout(function() {
        //$("body").animate({ backgroundColor: "#0072c6" }, 600);
        $("#load-bar-1").animate({ left: "-200%" }, 700);
        setTimeout(function() {
                $("#load-bar-1").hide();
                ShowInfographicsPanel();
            },
            700);
    }, (isLoadAnimCompleted === true ? 0 : 2800));    
}

$(document).ready(function() {
    $("#start-handle-button").click(function() {
        
        isLoadAnimCompleted = false;
        $("#desc-1-container").css("left", "100%");
        $("body").animate({ backgroundColor: "#435d70" }, 1000);
        $("#load-bar-1").fadeOut();
        setTimeout(function() { $("#load-bar-1").show().animate({ left: "0" }, 700) }, 1000);
        $("#main-progress-bar-step-1").animate({ width: "33.333%" }, 2700, function() { isLoadAnimCompleted = true; });
    });

    $("#form0").on("submit",
        function() {
            $("#input-url").prop("readonly", true);
            $("#start-handle-button").prop("disabled",true);
        });
    $("#horizontal-indicators-container").jPages({
        containerID: "tbody",
        previous: "←",
        next: "→",
        perPage: 200,
        delay: 20
    });
});