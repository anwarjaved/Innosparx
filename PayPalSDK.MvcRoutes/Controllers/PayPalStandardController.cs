using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalSDK.Controllers
{
    using System.Security;
    using System.Web.Mvc;

    using Framework;
    using Framework.Ioc;
    using Framework.Rest;
    using Framework.Rest.OAuth;
    using Framework.Services;

    using PayPalSDK.WebsiteStandard;

    
    [RouteArea("payment/gateway/paypal")]
    [RoutePrefix("wps")]
    public class PayPalStandardController : Controller
    {
        private readonly IWebContext context;
        private readonly IOAuthStateManager stateManager;

        public PayPalStandardController(IWebContext context, IOAuthStateManager stateManager)
        {
            this.context = context;
            this.stateManager = stateManager;
        }

        [Route("success")]
        public ActionResult Success(string state)
        {
            OAuthState authState = this.stateManager.GetState(state);

            if (authState == null)
            {
                throw new InvalidOperationException("Invalid Authorization State");
            }

            UrlBuilder redirectBuilder = new UrlBuilder(authState.SuccessUrl);
            return new RedirectResult(redirectBuilder.ToString());
        }

        [Route("cancel")]
        public ActionResult Cancel(string state)
        {
            OAuthState authState = this.stateManager.GetState(state);

            if (authState == null)
            {
                throw new InvalidOperationException("Invalid Authorization State");
            }

            UrlBuilder redirectBuilder = new UrlBuilder(authState.FailureUrl);
            return new RedirectResult(redirectBuilder.ToString());
        }

        [Route("verifyipn")]
        [HttpPost]
        public ActionResult VerifyIPN(string state)
        {
            OAuthState authState = this.stateManager.GetState(state);

            if (authState == null)
            {
                throw new InvalidOperationException("Invalid Authorization State");
            }

            string businessEmail = authState.State;

            if (!string.IsNullOrWhiteSpace(businessEmail))
            {
                string recieverEmail = this.Request["receiver_email"];

                if (!recieverEmail.IsEmpty() && recieverEmail.EqualsIgnoreCase(businessEmail))
                {
                    ServerType serverType = ServerType.Live;

                    if (this.Request["test_ipn"] != null)
                    {
                        //File.WriteAllText(path, "test_ipn");
                        serverType = ServerType.Live;
                    }

                    string serverUrl = serverType.ToDescription();

                    RestClient client = new RestClient();
                    RestRequest request = new RestRequest(serverUrl, RequestMode.UrlEncoded);

                    request.AddBody("cmd", "_notify-validate");

                    foreach (string postKey in this.Request.Params)
                    {
                        request.AddBody(postKey, this.Request[postKey]);
                    }

                    RestResponse restResponse = client.Post(request);

                    if (restResponse.Completed)
                    {
                        if (restResponse.Content == "VERIFIED")
                        {
                            var handler = Container.TryGet<IIPNProcessor>();

                            if (handler != null)
                            {
                                IPNResponse ipnResponse = new IPNResponse();
                                ipnResponse.BusinessEmail = businessEmail;
                                ipnResponse.ReceiverEmail = this.Request["receiver_email"];
                                ipnResponse.ReceiverID = this.Request["receiver_id"];
                                ipnResponse.TransactionID = this.Request["txn_id"];
                                ipnResponse.TransactionSubject = this.Request["transaction_subject"];
                                ////this.TransactionType = (TransactionType)Reflector.DescriptionToEnum(typeof(TransactionType), this.Context.Request["txn_type"]);
                                ////this.ReceiverCountry = (CountryCode)Reflector.DescriptionToEnum(typeof(CountryCode), this.Context.Request["residence_country"]);
                                ipnResponse.Custom = this.Request["custom"];
                                ipnResponse.ParentTransactionID = this.Request["parent_txn_id"];
                                ipnResponse.Payment.Parse(this.Request.Params);

                                handler.Process(ipnResponse);
                            }
                        }
                    }
                }
            }

            return new EmptyResult();
        }
    }
}
