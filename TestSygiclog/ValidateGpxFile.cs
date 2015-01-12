//-----------------------------------------------------------------------
// <copyright file="SygicTravelbookLog.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Globalization; // Culture
using System.IO; // File Path
using System.Xml;

namespace TestSygiclog
{
    class ValidateGpxFile
    {
        /// <summary>
        /// indicate if the validation passed
        /// </summary>
        private readonly bool passed;

        /// <summary>
        /// contains a possible error string
        /// </summary>
        private readonly string error;

        private XmlDocument _xmlDoc;
        private XmlNode _firstTrackPoint;
        private int _trackpoints;

         /// <summary>
        /// Initializes a new instance of the <see cref="ValidateGpxFile" /> class.
        /// The constructor opens the GPX file and loops it to find all features in one go.
        /// If the file could not be opened _enabled is false and _error contains the error message.
        /// </summary>
        /// <param name="fullLogFilename">The full text log file name with extension</param>
        public ValidateGpxFile(string fullLogFilename, string xmlExtension)
        {
            this.passed = false;

            //relative path
            string gpxfilename = Path.GetFileNameWithoutExtension(fullLogFilename) + xmlExtension;

            try
            {
                 _xmlDoc = new XmlDocument();
                 _xmlDoc.Load(gpxfilename);
                 this.passed = true;
            }
            catch (Exception e)
            {
                 Console.WriteLine("GpxFile not found:" + e.Message);
                 _xmlDoc = null;
                 this.passed = false;
                 this.error = e.Message;
                 
            }                        
        } // ValidateGpxFile constructor reader


        public string GetTrkName()
        {
            string name = ""; //default empty
            XmlNodeList nameNode = _xmlDoc.GetElementsByTagName("name");
            if (nameNode.Count >0)
            {                
                name = nameNode[0].InnerText;
            }
            return name;
        }

        public bool IsGpxExt()
        {
            bool isGpxExt = false; //default not extensions
            XmlNodeList gpxext = _xmlDoc.GetElementsByTagName("extensions");
            if (gpxext.Count > 0)
            {
                isGpxExt = true;
            }
            return isGpxExt;
        }

        public XmlNode GetFirstTrackPoint()
        {
            if (_firstTrackPoint == null)
            {
                XmlNodeList trackPointList = _xmlDoc.GetElementsByTagName("trkpt");
                _firstTrackPoint = trackPointList[0];
                _trackpoints = trackPointList.Count;
            }
            return _firstTrackPoint;
        }

        public float FirstTrackpointSpeed()
        {
            float speed = -1.0f;

            XmlNode firstTrackPoint = GetFirstTrackPoint();
           
            //XmlNodeList speedNodeList =  //GetElementsByTagName("gpxext:Speed");
            foreach (XmlNode trkNode in firstTrackPoint.ChildNodes)
            {
                if (trkNode.Name == "extensions")
                {
                    foreach (XmlNode extNode in trkNode.FirstChild.ChildNodes)
                    {
                        if (extNode.Name == "gpxext:Speed")
                        {
                            string speedString = extNode.InnerText;
                            float.TryParse(speedString, out speed);
                            break;
                        }                     
                    }
                }
            }
            return speed;
        }

        public string FirstTrackpointTime()
        {
            string time = "";

            XmlNode firstTrackPoint = GetFirstTrackPoint();

            foreach (XmlNode trkNode in firstTrackPoint.ChildNodes)
            {
                if (trkNode.Name == "time")
                {
                    time = trkNode.InnerText;                    
                    break;
                }
            }

            return time;
        }

        public float FirstTrackpointLatitude()
        {
            float latitude = 0.0f;
            System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowCurrencySymbol;

            XmlNode firstTrackPoint = GetFirstTrackPoint();

            if (firstTrackPoint != null)
            {
                foreach (XmlAttribute atribute in firstTrackPoint.Attributes)
                {
                    if (atribute.Name == "lat")
                    {
                        string latitudeString = atribute.Value;
                        float.TryParse(latitudeString, style, CultureInfo.InvariantCulture, out latitude);
                        break;
                    }
                }
            }
            return latitude;
        }

        public float FirstTrackpointLongitude()
        {
            float longitude = 0.0f;
 			System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowCurrencySymbol;

            XmlNode firstTrackPoint = GetFirstTrackPoint();
            if (firstTrackPoint != null)
            {
                foreach (XmlAttribute atribute in firstTrackPoint.Attributes)
                {
                    if (atribute.Name == "lon")
                    {
                        string longitudeString = atribute.Value;
                        float.TryParse(longitudeString, style, CultureInfo.InvariantCulture, out longitude);
                        break;
                    }
                }
            }
            return longitude;
        }

        public int Trackpoints()
        {
            int trackpoints = -1;

            XmlNode firstTrackPoint = GetFirstTrackPoint();
            if (firstTrackPoint != null)
            {
                trackpoints = _trackpoints;
            }
            return trackpoints;
        }

        /// <summary>
        /// Gets a value indicating whether the validation passed.
        /// </summary>
        public bool Passed
        {
            get { return this.passed; }
        }

        /// <summary>
        /// Gets the error text in case the validation didn't pass.
        /// </summary>
        public string Error
        {
            get { return this.error; }
        }
    }
}
/*
<gpx version="1.1" creator="SygicTravelbookLog" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://www.topografix.com/GPX/1/1" xmlns:gpxext="http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1" xsi:schemaLocation="http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1 http://www.p-l-j.org/xmlschemas/GpxExtPlj/v1/GpxExtPlj.xsd http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd">
	<trk>
		<name>..\..\..\TESTlogs\140713_113941.log</name>
		<cmt>created by SygicTravelbookLog</cmt>
		<trkseg>
			<trkpt lat="40.08155" lon="-74.15324">
				<ele>-33</ele>
				<time>2014-07-13T11:39:41</time>

                <extensions>
					<gpxext:TrackPointExtension>
						<gpxext:Speed>77</gpxext:Speed>
						<gpxext:Course>0.0</gpxext:Course>
					</gpxext:TrackPointExtension>
				</extensions>

			</trkpt>
*/