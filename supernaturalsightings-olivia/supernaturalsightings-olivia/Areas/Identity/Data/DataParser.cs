﻿using System;
using supernaturalsightings_olivia.Models;
using System.Text;

namespace supernaturalsightings_olivia.Areas.Identity.Data
{
    public class DataParser
    {
        private static readonly string DATA_FILE = "Areas/Identity/Data/entities.csv";

        static bool IsDataLoaded;

        static public List<Entity> AllEntities;

        //Loads data from the file
        static public void LoadData()
        {
            //Checks to make sure there's data
            if (AllEntities == null || AllEntities.Count == 0)
            {
                IsDataLoaded = false;
            }

            if (IsDataLoaded)
            {
                return;
            }

            //Grab all of the rows out of the spreadsheet
            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText(DATA_FILE))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArray = CSVRowToStringArray(line);
                    if (rowArray.Length > 0)
                    {
                        rows.Add(rowArray);
                    }
                }
            }

            //Puts the headers in their own array and out of the way
            string[] headers = rows[0];
            rows.Remove(headers);

            AllEntities = new List<Entity>();

            //Loops through the rows of data, transforming them into Entities and adding them to a list
            for (int i = 0; i < rows.Count; i++)
            {
                string[] row = rows[i];
                string aName = row[0];
                string aCity = row[1];
                string aState = row[2];
                string aDescription = row[3];
                string aType = row[4];

                Entity newEntity = new Entity(aName, aCity, aState, aDescription, aType);

                AllEntities.Add(newEntity);
            }

            IsDataLoaded = true;
        }

        //Transforms the rows in the CSV into an array of strings. Used in the LoadData function.
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            //Loops through one char at a time looking for separators
            for (int i = 0; i < row.ToCharArray().Length; i++)
            {
                char c = row.ToCharArray()[i];

                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }

    }
}
