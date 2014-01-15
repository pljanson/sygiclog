/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 29-6-2012 / 31-12-2013
 * Time: 14:17
 */
using System;
using NUnit.Framework;
using System.IO; //File Path

using sygiclog;

namespace TestSygiclog
{
	
	/// <summary>
	/// Reads the logfile provides the "features" for unittest validation
	/// </summary>
	class ValidateLogFile
	{		
		public bool _enabled;
		public string _error;
		
		private	int m_version;
		private	int m_trkpt;
		private string m_corStartTime;
		private string m_startLogDescription;
		private string m_endLogDescription;		
		
		/// <summary>
		/// the constructor opens the logfile and loopa it finding all features in one loop
		/// if the file could not be opened _enabled is false and _error contains the error message. 		
		/// </summary>
		/// <param name="logfilename">logfilename without extention</param>
		public ValidateLogFile(string logfilename)
		{
			TextReader m_reader;
			
			try
			{
				m_reader = new StreamReader(logfilename + ".txt");
				_enabled = true;
				_error = "";				
				
				if (_enabled)
				{
					string readline = m_reader.ReadLine();
					while (readline != null)
					{
						if (readline.StartsWith("version ="))
						{
							string sversion = readline.Substring(10);
							m_version = Convert.ToInt16(sversion);
						}
						else if (readline.StartsWith("wrote trkpt : "))
						{							
							string strkpt = readline.Substring(14);
							m_trkpt = Convert.ToInt16(strkpt);
						}
						else if (readline.StartsWith("Corrected Start Time = "))//Corrected Start Time = 17-7-2012 13:57:55
						{							
							m_corStartTime = readline.Substring(23); // "17-7-2012 13:57:55"			
						}
						else if (readline.StartsWith("startLogDescription p["))//startLogDescription p[27|1B]	[E20  Malmö]
						{
							string sCutoff = readline.Substring(23);
							int idx = sCutoff.IndexOf('[');
							m_startLogDescription = sCutoff.Substring(idx); // "[E20  Malmö]"
						}
						
						else if (readline.StartsWith("endLogDescription p["))//endLogDescription p[49|31]	[E20  Malmö]
						{
							string sCutoff = readline.Substring(21);
							int idx = sCutoff.IndexOf('[');
							m_endLogDescription = sCutoff.Substring(idx); // "[E20  Malmö]"
						}
						
						//read next line
						readline = m_reader.ReadLine();
					}
				}
			}
			catch(System.IO.FileNotFoundException e)
			{
				Console.WriteLine("logfilename not found:" + e.Message);
				m_reader = null;
				_enabled = false;
				_error = e.Message;
			}
		} //ValidateLogFile constructor reader
		
		public int Version
		{
			get
			{
				return m_version;
			}
		}
		public int Trkpt
		{
			get
			{
				return m_trkpt;
			}
		}
		public string CorrectedStartTime
		{
			get
			{
				return m_corStartTime;
			}
		}
		public string StartLogDescription
		{
			get
			{
				return m_startLogDescription;
			}
		}
		public string EndLogDescription
		{
			get
			{
				return m_endLogDescription;
			}
		}		
	} // class ValidateLogFile
		
	[TestFixture]
	public class Test1
	{
		//Usage : Add your test.
		//Assert.AreEqual( int expected, int actual, string message );
		//Assert.AreEqual( float expected, float actual, float tolerance, string message );
		//Assert.Fail("This test fails.");   
		//Assert.Ignore( string message );	

		//Assert.IsTrue( bool condition, string message );			
		//Assert.IsFalse( bool condition, string message );
		//Assert.IsNull( object anObject, string message );
		//Assert.IsNotNull( object anObject, string message );
		
		//Assert.Throws( Type expectedExceptionType, TestSnippet code, string message );
		
