(function($) {
    $.fn.attachFooter = function(elemtnsWithHeight) {
        var $element = $(this);
        $(document).ready(changeSize);
        $(window).resize(changeSize);

        function changeSize() {
            var height = $element.innerHeight();
            elemtnsWithHeight.forEach(function(el, i, arr) {
                height += $(el).innerHeight();
            });
            var position, width;
            if (window.innerHeight < height) {
                position = 'initial';
                width = 'auto';
            }
            else {
                position = 'fixed';
                width = '100%';
            }
            $element.each(function() {
                $(this).css('position', position);
                $(this).css('width', width);
                $(this).css('bottom', '0px');
            })
        }
    };
})(jQuery);