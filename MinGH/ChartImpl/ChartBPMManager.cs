﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace MinGH.ChartImpl
{
    class ChartBPMManager
    {
        // Reads in the bpm changes of a chart file (can pass either the whole input file
        // or the desired part)
        public List<BPMChange> AddBPMChanges(string inputFile)
        {
            List<BPMChange> BPMChangeListToReturn = new List<BPMChange>();

            // Single out the BPM section via regular expressions
            string pattern = Regex.Escape("[") + "SyncTrack]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(inputFile, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            while ((current_line = pattern_stream.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                current_line = current_line.Trim();
                parsed_line = current_line.Split(' ');

                // If a valid change is found, add it to the list
                if (parsed_line.Length == 4)
                {
                    if (parsed_line[2] == "B")
                        BPMChangeListToReturn.Add(new BPMChange(Convert.ToUInt32(parsed_line[0]), Convert.ToInt64(parsed_line[3])));
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return BPMChangeListToReturn;
        }
    }
}