
var GsPhalidator = {

    Version: "0.1",

    isValid: function (phone) {
        var p = 0;

        phone = phone.replace(/ /g, '');

        if (phone[0] == '+') {
            p = 1;
        } else if (phone[0] == '(') {
            p = 1;
            // Replace first ')' if phone is like '(8 800) 12345'
            phone = phone.replace(')', '');
        }

        phone = phone.replace('~', '');
        phone = phone.replace('-', '');

        // E.164 allows only 15 numbers
        if (phone.length - p > 15) {
            return false;
        }

        for (var i = p; i < phone.length; i++) {
            if (phone[i] < '0' || phone[i] > '9') {
                return false;
            }
        }
        return true;
    }

}
