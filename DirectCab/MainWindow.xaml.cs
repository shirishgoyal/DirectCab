using System.Windows;
using PayPal;
using GeoDirections;
using System.Xml;
using System;

namespace DirectCab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string src;
        public string dest;
        public int unit;

        public string distance;
        public string time;
        public string cost;
        public string surcharge;


        System.Collections.ArrayList detailedList;
        
        
        public MainWindow()
        {
            InitializeComponent();          

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            src = txtSrc.Text.Trim();
            dest = txtDest.Text.Trim();
            unit = Convert.ToInt32(txtPriceUnit.Text.Trim());

            GetDistance(src, dest, unit);

            txtDistance.Text = distance;
            txtTime.Text = time;
            txtCost.Text = cost;

            PrepareMap();

            txtCustEmail.Text = "yahoo_1279982396_biz@gmail.com";
            txtCabEmail.Text = "buyer_1279988020_per@gmail.com";
        }

        public void PrepareMap()
        {
            String mapStream="<html><head></head><script type=\"text/javascript\" src=\"http://api.maps.yahoo.com/ajaxymap?v=3.7&appid=YD-4g6HBf0_JX0yq2IsdnV1Ne9JTpKxQ3Miew--\"></script><body><div id=\"mapcontainer\" height=\"400\" width=\"600\"></div>";
            mapStream +="<script type=\"text/javascript\">function initializeMap(){var polylinePoints=new Array();var map=new YMap(document.getElementById('mapcontainer'));map.addTypeControl();map.addPanControl();map.addZoomLong();map.setMapType(YAHOO_MAP_SAT);";

            foreach (GeoDirections.Direction.DirectionEntry entry in detailedList)
            {
                mapStream += "var geoPoint=new YGeoPoint(" + entry.start_lat + "," + entry.start_lng + ");var newMarker=new YMarker(geoPoint);newMarker.addAutoExpand('Distance from Source :" + entry.distance + " metres.<br>Estimated Time :" + entry.timeSpan/60 + " min.');map.addOverlay(newMarker);polylinePoints.push(geoPoint);map.addOverlay(new YPolyline(polylinePoints,'black',2,0.5));";
            }

            mapStream += "map.drawZoomAndCenter('" + src + "',10);}window.onload=function(){initializeMap();}</script></body></html>";

            
            wbMain.NavigateToString(mapStream);

        }

        public void GetDistance(string src,string dest, int unit)
        {
            System.Collections.ArrayList briefList = new System.Collections.ArrayList();
            GeoDirections.Direction.DirectionEntry dirEntry = new GeoDirections.Direction.DirectionEntry();

            detailedList = new System.Collections.ArrayList();
            
            string Url = "http://maps.google.com/maps/api/directions/xml?origin=" + src + "&destination=" + dest + "&sensor=false";

            string xml = Direction.GetWebResponse(Url);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode parentNode = doc.DocumentElement;
            if (parentNode != null)
            {
                XmlNode childNodes = parentNode.SelectSingleNode("/DirectionsResponse/route/leg");
                if (childNodes != null)
                {
                    foreach (XmlNode xmlNode in childNodes)
                    {
                        dirEntry.start_lat = Convert.ToDouble(Direction.GetNodeValue(xmlNode, "start_location/lat"));
                        dirEntry.start_lng = Convert.ToDouble(Direction.GetNodeValue(xmlNode, "start_location/lng"));
                        dirEntry.end_lat = Convert.ToDouble(Direction.GetNodeValue(xmlNode, "end_location/lat"));
                        dirEntry.end_lng = Convert.ToDouble(Direction.GetNodeValue(xmlNode, "end_location/lng"));
                        dirEntry.distance = Direction.GetNumericalValue(xmlNode, "distance/value", 0);
                        dirEntry.timeSpan = Direction.GetNumericalValue(xmlNode, "duration/value", 0);
                        detailedList.Add(dirEntry);

                        if (xmlNode.NextSibling.Name != "step")
                        {
                            break;
                        }
                    }

                    dirEntry.start_lat = Convert.ToDouble(Direction.GetNodeValue(childNodes, "start_location/lat"));
                    dirEntry.start_lng = Convert.ToDouble(Direction.GetNodeValue(childNodes, "start_location/lng"));
                    dirEntry.end_lat = Convert.ToDouble(Direction.GetNodeValue(childNodes, "end_location/lat"));
                    dirEntry.end_lng = Convert.ToDouble(Direction.GetNodeValue(childNodes, "end_location/lng"));
                    dirEntry.distance = Direction.GetNumericalValue(childNodes, "distance/value", 0) / 1000;
                    dirEntry.timeSpan = Direction.GetNumericalValue(childNodes, "duration/value", 0) / 3600;
                    dirEntry.currency = dirEntry.distance * unit;
                    briefList.Add(dirEntry);

                    distance = dirEntry.distance.ToString();
                    time = dirEntry.timeSpan.ToString();
                    cost = dirEntry.currency.ToString();
                }
            }
        }        

        public bool Pay(string custemail, string cabemail, string price)
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
            string sAmount = price;

            // supply your own sandbox accounts for receiver and sender
            string sReceiverEmail = cabemail;// "buyer_1279988020_per@gmail.com";//yahoo_1279982396_biz@gmail.com";
            string sSenderEmail =custemail;//, string "yahoo_1279982396_biz@gmail.com";//buyer_1279988020_per@gmail.com";

            string sTrackingID = System.Guid.NewGuid().ToString();

            return AdaptivePayments.Pay(custemail,cabemail,sAmount);
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            surcharge = txtSurcharge.Text.Trim();
            string price = (Convert.ToInt32(cost)*( 1+ Convert.ToInt32(surcharge)/100)).ToString();
         
            bool response = Pay(txtCabEmail.Text,txtCustEmail.Text,price);

            if (response)
                MessageBox.Show("Transaction successful! Total Amount paid : " + price + " USD");
            else
                MessageBox.Show("The system is under maintenance. Please check later.");
        }

        
    }
}
