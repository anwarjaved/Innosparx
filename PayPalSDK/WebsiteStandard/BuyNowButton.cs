using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalSDK.WebsiteStandard
{
    using System.Security;
    using Framework;
    using Framework.Ioc;
    using Framework.Rest.OAuth;
    using Framework.Services;

    public class BuyNowButton : PayPalButton
    {
        public BuyNowButton()
        {
            this.PaymentDetails = new SingleItemPaymentDetails();
        }

        public SingleItemPaymentDetails PaymentDetails { get; private set; }
        
        public override string GetRedirectUrl(string state, string successUrl, string failureUrl)
        {
            if (string.IsNullOrWhiteSpace(this.BusinessEmail))
            {
                throw new InvalidOperationException("BusinessEmail is required");
            }

            var webContext = Container.Get<IWebContext>();
            this.Settings.SuccessUrl = webContext.BuildUrl("payment/gateway/paypal/wps/success?state=" + state);
            this.Settings.CancelUrl = webContext.BuildUrl("payment/gateway/paypal/wps/cancel?state=" + state);
            IDictionary<string, string> dictionary = this.Settings.GetValues();
            dictionary.Add("charset", "utf-8");
            dictionary.Add("business", this.BusinessEmail);

            IOAuthStateManager stateManager = Container.Get<IOAuthStateManager>();
            string key = state;

            OAuthState authState = new OAuthState();
            authState.FailureUrl = failureUrl;
            authState.SuccessUrl = successUrl;
            authState.State = this.BusinessEmail;
            stateManager.SaveState(key, authState);

            dictionary.Add("notify_url", webContext.BuildUrl("payment/gateway/paypal/wps/verifyipn?state=" + state));

            UrlBuilder builder = new UrlBuilder(this.Server.ToDescription());

            builder.QueryString.Add("cmd", "_xclick");
            builder.QueryString.Add("bn", "LM_BuyNow_WPS_IN");

            foreach (KeyValuePair<string, string> setting in dictionary)
            {
                builder.QueryString.Add(setting.Key, setting.Value);
            }

            IDictionary<string, string> dictionary2 = this.PaymentDetails.GetValues();

            foreach (KeyValuePair<string, string> setting in dictionary2)
            {
                builder.QueryString.Add(setting.Key, setting.Value);
            }


            return builder.ToString();
        }

    }
}
