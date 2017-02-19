(function ($) {
    function TicketCreate() {
        var $this = this;

        function initialize() {
            $('#Content').summernote({
                disableDragAndDrop: true,

                focus: true,
                height: 150,
                codemirror: {
                    theme: 'united'
                },
                toolbar: [
              ["style", ["style"]],
              ["font", ["bold", "underline", "clear"]],
              ["fontname", ["fontname"]],
              ["color", ["color"]],
              ["para", ["ul", "ol", "paragraph"]],
              //["table", ["table"]],
              //["insert", ["link", "picture", "video"]],
              ["view", ["fullscreen", "codeview", "help"]]
                ]
            });
        }

        $this.init = function () {
            initialize();
        }
    }
    $(function () {
        var self = new TicketCreate();
        self.init();
    })
}(jQuery))