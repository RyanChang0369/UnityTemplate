using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

/// <summary>
/// Helper class to build valid CSV, to be used with Newtonsoft's JsonTextReader
/// (but can also be used on its own). Includes support for arrays.
///
/// <br/>
///
/// Note: This has not been tested with nested arrays (arrays within arrays),
/// but they should work fine.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class CSVArrayBuilder
{
    #region Variables
    /// <summary>
    /// The internal dictionary of <see cref="CSVBuilder"/>s. Each builder
    /// represents one array. There also exists one builder, with the key of the
    /// empty string, that holds all non-array values.
    /// </summary>
    private Dictionary<string, CSVBuilder> builders = new();

    /// <summary>
    /// Maps fully qualified headers to shorter headers.
    /// </summary>
    private Dictionary<string, string> headerMap = new();

    #region Regex
    /// <summary>
    /// The regex that finds arrays.
    /// </summary>
    private static readonly Regex rx_array = new(@"(\w+)(\[\d+\])");

    /// <summary>
    /// The regex that finds array indices.
    /// </summary>
    private static readonly Regex rx_index = new(@"\[\d+\]");
    #endregion
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new <see cref="CSVArrayBuilder"/> from the specified <see
    /// cref="Newtonsoft.Json.JsonTextReader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="Newtonsoft.Json.JsonTextReader"/>
    /// that is used to create the CSV. The reader is not closed after it is
    /// done.</param>
    public CSVArrayBuilder(JsonTextReader reader)
    {
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonToken.Raw:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Bytes:
                case JsonToken.Date:
                    Add(reader.Path, reader.Value.ToString());
                    break;
            }
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds a data member to the csv. Header is allowed to contain array
    /// indices.
    /// </summary>
    /// <param name="header">The header that categorizes the data, formatted as
    /// <c>array_title[index]</c>, where <c>array_title</c> is a string that
    /// represents the name of the array and <c>index</c> is an integer the
    /// represents the index into that array.
    /// </param>
    /// <param name="data">The data member to be added to the csv, under the
    /// specified header.</param>
    public void Add(string header, string data)
    {
        // First thing to do is to check if this is an array.
        var matches = rx_array.Matches(header);

        if (matches.Any())
        {
            // We have an array.
            // We only need the last match.
            Match m = matches.Last();

            // The number as a string. The [1..^1] is an index operator
            // (https://learn.microsoft.com/en-us/dotnet/api/system.index).
            string num = m.Groups[2].ToString()[1..^1];

            // Header and indexes for the array.
            string arrHeader = m.Groups[1].Value;
            int arrIndex = Int32.Parse(num);

            // The portion of the header after the array header.
            Index ph_from = m.Groups[2].Index + m.Groups[2].Length + 1;
            string postHeader = header[ph_from..];

            // The portion of the header before the array header. The preHeader
            // is also a fully qualified identifier for this specific array.
            Index ph_to = m.Groups[0].Index;
            string preHeader = rx_index.Replace(header[..ph_to], "");

            // Add stuff to the builders.
            builders.GetOrCreate(preHeader).Add(postHeader, data);
        }
        else
        {
            // We have a regular property. Simply add it to some default value.
            builders.GetOrCreate("").Add(header, data);
        }
    }

    public override string ToString()
    {
        return String.Join("\r\n\r\n",
            builders.Keys.Select(k => builders[k].ToString()));
        // return "";
    }
    #endregion
}