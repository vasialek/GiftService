//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GiftService.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class product
    {
        public int id { get; set; }
        public string product_uid { get; set; }
        public int pos_id { get; set; }
        public string pos_user_uid { get; set; }
        public string pay_system_uid { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public decimal product_price { get; set; }
        public string currency_code { get; set; }
        public string customer_name { get; set; }
        public string pos_name { get; set; }
        public string pos_url { get; set; }
        public string pos_city { get; set; }
        public string pos_address { get; set; }
        public string phone_reservation { get; set; }
        public string email_reservation { get; set; }
        public Nullable<System.DateTime> valid_from { get; set; }
        public Nullable<System.DateTime> valid_till { get; set; }
        public string customer_phone { get; set; }
        public string customer_email { get; set; }
        public string remarks { get; set; }
    }
}