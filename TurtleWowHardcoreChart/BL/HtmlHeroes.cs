using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleWowHardcoreChart.BL;

internal static class HtmlHelp
{
    internal static List<HtmlNode> GetAliveHeroes(HtmlDocument document)
    {
        return document.DocumentNode.ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[3].ChildNodes
            .Where(h => h.Name == "tr")
            .ToList();
    }

    internal static List<HtmlNode> GetFallenHeroes(HtmlDocument document)
    {
        return document.DocumentNode.ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].ChildNodes[5].ChildNodes[1].ChildNodes[3].ChildNodes
            .Where(h => h.Name == "tr")
            .ToList();
    }

    internal static List<HtmlNode> GetImmortalHeroes(HtmlDocument document)
    {
        return document.DocumentNode.ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].ChildNodes[7].ChildNodes[1].ChildNodes[3].ChildNodes.Where(h => h.Name == "tr").ToList();
    }

    // <img src="Turtle%20WoW%20%E2%80%94%20Hardcore_files/Rogue.png" width="25">
    internal static string ParseClass(HtmlNode hero)
    {
        var images = hero.ChildNodes[1].ChildNodes.Where(c => c.Name == "img").ToList();
        if (images.Count < 2)
        {
            return "-1";
        }

        return images[1].Attributes
            .FirstOrDefault(a => a.Name == "src")
            ?.Value.Split("/")
            ?.LastOrDefault()
            ?.Replace(".png", "") ?? "-1";
    }

    internal static string ParseLevel(HtmlNode hero)
    {
        return hero.ChildNodes[1].ChildNodes.FirstOrDefault(c => c.Name == "span")?.ChildNodes[0]?.InnerHtml ?? "-1";
    }
}
