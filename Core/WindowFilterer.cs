﻿/*
 * Switcheroo - The incremental-search task switcher for Windows.
 * http://www.switcheroo.io/
 * Copyright 2009, 2010 James Sulak
 * Copyright 2014 Regin Larsen
 * 
 * Switcheroo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Switcheroo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Switcheroo.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.Linq;
using Switcheroo.Core.Matchers;
using IniParser;
using IniParser.Model;
using System;

namespace Switcheroo.Core
{
    public class WindowFilterer
    {
        public IEnumerable<FilterResult<T>> Filter<T>(WindowFilterContext<T> context, string query) where T : IWindowText
        {
            var filterText = query;
            string processFilterText = null;
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Switcheroo.ini");
            string strSpliter = data["config"]["Spliter"];
            // . is used to seperate the input string in to two parts, part1--processFilterText, part2--filterText
            char charSpliter = strSpliter.ToCharArray()[0];            
            var queryParts = query.Split(new[] { charSpliter }, 2);
           
            if (queryParts.Length == 2)
            {
                processFilterText = queryParts[0];
                if (processFilterText.Length == 0)
                {
                    processFilterText = context.ForegroundWindowProcessTitle;
                }
                processFilterText = data["config"][queryParts[0]];
                filterText = queryParts[1];
            }

            

            return context.Windows
                .Select(
                    w =>
                        new
                        {
                            Window = w,
                            ResultsTitle = Score(w.WindowTitle, filterText),
                            ResultsProcessTitle = Score(w.ProcessTitle, processFilterText ?? filterText)
                        })
                .Where(r =>
                {
                    if (processFilterText == null)
                    {
                        return r.ResultsTitle.Any(wt => wt.Matched) || r.ResultsProcessTitle.Any(pt => pt.Matched);
                    }
                    return r.ResultsTitle.Any(wt => wt.Matched) && r.ResultsProcessTitle.Any(pt => pt.Matched);
                })
                .OrderByDescending(r => r.ResultsTitle.Sum(wt => wt.Score) + r.ResultsProcessTitle.Sum(pt => pt.Score))
                .Select(
                    r =>
                        new FilterResult<T>
                        {
                            AppWindow = r.Window,
                            WindowTitleMatchResults = r.ResultsTitle,
                            ProcessTitleMatchResults = r.ResultsProcessTitle
                        });
            
        }

        private static List<MatchResult> Score(string title, string filterText)
        {
            var startsWithMatcher = new StartsWithMatcher();
            var containsMatcher = new ContainsMatcher();
            var significantCharactersMatcher = new SignificantCharactersMatcher();
            var individualCharactersMatcher = new IndividualCharactersMatcher();

            var results = new List<MatchResult>
            {
                startsWithMatcher.Evaluate(title, filterText),
                significantCharactersMatcher.Evaluate(title, filterText),
                containsMatcher.Evaluate(title, filterText),
                individualCharactersMatcher.Evaluate(title, filterText)
            };
             
            return results;
        }
    }

    public class WindowFilterContext<T> where T : IWindowText
    {
        public string ForegroundWindowProcessTitle { get; set; }
        public IEnumerable<T> Windows { get; set; } 
    }
}