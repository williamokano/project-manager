if (typeof String.prototype.rpad !== 'function') {
    String.prototype.rpad = function (character, size) {
        var l = this.length;
        var paddedText = this.toString();
        if (l < size) {
            for (i = l - 1; i < size - 1; i++) {
                paddedText = paddedText + character;
            }
        }
        return paddedText;
    }
}
if (typeof String.prototype.lpad !== 'function') {
    String.prototype.lpad = function (character, size) {
        var l = this.length;
        var paddedText = this.toString();
        if (l < size) {
            for (i = l - 1; i < size - 1; i++) {
                paddedText = character + paddedText;
            }
        }
        return paddedText;
    }
}
if (typeof String.prototype.endsWith !== 'function') {
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}

function HoursToTimeSpan(hours) {
    //Sem validação
    var temp = hours.split(":");
    temp[0] = parseInt(temp[0], 10);
    temp[1] = parseInt(temp[1], 10);
    temp[0] = temp[0] + parseInt(temp[1] / 60);
    temp[1] = parseInt(temp[1] % 60, 10);
    //Dias, horas, minutos, segundos
    var dadosTempo = [0, 0, 0, 0];
    dadosTempo[0] = parseInt(temp[0] / 24, 10);
    dadosTempo[1] = parseInt(temp[0] % 24, 10);
    dadosTempo[2] = parseInt(temp[1]);

    dadosTempo[0] = dadosTempo[0].toString();
    dadosTempo[1] = dadosTempo[1].toString().lpad("0", 2);
    dadosTempo[2] = dadosTempo[2].toString().lpad("0", 2);
    dadosTempo[3] = dadosTempo[3].toString().lpad("0", 2);
    return dadosTempo[0] + "." + dadosTempo[1] + ":" + dadosTempo[2] + ":" + dadosTempo[3];
}

var PageFuns = {
    ScrollTo: function (top) {
        $("html,body").animate({
            scrollTop: top
        });
    }
}