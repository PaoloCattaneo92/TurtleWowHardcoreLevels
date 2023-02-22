using HtmlAgilityPack;
using System.Text;

const int MAX_LEVEL = 60;

var fileName = GetFileName(args);

if(fileName == null)
{
    throw new Exception("Cannot find valid HTML file in this folder, you can add full path to file as first and only parameter to this executable");
}

var htmlData = File.ReadAllText(fileName);
var document = new HtmlDocument();
document.LoadHtml(htmlData);

var levelOfCharacters = ParseCharacters(document)
    .Select(hero => int.Parse(ParseLevel(hero)))
    .Where(level => level > 0);

var countForLevel = new int[MAX_LEVEL];
var levels = new int[MAX_LEVEL];
for (int i = 0; i < MAX_LEVEL; i++)
{
    countForLevel[i] = levelOfCharacters.Count(x => x == i);
    levels[i] = i;
}

var totalChars = countForLevel.Sum();

var percentage = new double[5];
for (int i = 0; i < percentage.Length; i++)
{
    percentage[i] = (double)countForLevel.AsEnumerable().Skip((i+1)*10).Take(10).Sum() / totalChars;
}

StringBuilder sb = BuildResultPercentageText(percentage);
File.WriteAllText("result_percentages.txt", sb.ToString());

using var client = new HttpClient();

var labels = string.Join(",", levels);
var data = string.Join(", ", countForLevel);
var result = await client.GetAsync("https://quickchart.io/chart?c={type:'bar',data:{labels:[" + labels + "],datasets:[{label:'Levels',data:[" + data + "]}]}}");
var bin = await result.Content.ReadAsByteArrayAsync();

File.WriteAllBytes("result_image.png", bin);

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
static List<HtmlNode> ParseCharacters(HtmlDocument document)
{
    return document.DocumentNode.ChildNodes[2].ChildNodes[3].ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[3].ChildNodes
        .Where(h => h.Name == "tr")
        .ToList();
}

static string ParseLevel(HtmlNode hero)
{
    return hero.ChildNodes[1].ChildNodes.FirstOrDefault(c => c.Name == "span")?.ChildNodes[0]?.InnerHtml ?? "-1";
}

static StringBuilder BuildResultPercentageText(double[] percentages)
{
    var tops = new double[5];

    tops[0] = 1;
    for (int i = 1; i < percentages.Length; i++)
    {
        tops[i] = tops[i-1] - percentages[i-1];
    }

    var sb = new StringBuilder().Append("Percentages: \n");
    for (int i = 0; i < percentages.Length; i++)
    {
        sb.Append(i+1).Append("0-").Append(i+1).Append("9\t").Append(percentages[i].ToString("P")).Append("\tTop ").Append(tops[i].ToString("P")).Append('\n');
    }

    return sb;
}