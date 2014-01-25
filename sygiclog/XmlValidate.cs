//-----------------------------------------------------------------------
// <copyright file="XmlValidate.cs" company="PLJ">
// Copyright (C) 2014, Paul Janson, LGPL 2.1
// </copyright>
//-----------------------------------------------------------------------
/*
 * Created by SharpDevelop.
 * User: plj
 * Date: 30-6-2012
 * Time: 15:02
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
namespace Sygiclog
{
using System;
using System.Xml;
using System.Xml.Schema; // validating

/// <summary>
/// Description of XmlValidate.
/// </summary>
public class XmlValidate
{
    /// <summary>
    /// the error counter
    /// </summary>
    private int errorsCount = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlValidate" /> class.
    /// Empty constructor.
    /// </summary>
    public XmlValidate()
    {
        // empty constructor for now
    } // xmlValidate constructor

    /// <summary>
    /// Validate the file using the schema files
    /// </summary>
    /// <param name="xmlFile">the xml file to validate</param>
    /// <param name="xsdFile">the main schema file to validate with</param>
    /// <param name="xsdExtFile">optional additional schema</param>
    public void Validate(string xmlFile, string xsdFile, string xsdExtFile)
    {
        Console.WriteLine("\n[validate]: xml:[" + xmlFile + "] with xsd:[" + xsdFile + "] with xsdext:[" + xsdExtFile + "]");

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add(null, xsdFile);

        // if (xsdextFile != string.Empty)
        if (!string.IsNullOrEmpty(xsdExtFile))
        {
            settings.Schemas.Add(null, xsdExtFile);
        }

        settings.ValidationType = ValidationType.Schema;

        try
        {
            XmlReader reader = XmlReader.Create(xmlFile, settings);
            XmlDocument rootDoc = new XmlDocument();
            rootDoc.Load(reader);
            ValidationEventHandler eventHandler = new ValidationEventHandler(this.ValidationEventHandler);

            rootDoc.Validate(eventHandler);
        }
        catch (XmlSchemaValidationException e)
        {
            Console.WriteLine("            Error: " + e.Message + "line:" + e.LineNumber + " pos:" + e.LinePosition);
            this.errorsCount++;
        }

        // Final feedback
        if (this.errorsCount == 0)
        {
            Console.WriteLine("            Validation completed successfully.");
        }
        else if (this.errorsCount == 1)
        {
            Console.WriteLine("            Validation Failed: there was 1 error found");
        }
        else
        {
            Console.WriteLine("            Validation Failed: there were " + this.errorsCount + " errors found");
        }
    } // method validate

    /// <summary>
    /// The event handler for the schema validation 
    /// </summary>
    /// <param name="sender">the sender</param>
    /// <param name="args">the args</param>
    public void ValidationEventHandler(object sender, ValidationEventArgs args)
    {
        Console.WriteLine("ValidationEventHandler Error: " + args.Message);
        this.errorsCount++;
    } // method ValidationEventHandler
} // class xmlValidate
} // namespace sygiclog