		[Test]
		public void LogParserSettings()
		{
			//check the initialized settings
			LogParserSettings settings = new LogParserSettings();

			Assert.AreEqual( string.Empty, settings.AppTitle, " settings.AppTitle = \"\" | string.empty " );
			Assert.AreEqual( string.Empty, settings.InputFileName, " settings.InputFileName = \"\" | string.empty " );
			Assert.AreEqual( string.Empty, settings.FileNameWithoutExtension, " settings.FileNameWithoutExtension = \"\" | string.empty " );
			Assert.AreEqual( ".gpx", settings.XMLExtention, " settings.XMLExtention = \".gpx\" " );			
			Assert.AreEqual( false, settings.All, " settings.All = false" );
			Assert.AreEqual( string.Empty, settings.Tzc, " settings.Tzc =  = \"\" | string.empty " );
			Assert.AreEqual( 0, settings.TzcHours, " settings.TzcHours = 0" );
			Assert.AreEqual( 0, settings.TzcMinutes, " settings.TzcMinutes = 0" );
			Assert.AreEqual( false, settings.Validate, " settings.Validate = false" );
			Assert.AreEqual( false, settings.GpxExt, " settings.GpxExt = false" );
			Assert.AreEqual( false, settings.WaitConsole, " settings.WaitConsole = false" );
			Assert.AreEqual( false, settings.TxtLog, " settings.TxtLog = false" );
			//settings All
			settings.All = true;
			Assert.AreEqual( true, settings.All, " settings.All = true" );
			//settings Title
			settings.AppTitle= "testTitle";
			Assert.AreEqual( "testTitle", settings.AppTitle, " settings.AppTitle = testTitle" );
			//settings ToComment
			string testString = settings.ToComment();
			Assert.AreEqual( "\nm_sAppTitle=[testTitle]\nm_sInputFileName=[]\nm_sXMLExtention=[.gpx]\nm_bAll=[True]\nm_sTzc=[] 0 : 0\nm_bValidate=[False]\nm_bGpxExt=[False]\nm_bWaitConsole=[False]\nm_bTxtlog=[False]\nm_sLogStartTime=[]\n", testString, " settings.ToComment()" );
		} // LogParserSettings
		
		[Test]
		public void ProgramCs()
		{			
			Assert.AreEqual( "Sygiclog v1.5.0", SygicTravelbookLog.sTitle, " SygicTravelbookLog version sTitle" );
			//todo doUsage()
		}
		
		[Test]
		public void LogFile()
		{
			//check the initialized settings
			bool bEnable = false; 
			string sLogFile = "noFile";
				
			LogFile logfile = new LogFile(bEnable, sLogFile);
			Assert.AreEqual( false, logfile.Enabled, "LogFile enabled False");
			//test logfile exist ois false
			
			bEnable = true;
			sLogFile = "test1.logfile";
			LogFile logfile2 = new LogFile(bEnable, sLogFile);
			Assert.AreEqual( true, logfile2.Enabled, "LogFile enabled True");
			//test logfile exist is true
			
			//static int bytes2int32(int b1, int b2, int b3, int b4)
			Assert.AreEqual(0, 			sygiclog.LogFile.bytes2int32(0,0,0,0), "bytes2int32(0,0,0,0)");
			Assert.AreEqual(1, 			sygiclog.LogFile.bytes2int32(1,0,0,0), "bytes2int32(1,0,0,0)");
			Assert.AreEqual(256, 		sygiclog.LogFile.bytes2int32(0,1,0,0), "bytes2int32(0,1,0,0)");
			Assert.AreEqual(65536, 		sygiclog.LogFile.bytes2int32(0,0,1,0), "bytes2int32(0,0,1,0)");
			Assert.AreEqual(16777216, 	sygiclog.LogFile.bytes2int32(0,0,0,1), "bytes2int32(0,0,0,1)");
			Assert.AreEqual(16843009, 	sygiclog.LogFile.bytes2int32(1,1,1,1), "bytes2int32(1,1,1,1)");
		} //LogFile
		
		//now test all versions available :)
		
		[Test]
		public void SygicLogFileV2()
		{
			//--------------------------------			
			// version 2
			//120528_114812.log  //iPhone
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120528_114812.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse V2");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(2, validatelogfile.Version, "version == 2");
					Assert.AreEqual(761, validatelogfile.Trkpt, "trkpt == 761");
					
					Assert.AreEqual("28-5-2012 11:48:12", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time
					Assert.AreEqual("[CV-502  Cullera]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription
					Assert.AreEqual("[Avenida de la Gola del Puchol  Valencia]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");				
				}
			}
			//- - - - - - - - - - - - - - - -
			//120530_090514.log  //iPhone
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120530_090514.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(2, validatelogfile.Version, "version == 2");
					Assert.AreEqual(662, validatelogfile.Trkpt, "trkpt == 662");
					
					Assert.AreEqual("30-5-2012 9:05:14", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time
					Assert.AreEqual("[Estación de Tamarit 28/53  Moncofa]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription
					Assert.AreEqual("[Avenida Mallorca 134/101  Nules]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");				
				}
			}
			//- - - - - - - - - - - - - - - -
			//110715_075330.log
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\110715_075330.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(2, validatelogfile.Version, "version == 2");
					Assert.AreEqual(8967, validatelogfile.Trkpt, "trkpt == 8967");
					
					Assert.AreEqual("15-7-2011 7:53:30", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time
					Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription
					Assert.AreEqual("[Grensstraat -/1  Veurne]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");
				}
			}
			//---
		}// SygicLogFileV2
		
