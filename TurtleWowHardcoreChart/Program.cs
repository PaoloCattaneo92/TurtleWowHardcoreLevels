using HtmlAgilityPack;
using System.Diagnostics;
using System.Text;
using TurtleWowHardcoreChart.BL;

const int MAX_LEVEL = 60;

var fileName = GetFileName(args);

if(fileName == null)
{
    throw new Exception("Cannot find valid HTML file in this folder, you can add full path to file as first and only parameter to this executable");
}

var htmlData = File.ReadAllText(fileName);
var document = new HtmlDocument();
document.LoadHtml(htmlData);

//var aliveHeroesLevels = HtmlHelp.GetAliveHeroes(document)
//    .Select(hero => int.Parse(HtmlHelp.ParseLevel(hero)))
//    .Where(level => level > 0);

//var aliveChartData = ChartHelp.CreateLevelChart(aliveHeroesLevels);
//var alivePercentage = PercentageCalculator.GetPercentageData(aliveChartData);
//var alivePercentageText = PercentageCalculator.BuildResultPercentageText(alivePercentage);
//File.WriteAllText("alive_percentages.txt", alivePercentageText);
//var aliveImage = await ChartHelp.GetChartImage(aliveChartData);
//File.WriteAllBytes("alive_image.png", aliveImage);

//var fallenHeroesLevels = HtmlHelp.GetFallenHeroes(document)
//    .Select(hero => int.Parse(HtmlHelp.ParseLevel(hero)))
//    .Where(level => level > 0);
//var fallenChartData = ChartHelp.CreateLevelChart(fallenHeroesLevels);
//var fallenPercentage = PercentageCalculator.GetPercentageData(fallenChartData);
//var fallenPercentageText = PercentageCalculator.BuildResultPercentageText(fallenPercentage);
//File.WriteAllText("fallen_percentages.txt", fallenPercentageText);
//var fallenImage = await ChartHelp.GetChartImage(fallenChartData);
//File.WriteAllBytes("fallen_image.png", fallenImage);

var immortalHeroes = HtmlHelp.GetImmortalHeroes(document)
    .Select(HtmlHelp.ParseClass)
    .Where(c => c != "-1");
var immortalChartData = ChartHelp.CreateClassChart(immortalHeroes);
var immortalImage = await ChartHelp.GetChartImage(immortalChartData);
File.WriteAllBytes("immortal_image.png", immortalImage);


static string? GetFileName(string[] args)
{
    string fileName = string.Empty;
    if (args?.Length > 0)
    {
        return args[0];
    }

    var htmlFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.htm|*.html");
    return htmlFiles.FirstOrDefault();
}


