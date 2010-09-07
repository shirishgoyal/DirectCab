using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;

//  ****************************************************************
//  THIS IS STRICTLY EXAMPLE SOURCE CODE. IT IS ONLY MEANT TO 
//  QUICKLY DEMONSTRATE THE CONCEPT AND THE USAGE OF THE ADAPTIVE 
//  PAYMENTS API. PLEASE NOTE THAT THIS IS *NOT* PRODUCTION-QUALITY 
//  CODE AND SHOULD NOT BE USED AS SUCH.
//
//  THIS EXAMPLE CODE IS PROVIDED TO YOU ONLY ON AN "AS IS" 
//  BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER 
//  EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY WARRANTIES 
//  OR CONDITIONS OF TITLE, NON-INFRINGEMENT, MERCHANTABILITY OR 
//  FITNESS FOR A PARTICULAR PURPOSE. PAYPAL MAKES NO WARRANTY THAT 
//  THE SOFTWARE OR DOCUMENTATION WILL BE ERROR-FREE. IN NO EVENT 
//  SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY 
//  DIRECT, INDIRECT, INCIDENTAL, SPECIAL,  EXEMPLARY, OR 
//  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
//  OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
//  OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
//  THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
//  OF SUCH DAMAGE.
//  ****************************************************************


namespace PayPal
{
    class AdaptivePayments
    {
        public static bool Pay(string sReceiverEmail, string sSenderEmail, string sAmount )
        {           

            // API Credentials - supply your own sandbox credentials
            string sAPIUser = "yahoo_1279982396_biz_api1.gmail.com";
            string sAPIPassword = "1279982401";
            string sAPISignature = "AX-618aD4V6oDw6yoaVy.PPvwH0wAjTDlYtfd55NBXLcsUYhRH-o4VvZ";

            // API endpoint for the Pay call in the Sandbox
            string sAPIEndpoint = "https://svcs.sandbox.paypal.com/AdaptivePayments/Pay";

            // Version that you are coding against
            string sVersion = "1.1.0";

            // Error Langugage
            string sErrorLangugage = "en_US";

            // Detail Level
            string sDetailLevel = "ReturnAll";

            // Request Data Binding
            string sRequestDataBinding = "NV";

            // Response Data Binding
            string sResponseDataBinding = "NV";

            // Application ID
            string sAppID = "APP-80W284485P519543T";

            // other clientDetails fields
            string sIpAddress = "255.255.255.255";
            string sPartnerName = "Team - Righteous";
            string sDeviceID = "255.255.255.255";

            // Currency Code
            string sCurrencyCode = "USD";

            // Action Type
            string sActionType = "PAY";

            // ReturnURL and CancelURL used for approval flow
            string sReturnURL = "https://www.sandbox.paypal.com/us/cgi-bin/webscr?cmd=_account";
            string sCancelURL = "https://MyCancelURL";

            // who pays the fees
            string sFeesPayer = "EACHRECEIVER";

            // memo field
            string sMemo = "testing my first pay call";

            // transaction amount
            //string sAmount = "1000";

            // supply your own sandbox accounts for receiver and sender
            //string sReceiverEmail = "buyer_1279988020_per@gmail.com";//yahoo_1279982396_biz@gmail.com";
            //string sSenderEmail = "yahoo_1279982396_biz@gmail.com";//buyer_1279988020_per@gmail.com";

            string sTrackingID = System.Guid.NewGuid().ToString();

            // construct the name value request string
            // be sure to UrlEncode all values
            StringBuilder sRequest = new StringBuilder();
            // requestEnvelope fields
            sRequest.Append("requestEnvelope.errorLanguage=");
            sRequest.Append(Uri.EscapeUriString(sErrorLangugage));
            sRequest.Append("&requestEnvelope.detailLevel=");
            sRequest.Append(Uri.EscapeUriString(sDetailLevel));
            // clientDetails fields
            sRequest.Append("&clientDetails.applicationId=");
            sRequest.Append(Uri.EscapeUriString(sAppID));
            sRequest.Append("&clientDetails.deviceId=");
            sRequest.Append(Uri.EscapeUriString(sDeviceID));
            sRequest.Append("&clientDetails.ipAddress=");
            sRequest.Append(Uri.EscapeUriString(sIpAddress));
            sRequest.Append("&clientDetails.partnerName=");
            sRequest.Append(Uri.EscapeUriString(sPartnerName));
            // request specific data fields
            sRequest.Append("&actionType=");
            sRequest.Append(Uri.EscapeUriString(sActionType));
            sRequest.Append("&cancelUrl=");
            sRequest.Append(Uri.EscapeUriString(sCancelURL));
            sRequest.Append("&returnUrl=");
            sRequest.Append(Uri.EscapeUriString(sReturnURL));
            sRequest.Append("&currencyCode=");
            sRequest.Append(Uri.EscapeUriString(sCurrencyCode));
            sRequest.Append("&feesPayer=");
            sRequest.Append(Uri.EscapeUriString(sFeesPayer));
            sRequest.Append("&memo=");
            sRequest.Append(Uri.EscapeUriString(sMemo));
            sRequest.Append("&receiverList.receiver(0).amount=");
            sRequest.Append(Uri.EscapeUriString(sAmount));
            sRequest.Append("&receiverList.receiver(0).email=");
            sRequest.Append(Uri.EscapeUriString(sReceiverEmail));
            sRequest.Append("&senderEmail=");
            sRequest.Append(Uri.EscapeUriString(sSenderEmail));
            sRequest.Append("&trackingId=");
            sRequest.Append(Uri.EscapeUriString(sTrackingID));

            try
            {
                // get ready to make the call
                HttpWebRequest oPayRequest = (HttpWebRequest)WebRequest.Create(sAPIEndpoint);
                oPayRequest.Method = "POST";
                oPayRequest.ContentLength = sRequest.Length;
                oPayRequest.ContentType = "application/x-www-form-urlencoded";
                // set the HTTP Headers
                oPayRequest.Headers.Add("X-PAYPAL-SECURITY-USERID", sAPIUser);
                oPayRequest.Headers.Add("X-PAYPAL-SECURITY-PASSWORD", sAPIPassword);
                oPayRequest.Headers.Add("X-PAYPAL-SECURITY-SIGNATURE", sAPISignature);
                oPayRequest.Headers.Add("X-PAYPAL-SERVICE-VERSION", sVersion);
                oPayRequest.Headers.Add("X-PAYPAL-APPLICATION-ID", sAppID);
                oPayRequest.Headers.Add("X-PAYPAL-REQUEST-DATA-FORMAT", sRequestDataBinding);
                oPayRequest.Headers.Add("X-PAYPAL-RESPONSE-DATA-FORMAT", sResponseDataBinding);
                // send the request
                StreamWriter oStreamWriter = new StreamWriter(oPayRequest.GetRequestStream());
                oStreamWriter.Write(sRequest.ToString());
                oStreamWriter.Close();
                // get the response
                HttpWebResponse oPayResponse = (HttpWebResponse)oPayRequest.GetResponse();
                StreamReader oStreamReader = new StreamReader(oPayResponse.GetResponseStream());
                string sResponse = oStreamReader.ReadToEnd();
                oStreamReader.Close();
                // write the response string to the console

                if (Uri.EscapeUriString(sResponse).Contains("COMPLETED"))
                    return true;
                else
                    return false;


            }
            catch (Exception e)
            {
                
            }
            return false;
            
        }
    }
}
