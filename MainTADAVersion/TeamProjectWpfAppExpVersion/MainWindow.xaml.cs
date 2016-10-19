using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamProjectWpfAppExpVersion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Conrads Version2
        RESTHandler objRest;
        Response.RootObject myResponse;

        String Description, BuyNow, Price, BidCount, Dealer,Flags, mainDetails,Details, cityDetail, tempDetail, yearDescription, modelDescription;
        String Body, Odometer, Engine, Transmission, EngineType;
        List<Response.Group> groupList;
        List<Response.Detail> detailList;

        public MainWindow()
        {
            InitializeComponent();

            PerformOperation();
        }

        public string RemoveTail(string initalstring)
        {
            initalstring = initalstring.Remove(initalstring.Length - 2);
            return initalstring;

        }

        public string CleansePrice(string initalstring)
        {
            StringBuilder sb = new StringBuilder(initalstring);
            sb.Remove(0, 1);
            initalstring = sb.ToString();
            initalstring = initalstring.Replace(",", "");
            return initalstring;
        }

        public void PerformOperation()
        {
            objRest = new RESTHandler(@"https://extraction.import.io/query/extractor/cc2ec580-d520-44e4-9bd6-0048f8c2f663?_apikey=a759e60c74034f7f839fefaf97502e5a6bc346bf84931f5c90d436954961631a25086cb8866a99ae3dd9f2cf60d7ee0e7586aa58a38c227f1b7eede0b17e03d17c511383803d0abbb81d0d1587392d6a&url=http%3A%2F%2Fwww.trademe.co.nz%2FBrowse%2FSearchResults.aspx%3Fsort_order%3Dbids_asc%26from%3Dadvanced%26advanced%3Dtrue%26searchstring%3D2004%26current%3D0%26cid%3D268%26rptpath%3D1-268-%26searchregion%3D100");

            myResponse = objRest.ExecuteCurrentRequest();

            //Check if group is null
            if (myResponse.extractorData.data[0].group != null)
            {
                //populate groupList with all groups.
                groupList = myResponse.extractorData.data[0].group;
            }

            List<VehicleListing> Vehicles = new List<VehicleListing>();

            //Loop through all groups.
            for (int i = 0; i < groupList.Count; i++)
            {
                //Description operation.
                if (myResponse.extractorData.data[0].group[i].Description != null)
                {
                    Description = myResponse.extractorData.data[0].group[i].Description[0].text;

                    string[] myDescriptions = Description.Split(' ');
                    yearDescription = myDescriptions[(myDescriptions.Count()-1)];
                    modelDescription = myDescriptions[0];

                    Description = "";
                    for(int x=1; x < (myDescriptions.Count()-1);x++)
                    {
                        Description = Description + " "+ myDescriptions[x]; 
                    }
                }

                //BuyNow operation.
                if (myResponse.extractorData.data[0].group[i].BuyNow != null)
                {
                    BuyNow = myResponse.extractorData.data[0].group[i].BuyNow[0].text;
                    string[] BuyNowDollars = BuyNow.Split(' ');
                    BuyNow = BuyNowDollars[0];
                    BuyNow = CleansePrice(BuyNow);
                }

                //Price operation.
                if (myResponse.extractorData.data[0].group[i].Price != null)
                {
                    Price = myResponse.extractorData.data[0].group[i].Price[0].text;
                   
                    Price = CleansePrice(Price);
                }

                //BidCount operation.
                if (myResponse.extractorData.data[0].group[i].BidCount != null)
                {
                    BidCount = myResponse.extractorData.data[0].group[i].BidCount[0].text;
                    string[] BidNumbers = BidCount.Split(null);
                    BidCount = BidNumbers[0];
                }
             
                //Dealer operation.
                if (myResponse.extractorData.data[0].group[i].Dealer != null)
                {
                    string myDealer = myResponse.extractorData.data[0].group[i].Dealer[0].alt;

                    if (myDealer == "Dealer listing")
                    {
                        Dealer = "1";
                    }

                }
                else
                {
                    Dealer = "0";
                }            
                
                //Flags operation.
                if (myResponse.extractorData.data[0].group[i].Flags != null)
                {
                    string myFlags = myResponse.extractorData.data[0].group[i].Flags[0].alt;

                    if (myFlags == "Reserve Met")
                    {
                        Flags = "1";
                    }
                    else if (myFlags == "No Reserve")
                    {
                        Flags = "0";
                    }
                }

                //Perform details operation
                if (myResponse.extractorData.data[0].group[i].Details != null)
                {

                    detailList = myResponse.extractorData.data[0].group[i].Details;
                    Details = "";

                    mainDetails = myResponse.extractorData.data[0].group[i].Details[0].text;

                    String[] mainTempArray = mainDetails.Split(',');

                    Body = mainTempArray[2];
                    Odometer = mainTempArray[0] + mainTempArray[1];
                    Odometer = RemoveTail(Odometer);
                    Engine = mainTempArray[3];
                    Transmission = mainTempArray[4];

                    //clean Engine
                    //MessageBox.Show(Engine);
                    String[] engineArray = Engine.Split(' ');
                    Engine = engineArray[1];
                    Engine = RemoveTail(Engine);
                    EngineType = engineArray[2] + ' ' + engineArray[3];

                    //clean second detail
                    tempDetail = myResponse.extractorData.data[0].group[i].Details[1].text;

                    String[] tempArray = tempDetail.Split(',');

                    cityDetail = tempArray[0];

                }

                Vehicles.Add(new VehicleListing() { VehicleModel= modelDescription,VehicleDescription = Description,VehicleYear = yearDescription, VehicleBuyNow = BuyNow, VehiclePrice = Price, VehicleBody = Body, VehicleOdometer = Odometer, VehicleEngineSize = Engine, VehicleEngineType = EngineType, VehicleTransmission = Transmission, VehicleBidCount = BidCount, VehicleDealer = Dealer,  VehicleFlags = Flags, /*VehicleDetails = mainDetails,*/ VehicleCity = cityDetail });

            }
                dataGrid.ItemsSource = Vehicles;                                   
            
        }
    }
}


