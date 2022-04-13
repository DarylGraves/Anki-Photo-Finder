﻿using System.IO;
namespace Core;

public class Data
{
    public string[] csv { get; }
    public char delimiter { get; }
    public string[] headers { get; }
    
    // Constructor
    public Data(string path, char delimiter)
    {
        string[] csvToValidate;
        
        // Load CSV
        if (File.Exists(path))
        {
            csvToValidate = File.ReadAllLines(path);               
        }
        else
        {
            throw new FileNotFoundException($"{path} not found!");
        }

        // Validate CSV is valid
        
        // First check there is more than just a header
        if (csvToValidate.Length > 1)
        {
            // Then check it includes the delimiter 
            if (csvToValidate[0].Split(delimiter).Count() > 1)
            {
                // This File was accepted 
                this.csv = csvToValidate;
                this.delimiter = delimiter;

                this.headers = csvToValidate[0].Split(delimiter);
            }
            else
            {
                throw new Exception("Invalid Csv File - Delimiter was not found");
            }
        }
        else
        {
            throw new Exception("Invalid Csv File - File was less than 2 lines");
        }
    }

    ///<summary>
    /// Returns all the heads of the loaded Csv as a string array.
    ///</summary>
    public string[] GetHeaders()
    {
        return headers;
    }

    public int FindColumnNo(string headerToFind)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i] == headerToFind)
            {
                return i;
            }
        }
        
        // Header doesn't exist
        return -1;
    }


    //TODO: CreateCollection()
        // Do we want to call this one something else? The Method will accept a column and then create the collection using the column as the index
    //TODO: GetNextWord()
    //TODO: GetPreviousWord()
    //TODO: Save()
        // Saves the Collection down as a CSV ready for Anki
}