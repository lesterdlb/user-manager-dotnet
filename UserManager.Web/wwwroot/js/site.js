(function ($) {
    $.fn.serializeFormToObject = function (camelCased = false) {
        const data = $(this).serializeArray();

        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        const obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    $.fn.clearForm = function () {
        const $this = $(this);

        $('[name]', $this).each((i, obj) => {
            $(obj).removeClass('is-invalid');
        });
        $this[0].reset();
    };
})(jQuery);
