var Checkout = {

    _isConsole: true && typeof console !== undefined,

    init: function (payButtonSelector) {
        this.log("Initializing Checkout");

        var self = this;
        $(payButtonSelector).click(function (e) {
            e.preventDefault();

            var errors = self.validatePaymentForm();
            if (errors.lengh > 0) {
                self.displayErrors();
                return;
            }


        });
    },

    validatePaymentForm: function () {
        var errors = [];

        return errors;
    },

    displayErrors: function (errors) {
        
    },

    log: function (msg) {
        if (this._isConsole) {
            console.log(msg);
        }
    }

};
