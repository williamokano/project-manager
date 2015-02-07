/*
	Máscara telefônica novo dítigo for Masked Input plugin for jQuery
	Copyright (c) 2013-2013 Quântica Networks (quanticanetworks.com.br)
	We have no licence (IT'S FREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE)
	Version: 1.0s
*/
jQuery.fn.brTelMask = function () {

    return this.each(function () {
        var el = this;
        $(el).focus(function () {
            $(el).mask("(99) 9999-9999?9");
        });

        $(el).focusout(function () {
            var phone, element;
            element = $(el);
            element.unmask();
            phone = element.val().replace(/\D/g, '');
            if (phone.length > 10) {
                element.mask("(99) 99999-999?9");
            } else {
                element.mask("(99) 9999-9999?9");
            }
        });
    });
}