		[Test]
		public void SygicLogFileV3()
		{
			//---------------------------------------------------------------------
			//version 3
						
			//--------------------------------
			//version 3 file
			//120520_113917.log
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120520_113917.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(3, validatelogfile.Version, "version == 3");
					Assert.AreEqual(1592, validatelogfile.Trkpt, "trkpt == 1592");
					
					Assert.AreEqual("20-5-2012 11:39:17", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time
					Assert.AreEqual("[Terno d'Isola]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription
					Assert.AreEqual("[Tangenziale Est  Vimercate]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");
				}
			}
			//- - - - - - - - - - - - - - - -
			//120717_135755.log (E20 Malmo Sweden) (android 11.2.6)
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120717_135755.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(3, validatelogfile.Version, "version == 3");
					Assert.AreEqual(58, validatelogfile.Trkpt, "trkpt == 58");
					
					Assert.AreEqual("17-7-2012 13:57:55", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time
					Assert.AreEqual("[E20  Malmö]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription
					Assert.AreEqual("[E20  Malmö]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");				
				}
			}
			//- - - - - - - - - - - - - - - -
			//120413_074738.log
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120413_074738.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(3, validatelogfile.Version, "version == 3");
					Assert.AreEqual(286 , validatelogfile.Trkpt, "trkpt == 286");
					
					Assert.AreEqual("13-4-2012 7:47:38", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time
					Assert.AreEqual("[Collse Hoefdijk  Nuenen, Gerwen en Nederwetten]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription
					Assert.AreEqual("[N270  Helmond]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");				
				}
			}
			//---
		} // SygicLogFileV3
							
		[Test]
		public void SygicLogFileV4()
		{
			//---------------------------------------------------------------------
			//version 4 file (android, sygic 12.1.0)
			//120801_083158.log (Denmark)
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120801_083158.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(4, validatelogfile.Version, "version == 4");
					Assert.AreEqual(50, validatelogfile.Trkpt, "trkpt == 50");
					
					Assert.AreEqual("1-8-2012 8:31:58", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time = 17-7-2012 13:57:55
					Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription p[27|1B]	[E20  Malmö]
					Assert.AreEqual("[Sønderjyske Motorvej  Vojens]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription p[49|31]	[E20  Malmö]			
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");
				}
			}
			//- - - - - - - - - - - - - - - -
			//120730_141948.log
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\120730_141948.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(4, validatelogfile.Version, "version == 4");
					Assert.AreEqual(1795, validatelogfile.Trkpt, "trkpt == 1795");
					
					Assert.AreEqual("30-7-2012 14:19:48", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time = 17-7-2012 13:57:55
					Assert.AreEqual("[Växjö]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription p[27|1B]	[E20  Malmö]
					Assert.AreEqual("[23  Grimslöv]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription p[49|31]	[E20  Malmö]			
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");
				}
			}
			//---
		} // SygicLogFileV4
			
		[Test]
		public void SygicLogFileV5()
		{
			//---------------------------------------------------------------------
			//version 5 ... android / 13.xx
			//130729_091649.log (Denmark)
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\130729_091649.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(5, validatelogfile.Version, "version == 5");
					Assert.AreEqual(797, validatelogfile.Trkpt, "trkpt == 797");
					
					Assert.AreEqual("29-7-2013 9:16:49", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time = 17-7-2012 13:57:55
					Assert.AreEqual("[Tongelresestraat 423/-  Eindhoven]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription p[27|1B]	[E20  Malmö]
					Assert.AreEqual("[Middenweg 235/224  Venlo]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription p[49|31]	[E20  Malmö]			
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");
				}
			}
			//- - - - - - - - - - - - - - - -
			//131231_164356.log  sygic 13.4.1 ermelo looweg
			{
				LogParserSettings settings = new LogParserSettings();
				settings.InputFileName = "..\\..\\..\\TESTlogs\\131231_164356.log";
				settings.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(settings.InputFileName);
				
				settings.TxtLog = true;
				
				SygicLogFile sygicLogFile = new SygicLogFile(settings);
				Assert.AreEqual(true, sygicLogFile.Parse(), "parse");
				ValidateLogFile validatelogfile = new ValidateLogFile(settings.FileNameWithoutExtension);
				if (validatelogfile._enabled)
				{
					//validate in the log file
					Assert.AreEqual(5, validatelogfile.Version, "version == 5");
					Assert.AreEqual(806, validatelogfile.Trkpt, "trkpt == 806");
					
					Assert.AreEqual("31-12-2013 16:43:56", validatelogfile.CorrectedStartTime, "Corrected Start Tim == ");//Corrected Start Time = 17-7-2012 13:57:55
					Assert.AreEqual("[Kawoepersteeg 21/24  Ermelo]", validatelogfile.StartLogDescription, "StartLogDescription == ");//startLogDescription p[27|1B]	[E20  Malmö]
					Assert.AreEqual("[Looweg 15/10  Ermelo]", validatelogfile.EndLogDescription, "EndLogDescription == ");//endLogDescription p[49|31]	[E20  Malmö]			
				}
				else
				{
					Assert.AreEqual("", validatelogfile._error, "validate error : ");
				}
			}
			//---
		} // SygicLogFileV5
	}
}
