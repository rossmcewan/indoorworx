using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Models;
using System.Windows.Browser;
using System.IO;
using System.Collections.Generic;

namespace IndoorWorx.Player.Views
{
    public class PlayerPresentationModel : BaseModel, IPlayerPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        public PlayerPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            LoadTelemetry();
            //LoadInformation();
        }

        private void LoadTelemetry()
        {
            Uri dataURI = new Uri(HtmlPage.Document.DocumentUri, "DataSources/telemetry.csv");
            WebClient dataRetriever = new WebClient();
            dataRetriever.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
                (sender,e)=>
                {
                    StringReader dataReader = new StringReader(e.Result);
                    this.BindChart(dataReader);
 
                });
            dataRetriever.DownloadStringAsync(dataURI);
        }




        //private Dictionary<double, ChartData> linked = new Dictionary<double, ChartData>();
        private void BindChart(TextReader dataReader)
        {
        //    //string line;
        //    //int count = 0;

        //    //List<ChartData> chartData = new List<ChartData>();

        //    //while ((line = dataReader.ReadLine()) != null)
        //    //{
        //    //    if (count == 0)
        //    //    {
        //    //        count++;
        //    //        continue;
        //    //    }
        //    //    var elements = line.Split(',');
        //    //    var minutes = TimeSpan.FromMinutes(Convert.ToDouble(elements[0]));
        //    //    var chartDataElement = new ChartData(zeroTime.Add(minutes), double.Parse(elements[3]) / 300.0);
        //    //    chartData.Add(chartDataElement);
        //    //    linked.Add(minutes.TotalSeconds, chartDataElement);
        //    //}

        //    //RadChart1.ItemsSource = chartData;
        //    //RadChart2.ItemsSource = chartData;

        //    //var max = chartData.Max(x => x.YValue);
        //    //RadChart1.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
        //    //RadChart2.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
        //    //radialScale.Min = 0;
        //    //radialScale.MajorTickStep = 20;
        //    //radialScale.Max = max * 100;
        //    //radialScale.Label.Location = Telerik.Windows.Controls.Gauges.ScaleObjectLocation.Outside;
        }

        private TimeSpan playerPosition = new TimeSpan();
        public TimeSpan PlayerPosition
        {
            get { return playerPosition; }
            set
            {
                playerPosition = value;
                FirePropertyChanged("PlayerPosition");
            }

        }

        private IPlayerView view;
        public IPlayerView View
        {
            get
            {
                return this.view;
            }
            set
            {
                this.view = value;
            }
        }

        private VideoText currentVideoText;
        public VideoText CurrentVideoText
        {
            get { return currentVideoText; }
            set
            {
                currentVideoText = value;
                FirePropertyChanged("CurrentVideoText");
            }
        }
    }
}
