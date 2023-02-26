using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurtleWowHardcoreChart.Model;

namespace TurtleWowHardcoreChart.BL;

internal static class PercentageCalculator
{
    internal static double[] GetPercentageData(LevelChartData chartData)
    {
        var totalChars = chartData.CountForLevel.Sum();
        var percentage = new double[5];
        for (int i = 0; i < percentage.Length; i++)
        {
            percentage[i] = (double)chartData.CountForLevel.AsEnumerable().Skip((i + 1) * 10).Take(10).Sum() / totalChars;
        }
        return percentage;
    }

    internal static string BuildResultPercentageText(double[] percentage)
    {
        var tops = new double[5];

        tops[0] = 1;
        for (int i = 1; i < percentage.Length; i++)
        {
            tops[i] = tops[i - 1] - percentage[i - 1];
        }

        var sb = new StringBuilder().Append("Percentages: \n");
        for (int i = 0; i < percentage.Length; i++)
        {
            sb.Append(i + 1).Append("0-").Append(i + 1).Append("9\t").Append(percentage[i].ToString("P")).Append("\tTop ").Append(tops[i].ToString("P")).Append('\n');
        }

        return sb.ToString();
    }
}
