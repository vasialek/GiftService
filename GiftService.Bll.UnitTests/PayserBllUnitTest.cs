using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models.Payments;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class PayserBllUnitTest
    {
        private Uri _psPaymentUrl = new Uri("https://www.mokejimai.lt/pay/");
        private PayseraBll _psBll = new PayseraBll();
        private PayseraPaymentRequest _rqValid = new PayseraPaymentRequest();

        // Real for RM
        private const string PayseraPassword = "8ad52eb96c5f3337ba70f9b251310984";
        private const string PayseraProjectId = "76457";

        [TestInitialize]
        public void Init()
        {
            _rqValid = new PayseraPaymentRequest();

            _rqValid.PayseraProjectPassword = "8ad52eb96c5f3337ba70f9b251310984";

            _rqValid.ProjectId = "666";
            _rqValid.OrderId = Guid.NewGuid().ToString("N");

            _rqValid.AcceptUrl = "http://localhost:56620/Ok/";
            _rqValid.CallbackUrl = "http://localhost:56620/Callback/";
            _rqValid.CancelUrl = "http://localhost:56620/Cancel/";

            //_rqValid.Language = PayseraPaymentRequest.Languages.LIT;

            //_rqValid.AmountToPay = 123.45m;
            //_rqValid.CurrencyCode = "EUR";
        }

        [TestMethod]
        public void Test_ValidateSs1_From_Paysera()
        {
            // Real from Paysera
            string rawUrl = "/payment/accept?data=b3JkZXJpZD0xMzJlZDkwOTQwNjA0ZTkxOTI0ODRkMDQ5MmYzYTM1OSZhbW91bnQ9NDMwMCZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1tYiZwX2VtYWlsPXByby5nbGFtZXIlNDBnbWFpbC5jb20mcF9maXJzdG5hbWU9QWxla3NlaitUYWsmcF9sYXN0bmFtZT0mcF9waG9uZT0lMkIzNzArNjAwKzEyMzQ1JnBfY29tbWVudD0mcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmcGF5dGV4dD1SaXRvc01hc2F6YWkubHQrLStLJUM1JUFCbm8rJUM1JUExdmVpdGltYXMrc3UrbWVkdW1pK2lyK2dpbnRhcm8rc20lQzQlOTdsaXUrJTI4MzArbWluJTI5KyVDNCVBRnZ5bmlvamltYXMraXIrYXRwYWxhaWR1b2phbnRpcyt2aXNvK2suK0p1c3UrdXpzYWt5bWFzK2h0dHAlM0ElMkYlMkZ3d3cuZG92YW51a3Vwb25haS5jb20lMkZnaWZ0JTJGZ2V0JTJGMTMyZWQ5MDk0MDYwNGU5MTkyNDg0ZDA0OTJmM2EzNTkuK0Rla3VvanUlMkMrUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3Jmxhbmc9bGl0Jm1fcGF5X3Jlc3RvcmVkPTkxMDEwMzk5JnN0YXR1cz0xJnJlcXVlc3RpZD05MTAxMDM5OSZwYXlhbW91bnQ9NDMwMCZwYXljdXJyZW5jeT1FVVI%3D&ss1=22517d27ea349e56eb3e0712b8a43301&ss2=S1LdqbJgXWtRxyuL2-4PDF4vLBo04F177 7cmyBGV6f6woY4984jUfRXKc1tMGruMYoB3u5HEhAQEr-k24yK864e_Wjk_9GCJhehKBikioJ0dh19dmMJ6f50zUGe_zbT64RL2phR9dNNIDi0PLt-6LWn6mZXfo-3_PXt932lRKMc%3D";
            string data = "b3JkZXJpZD0xMzJlZDkwOTQwNjA0ZTkxOTI0ODRkMDQ5MmYzYTM1OSZhbW91bnQ9NDMwMCZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1tYiZwX2VtYWlsPXByby5nbGFtZXIlNDBnbWFpbC5jb20mcF9maXJzdG5hbWU9QWxla3NlaitUYWsmcF9sYXN0bmFtZT0mcF9waG9uZT0lMkIzNzArNjAwKzEyMzQ1JnBfY29tbWVudD0mcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmcGF5dGV4dD1SaXRvc01hc2F6YWkubHQrLStLJUM1JUFCbm8rJUM1JUExdmVpdGltYXMrc3UrbWVkdW1pK2lyK2dpbnRhcm8rc20lQzQlOTdsaXUrJTI4MzArbWluJTI5KyVDNCVBRnZ5bmlvamltYXMraXIrYXRwYWxhaWR1b2phbnRpcyt2aXNvK2suK0p1c3UrdXpzYWt5bWFzK2h0dHAlM0ElMkYlMkZ3d3cuZG92YW51a3Vwb25haS5jb20lMkZnaWZ0JTJGZ2V0JTJGMTMyZWQ5MDk0MDYwNGU5MTkyNDg0ZDA0OTJmM2EzNTkuK0Rla3VvanUlMkMrUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3Jmxhbmc9bGl0Jm1fcGF5X3Jlc3RvcmVkPTkxMDEwMzk5JnN0YXR1cz0xJnJlcXVlc3RpZD05MTAxMDM5OSZwYXlhbW91bnQ9NDMwMCZwYXljdXJyZW5jeT1FVVI=";
            string ss1 = "22517d27ea349e56eb3e0712b8a43301";

            _psBll.ValidateSs1(PayseraPassword, data, ss1);
        }

        [TestMethod]
        public void Test_DovanuKuponai_Payment_Link_As_Test()
        {
            var rq = new PayseraPaymentRequest();
            rq.PayseraProjectPassword = PayseraPassword;
            rq.ProjectId = PayseraProjectId;

            rq.OrderId = Guid.NewGuid().ToString("N");
            rq.AmountToPay = 0.01m;
            rq.CurrencyCode = "EUR";
            rq.Country = PayseraPaymentRequest.Countries.LT;

            rq.AcceptUrl = "http://www.dovanukuponai.com/payment/accept/";
            rq.CancelUrl = "http://www.dovanukuponai.com/payment/cancel/";
            rq.CallbackUrl = "http://www.dovanukuponai.com/payment/callback/";

            rq.CustomerName = "Aleksej";
            rq.CustomerLastName = "Vasinov";
            rq.CustomerEmail = "it@interateitis.lt";
            rq.CustomerPhone = "+370 5 2166481";
            rq.Remarks = "Just test payment";

            rq.PayText = "RitosMasazai.lt [owner_name]. Jusu uzsakymas http://www.dovanukuponai.com/gift/get/[order_nr]";

            Uri paymentLink = _psBll.PreparePaymentLink(_psPaymentUrl, rq);
            // To get exception and see result :)
            Assert.AreEqual("-", paymentLink.AbsoluteUri);
        }

        [TestMethod]
        public void Test_Parse_Real_Payser_Response()
        {
            // Real response from Paysera
            string dataFromPaysera = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(dataFromPaysera);

            // Known magick numbers :)
            Assert.AreEqual(0.01m, _rqValid.AmountToPay);
        }

        [TestMethod]
        public void Test_DovanuKuponai_Payment_Link_By_WebToPay()
        {
            string goodData = "b3JkZXJpZD0xMjM0NTY3ODkwJmFtb3VudD0xMjM0NSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZhY2NlcHR1cmw9aHR0cCUzQSUyRiUyRnd3dy5kb3ZhbnVrdXBvbmFpLmNvbSUyRnBheW1lbnQlMkZhY2NlcHQlMkYmY2FuY2VsdXJsPWh0dHAlM0ElMkYlMkZ3d3cuZG92YW51a3Vwb25haS5jb20lMkZwYXltZW50JTJGY2FuY2VsJmNhbGxiYWNrdXJsPWh0dHAlM0ElMkYlMkZ3d3cuZG92YW51a3Vwb25haS5jb20lMkZwYXltZW50JTJGY2FsbGJhY2smdGVzdD0xJnBheW1lbnQ9cGF5bWVudCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPVRlc3QmcF9sYXN0bmFtZT1JQSZwX3Bob25lPSUyQjM3MCs2MDArMTIzNDUmcF9jb21tZW50PWNvbW1lbnQmcF9pcD0lM0ElM0ExJnBfYWdlbnQ9TW96aWxsYSUyRjUuMCslMjhXaW5kb3dzK05UKzEwLjAlM0IrV09XNjQlM0IrcnYlM0E0Mi4wJTI5K0dlY2tvJTJGMjAxMDAxMDErRmlyZWZveCUyRjQyLjAmcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTc=";
            string goodSign = "78f862fcddbc175a55dd5651410a7df1";

            var rq = new PayseraPaymentRequest();
            rq.PayseraProjectPassword = PayseraPassword;
            rq.ProjectId = PayseraProjectId;

            rq.OrderId = "1234567890";
            rq.AmountToPay = 123.45m;
            rq.CurrencyCode = "EUR";
            rq.Country = PayseraPaymentRequest.Countries.LT;

            rq.AcceptUrl = "http://www.dovanukuponai.com/payment/accept/";
            rq.CancelUrl = "http://www.dovanukuponai.com/payment/cancel";
            rq.CallbackUrl = "http://www.dovanukuponai.com/payment/callback";

            rq.IsTestPayment = true;
            rq.RequestedPaymentMehtods = "payment";

            rq.CustomerEmail = "it@interateitis.lt";
            rq.CustomerName = "Test";
            rq.CustomerLastName = "IA";
            rq.CustomerPhone = "+370 600 12345";
            rq.Remarks = "comment";

            rq.Ip = "::1";
            rq.BrowserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";
            rq.File = ".";

            string data = _psBll.EncodeString(_psBll.JoinPaymentParameters(rq));
            Assert.AreEqual(goodData, data);
            string sign1 = _psBll.GenerateSs1(data, rq.PayseraProjectPassword);
            Assert.AreEqual(goodSign, sign1);

            Uri paymentUrl = _psBll.PreparePaymentLink(new Uri("https://www.mokejimai.lt/pay/"), rq);
        }

        [TestMethod]
        public void Test_Http_Encode()
        {
            string s = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";
            string phpEncoded = "Mozilla%2F5.0+%28Windows+NT+10.0%3B+WOW64%3B+rv%3A42.0%29+Gecko%2F20100101+Firefox%2F42.0";

            //string netEncoded = System.Net.WebUtility.UrlEncode(s);
            //netEncoded = netEncoded.Replace("(", "%28")
            //    .Replace(")", "%29");

            Assert.AreEqual(phpEncoded, _psBll.UrlEncode(s));
        }

        [TestMethod]
        public void Test_Check_Prepared_Payment_Link()
        {
            string goodData = "b3JkZXJpZD0wJmFtb3VudD0xMjM0NSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZhY2NlcHR1cmw9JTJGYWNjZXB0LnBocCZjYW5jZWx1cmw9JTJGY2FuY2VsLnBocCZjYWxsYmFja3VybD0lMkZjYWxsYmFjay5waHAmdGVzdD0wJnBheW1lbnQ9cGF5bWVudCZwX2VtYWlsPWVtYWlsJnBfZmlyc3RuYW1lPWZpcnN0bmFtZSZwX2xhc3RuYW1lPWxhc3RuYW1lJnBfcGhvbmU9cGhvbmUmcF9jb21tZW50PWNvbW1lbnQmcF9pcD0lM0ElM0ExJnBfYWdlbnQ9TW96aWxsYSUyRjUuMCslMjhXaW5kb3dzK05UKzEwLjAlM0IrV09XNjQlM0IrcnYlM0E0Mi4wJTI5K0dlY2tvJTJGMjAxMDAxMDErRmlyZWZveCUyRjQyLjAmcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzMwOTc=";

            //goodData = "b3JkZXJpZD0wJmFtb3VudD0xMjM0NSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZhY2NlcHR1cmw9JTJGYWNjZXB0LnBocCZjYW5jZWx1cmw9JTJGY2FuY2VsLnBocCZjYWxsYmFja3VybD0lMkZjYWxsYmFjay5waHAmdGVzdD0wJnBheW1lbnQ9cGF5bWVudCZwX2VtYWlsPWVtYWlsJnBfZmlyc3RuYW1lPWZpcnN0bmFtZSZwX2xhc3RuYW1lPWxhc3RuYW1lJnBfcGhvbmU9cGhvbmUmcF9jb21tZW50PWNvbW1lbnQmcF9pcD0lM0ElM0ExJnBfYWdlbnQ9TW96aWxsYSUyRjUuMCslMjhXaW5kb3dzK05UKzEwLjAlM0IrV09XNjQlM0IrcnYlM0E0Mi4wJTI5K0dlY2tvJTJGMjAxMDAxMDErRmlyZWZveCUyRjQyLjAmcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzMwOTc";

            var rq = new PayseraPaymentRequest();
            rq.OrderId = "0";
            rq.AmountToPay = 123.45m;
            rq.CurrencyCode = "EUR";
            rq.Country = PayseraPaymentRequest.Countries.LT;
            rq.AcceptUrl = "/accept.php";
            rq.CancelUrl = "/cancel.php";
            rq.CallbackUrl = "/callback.php";
            rq.IsTestPayment = false;
            rq.RequestedPaymentMehtods = "payment";
            rq.CustomerEmail = "email";
            rq.CustomerName = "firstname";
            rq.CustomerLastName = "lastname";
            rq.CustomerPhone = "phone";
            rq.Remarks = "comment";

            rq.Ip = "::1";
            rq.BrowserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";

            rq.File = "";
            rq.ProjectId = "73097";

            string data = _psBll.EncodeString(_psBll.JoinPaymentParameters(rq));

            Assert.AreEqual(goodData, data);
        }

        [TestMethod]
        public void Test_Check_Prepared_Payment_Sign1()
        {
            string goodSign1 = "524b0c22694888382a9698ee18a8c429";

            var rq = new PayseraPaymentRequest();
            rq.PayseraProjectPassword = "3cdd5d87d8de847170314268ced43126";

            rq.OrderId = "0";
            rq.AmountToPay = 123.45m;
            rq.CurrencyCode = "EUR";
            rq.Country = PayseraPaymentRequest.Countries.LT;
            rq.AcceptUrl = "/accept.php";
            rq.CancelUrl = "/cancel.php";
            rq.CallbackUrl = "/callback.php";
            rq.IsTestPayment = false;
            rq.RequestedPaymentMehtods = "payment";
            rq.CustomerEmail = "email";
            rq.CustomerName = "firstname";
            rq.CustomerLastName = "lastname";
            rq.CustomerPhone = "phone";
            rq.Remarks = "comment";

            rq.Ip = "::1";
            rq.BrowserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";

            rq.File = "";
            rq.ProjectId = "73097";

            string data = "b3JkZXJpZD0wJmFtb3VudD0xMjM0NSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZhY2NlcHR1cmw9JTJGYWNjZXB0LnBocCZjYW5jZWx1cmw9JTJGY2FuY2VsLnBocCZjYWxsYmFja3VybD0lMkZjYWxsYmFjay5waHAmdGVzdD0wJnBheW1lbnQ9cGF5bWVudCZwX2VtYWlsPWVtYWlsJnBfZmlyc3RuYW1lPWZpcnN0bmFtZSZwX2xhc3RuYW1lPWxhc3RuYW1lJnBfcGhvbmU9cGhvbmUmcF9jb21tZW50PWNvbW1lbnQmcF9pcD0lM0ElM0ExJnBfYWdlbnQ9TW96aWxsYSUyRjUuMCslMjhXaW5kb3dzK05UKzEwLjAlM0IrV09XNjQlM0IrcnYlM0E0Mi4wJTI5K0dlY2tvJTJGMjAxMDAxMDErRmlyZWZveCUyRjQyLjAmcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzMwOTc=";

            string sign1 = _psBll.GenerateSs1(data, rq.PayseraProjectPassword);

            Assert.AreEqual(goodSign1, sign1);
        }

        [TestMethod]
        public void Test_Prepare_Payment_Redirect()
        {
            var url = _psBll.PreparePaymentLink(_psPaymentUrl, _rqValid);

            Assert.IsNotNull(url);
            Assert.IsTrue(url.Query.IndexOf("data=") > 0);
            Assert.IsTrue(url.Query.IndexOf("sign=") > 0);
        }

        #region Parse parameters from Paysera

        [TestMethod]
        public void Test_ParseHttpParams_Status()
        {
            string status = "1";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(status, resp.Status);
        }

        [TestMethod]
        public void Test_IsTestPayment_By_Default_Is_True()
        {
            var resp = new PayseraPaymentResponse();
            Assert.IsTrue(resp.IsTestPayment);
        }

        [TestMethod]
        public void Test_ParseHttpParams_IsTestPayment()
        {
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.IsTrue(resp.IsTestPayment);
        }

        [TestMethod]
        public void Test_ParseHttpParams_OrderId()
        {
            string orderId = "bfaafc61900b456c87310cc8756d80ab";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(orderId, resp.OrderId);
        }

        [TestMethod]
        public void Test_ParseHttpParams_ProjectId()
        {
            string projectId = "76457";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(projectId, resp.ProjectId);
        }

        [TestMethod]
        public void Test_ParseHttpParams_Amount()
        {
            decimal amountRequested = 0.01m;
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(amountRequested, resp.AmountToPay);
        }

        [TestMethod]
        public void Test_ParseHttpParams_PayAmount()
        {
            decimal amountPaid = 0.01m;
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(amountPaid, resp.PayAmount);
        }

        [TestMethod]
        public void Test_ParseHttpParams_CurrencyCode()
        {
            string currencyCode = "EUR";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(currencyCode, resp.CurrencyCode);
        }

        [TestMethod]
        public void Test_ParseHttpParams_PayCurrencyCode()
        {
            string currencyCode = "EUR";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(currencyCode, resp.PayCurrencyCode);
        }

        [TestMethod]
        public void Test_ParseHttpParams_CustomerName()
        {
            string name = "Aleksej";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(name, resp.CustomerName);
        }

        [TestMethod]
        public void Test_ParseHttpParams_CustomerLastName()
        {
            string name = "Vasinov";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(name, resp.CustomerLastName);
        }

        [TestMethod]
        public void Test_ParseHttpParams_CustomerEmail()
        {
            string s = "it@interateitis.lt";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(s, resp.CustomerEmail);
        }

        [TestMethod]
        public void Test_ParseHttpParams_CustomerPhone()
        {
            string s = "+370 5 2166481";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(s, resp.CustomerPhone);
        }

        [TestMethod]
        public void Test_ParseHttpParams_Remarks()
        {
            string remarks = "Just test payment";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(remarks, resp.Remarks);
        }

        [TestMethod]
        public void Test_ParseHttpParams_PayText()
        {
            string orderId = "bfaafc61900b456c87310cc8756d80ab";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.IsTrue(resp.PayText.IndexOf(orderId) >= 0);
        }

        [TestMethod]
        public void Test_ParseHttpParams_PayText_Custome()
        {
            // Real from Payser
            string data = "?data=b3JkZXJpZD0xMzJkMmYzMDU3NzQ0YzEzYWMxYWU2MjYwOWExMGI0ZiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD12YjImcF9lbWFpbD1pdCU0MGludGVyYXRlaXRpcy5sdCZwX2ZpcnN0bmFtZT1BbGVrc2VqJnBfbGFzdG5hbWU9VmFzaW5vdiZwX3Bob25lPSUyQjM3MCs1KzIxNjY0ODEmcF9jb21tZW50PUp1c3QrdGVzdCtwYXltZW50JnBfaXA9JnBfYWdlbnQ9JnBfZmlsZT0mdmVyc2lvbj0xLjYmcHJvamVjdGlkPTc2NDU3JnBheXRleHQ9MTMyZDJmMzA1Nzc0NGMxM2FjMWFlNjI2MDlhMTBiNGYrUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3Jmxhbmc9bGl0Jm1fcGF5X3Jlc3RvcmVkPTkxMDA4NjI5JnN0YXR1cz0xJnJlcXVlc3RpZD05MTAwODYyOSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual("-", resp.PayText);
        }

        [TestMethod]
        public void Test_ParseHttpParams_Payment()
        {
            // Which bank handles payment
            string payment = "nordealt";
            // Real from Paysera
            string data = "?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D";

            var resp = _psBll.ParseData(data);

            Assert.AreEqual(payment, resp.Payment);
        }

        #endregion

        [TestMethod]
        public void Test_Join_Payment_Request()
        {
            string s = _psBll.JoinPaymentParameters(_rqValid);
        }

        [TestMethod]
        public void Test_JoinPaymentRequest_Escape_Bad_Symbols()
        {
            _rqValid.CustomerCity = "%=&";

            string s = _psBll.JoinPaymentParameters(_rqValid);

            int pos = s.IndexOf("CustomerCity=");

            string escaped = s.Substring(pos + "CustomerCity=".Length);
            Assert.IsTrue(escaped.StartsWith("%25%3D%26"));
        }

        [TestMethod]
        public void Test_Validate_Good_Payment_Request()
        {
            _psBll.ValidatePaymentRequest(_rqValid);
        }

        [TestMethod]
        public void Test_Validate_ProjectId_Not_Set()
        {
            _rqValid.ProjectId = String.Empty;

            Assert.IsTrue(WasExceptionForProperty(_rqValid, "ProjectId"));
        }

        [TestMethod]
        public void Test_Validate_OrderId_Is_Set()
        {
            _rqValid.OrderId = String.Empty;

            Assert.IsTrue(WasExceptionForProperty(_rqValid, "OrderId"));
        }

        [TestMethod]
        public void Test_Validate_AcceptUrl_Is_Set()
        {
            _rqValid.AcceptUrl = null;

            Assert.IsTrue(WasExceptionForProperty(_rqValid, "AcceptUrl"));
        }

        [TestMethod]
        public void Test_Validate_CancelUrl_Is_Set()
        {
            _rqValid.CancelUrl = null;

            Assert.IsTrue(WasExceptionForProperty(_rqValid, "CancelUrl"));
        }


        [TestMethod]
        public void Test_Validate_CallbackUrl_Is_Set()
        {
            _rqValid.CallbackUrl = null;

            Assert.IsTrue(WasExceptionForProperty(_rqValid, "CallbackUrl"));
        }

        private bool WasExceptionForProperty(PayseraPaymentRequest rq, string propertyName)
        {
            try
            {
                _psBll.ValidatePaymentRequest(rq);
            }
            catch (ArgumentNullException anex)
            {
                return anex.ParamName == propertyName;
            }
            return false;
        }
    }
}
