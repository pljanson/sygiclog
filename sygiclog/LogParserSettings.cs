/*
 * Created by SharpDevelop.
 * User: paul
 * Date: 29-6-2012
 * Time: 14:27
 */
using System;

namespace sygiclog
{

/// <summary>
/// Description of LogParserSettings.
/// </summary>
public class LogParserSettings
{
	private string m_sAppTitle;
	private string m_sInputFileName;
	private string m_sFileNameWithoutExtension;
	private string m_sXMLExtention;
	private bool m_bAll;
	private string m_sTzc;
	private int m_nTzcHours;
	private int m_nTzcMinutes;	
	private bool m_bValidate;
	private bool m_bGpxExt;
	private bool m_bWaitConsole;
	private bool m_bTxtlog;
	private string m_sLogStartTime;

	// Default constructor:
	public LogParserSettings()
	{
		m_sAppTitle = string.Empty;
		m_sInputFileName = string.Empty;
		m_sFileNameWithoutExtension = string.Empty;
		m_sXMLExtention = ".gpx";
		m_bAll = false;
		m_sTzc = string.Empty;
		m_nTzcHours = 0;
		m_nTzcMinutes = 0;
		m_bValidate = false;
		m_bGpxExt = false;
		m_bWaitConsole = false;
		m_bTxtlog = false;
		m_sLogStartTime = string.Empty;
	}

	public string ToComment()
	{
		string sComment = "\n" +
			"m_sAppTitle=[" 		+ m_sAppTitle + "]\n" +
			"m_sInputFileName=[" 	+ m_sInputFileName + "]\n" +
			"m_sXMLExtention=[" 	+ m_sXMLExtention + "]\n" +
			"m_bAll=[" 				+ m_bAll.ToString() + "]\n" +
			"m_sTzc=[" 				+ m_sTzc + "] " + m_nTzcHours.ToString() + " : " + m_nTzcMinutes.ToString() + "\n" +
			"m_bValidate=[" 		+ m_bValidate.ToString() + "]\n" +
			"m_bGpxExt=[" 			+ m_bGpxExt.ToString() + "]\n" +
			"m_bWaitConsole=[" 		+ m_bWaitConsole.ToString() + "]\n" +
			//"m_bTxtlog=[" 			+ m_bTxtlog.ToString() + "]\n";
			"m_bTxtlog=[" 			+ m_bTxtlog.ToString() + "]\n" + 
			"m_sLogStartTime=[" 	+ m_sLogStartTime + "]\n";
		return sComment;
	}
	
	public bool All
	{
		get
		{
			return m_bAll;
		}
		set
		{
			m_bAll = value;
		}
	}
	public string Tzc
	{
		get
		{
			return m_sTzc;
		}
		set
		{
			m_sTzc = value;
		}
	}
	public int TzcHours
	{
		get
		{
			return m_nTzcHours;
		}
		set
		{
			m_nTzcHours = value;
		}
	}
	public int TzcMinutes
	{
		get
		{
			return m_nTzcMinutes;
		}
		set
		{
			m_nTzcMinutes = value;
		}
	}
	public string AppTitle
	{
		get
		{
			return m_sAppTitle;
		}
		set
		{
			m_sAppTitle = value;
		}
	}
	public string InputFileName
	{
		get
		{
			return m_sInputFileName;
		}
		set
		{
			m_sInputFileName = value;
		}
	}
	public string FileNameWithoutExtension
	{
		get
		{
			return m_sFileNameWithoutExtension;
		}
		set
		{
			m_sFileNameWithoutExtension = value;
		}
	}	
	public string XMLExtention
	{
		get
		{
			return m_sXMLExtention;
		}
		set
		{
			m_sXMLExtention = value;
		}
	}
	public bool Validate
	{
		get
		{
			return m_bValidate;
		}
		set
		{
			m_bValidate = value;
		}
	}

	public bool GpxExt
	{
		get
		{
			return m_bGpxExt;
		}
		set
		{
			m_bGpxExt = value;
		}
	}

	public bool WaitConsole
	{
		get
		{
			return m_bWaitConsole;
		}
		set
		{
			m_bWaitConsole = value;
		}
	}

	public bool TxtLog
	{
		get
		{
			return m_bTxtlog;
		}
		set
		{
			m_bTxtlog = value;
		}
	}	
	public string LogStartTime
	{
		get
		{
			return m_sLogStartTime;
		}
		set
		{
			m_sLogStartTime = value;
		}
	}
	
} // class LogParserSettings
} // namespace sygiclog
