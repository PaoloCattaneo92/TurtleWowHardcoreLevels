using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurtleWowHardcoreChart.Model;

namespace TurtleWowHardcoreChart.BL;

internal static class ChartHelp
{
    private static string GetChartQueryString(IEnumerable<string> indexes, IEnumerable<int> values)
    {
        var labels = string.Join(",", indexes.Select(i => $"\"{i}\""));
        var data = string.Join(", ", values);
        return "https://quickchart.io/chart?c={type:'bar',data:{labels:[" + labels + "],datasets:[{label:'Levels',data:[" + data + "]}]}}";
    }

    private static string GetChartQueryString(IEnumerable<int> indexes, IEnumerable<int> values)
    {
        var labels = string.Join(",", indexes);
        var data = string.Join(", ", values);
        return "https://quickchart.io/chart?c={type:'bar',data:{labels:[" + labels + "],datasets:[{label:'Levels',data:[" + data + "]}]}}";
    }

    internal async static Task<byte[]> GetChartImage(string query)
    {
        using var client = new HttpClient();
        var result = await client.GetAsync(query);
        var bin = await result.Content.ReadAsByteArrayAsync();
        return bin;
    }

    internal async static Task<byte[]> GetChartImage(ClassChartData data)
    {
        using var client = new HttpClient();
        var query = GetChartQueryString(Constants.CLASS_NAMES, data.CountForClass.Values);
        return await GetChartImage(query);
    }

    internal async static Task<byte[]> GetChartImage(LevelChartData data)
    {
        using var client = new HttpClient();
        var query = GetChartQueryString(data.Levels, data.CountForLevel);
        return await GetChartImage(query);
    }

    internal static LevelChartData CreateLevelChart(IEnumerable<int> heroLevels)
    {
        var countForLevel = new int[Constants.MAX_LEVEL];
        var levels = new int[Constants.MAX_LEVEL];
        for (int i = 0; i < Constants.MAX_LEVEL; i++)
        {
            countForLevel[i] = heroLevels.Count(x => x == i);
            levels[i] = i;
        }

        return new LevelChartData(levels, countForLevel);
    }

    internal static ClassChartData CreateClassChart(IEnumerable<string> immortalHeroes)
    {
        var result = new ClassChartData(new Dictionary<string, int>());
        foreach(var className in Constants.CLASS_NAMES)
        {
            var count = immortalHeroes.Count(x => x == className);
            result.CountForClass.Add(className, count);
        }
        return result;
    }
}
